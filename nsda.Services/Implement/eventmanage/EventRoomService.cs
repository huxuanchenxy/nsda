using nsda.Models;
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
    /// 赛事教室管理
    /// </summary>
    public class EventRoomService: IEventRoomService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventRoomService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public bool Insert(out string msg)
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
                LogUtils.LogError("EventRoomService.Insert", ex);
            }
            return flag;
        }

        public bool Update(out string msg)
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
                LogUtils.LogError("EventRoomService.Update", ex);
            }
            return flag;
        }

        public bool Delete(int id, string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventroom room = _dbContext.Get<t_eventroom>(id);
                if (room != null)
                {
                }
                else {

                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.Delete", ex);
            }
            return flag;
        }
    }
}
