using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.member;
using nsda.Services.Contract.referee;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.referee
{
    /// <summary>
    /// 裁判报名管理
    /// </summary>
    public class RefereeSignUpService: IRefereeSignUpService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public RefereeSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public bool Apply(int eventId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (eventId <= 0)
                {
                    msg = "请选择赛事";
                    return flag;
                }

                t_event t_event = _dbContext.Get<t_event>(eventId);
                if (t_event != null)
                {
                    //进一步判断赛事状态
                    var data = _dbContext.Select<t_referee_signup>(c => c.eventId == eventId && c.memberId == memberId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        msg = "您已提交过申请";
                        return flag;
                    }
                    _dbContext.Insert(new t_referee_signup {
                         eventId=eventId,
                         isTemp=false,
                         memberId=memberId,
                         refereeSignUpStatus=Model.enums.RefereeSignUpStatusEm.待审核
                    });
                }
                else
                {
                    msg = "赛事有误";
                    return flag;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.Apply", ex);
            }
            return flag;
        }

        //裁判报名列表
        public PagedList<RefereeSignUpResponse> List(RefereeSignUpQueryRequest request)
        {
            PagedList<RefereeSignUpResponse> list = new PagedList<RefereeSignUpResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.List", ex);
            }
            return list;
        }
    }
}
