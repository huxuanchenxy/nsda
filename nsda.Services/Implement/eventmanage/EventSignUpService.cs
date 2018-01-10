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
    public class EventSignUpService: IEventSignUpService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public bool SignUp(out string msg)
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
                LogUtils.LogError("EventRoomService.SignUp", ex);
            }
            return flag;
        }

        public bool BatchSignUp(out string msg)
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
                LogUtils.LogError("EventRoomService.SignUp", ex);
            }
            return flag;
        }
    }
}
