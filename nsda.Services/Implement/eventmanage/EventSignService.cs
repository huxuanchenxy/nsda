using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事签到管理
    /// </summary>
    public class EventSignService: IEventSignService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventSignService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //裁判 选手签到
        public bool Sign(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.Sign", ex);
            }
            return flag;
        }
        //赛事管理员 批量签到
        public bool BatchSign(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.BatchSign", ex);
            }
            return flag;
        }
        //选手签到列表
        public void PlayerSignList()
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.PlayerSignList", ex);
            }
        }
        //裁判签到列表
        public void RefereeSignList()
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.RefereeSignList", ex);
            }
        }
    }
}
