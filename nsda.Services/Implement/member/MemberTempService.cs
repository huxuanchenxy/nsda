using nsda.Repository;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;

namespace nsda.Services.Implement.member
{
    public class MemberTempService: IMemberTempService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberTempService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //新增临时选手
        public bool InsertTempPlayer(TempPlayerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.InsertTempPlayer", ex);
            }
            return flag;
        }
        //新增临时裁判
        public bool InsertTempReferee(TempRefereeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.InsertTempReferee", ex);
            }
            return flag;
        }
        //临时选手绑定
        public bool BindTempPlayer(BindTempPlayerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.BindTempPlayer", ex);
            }
            return flag;
        }
        //临时教练绑定 
        public bool BindTempReferee(BindTempPlayerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.BindTempReferee", ex);
            }
            return flag;
        }
        //临时会员数据列表
        public PagedList<MemberTempResponse> List(TempMemberQueryRequest request)
        {
            PagedList<MemberTempResponse> list = new PagedList<MemberTempResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.List", ex);
            }
            return list;
        }
    }
}
