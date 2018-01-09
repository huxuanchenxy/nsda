using Dapper;
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
using nsda.Model.enums;

namespace nsda.Services.Implement.eventmanage
{
    public class EventService: IEventService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
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
                LogUtils.LogError("EventService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.Edit", ex);
            }
            return flag;
        }

        public bool SettingLevel(int id, EventTypeEm eventType, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.SettingLevel", ex);
            }
            return flag;
        }
    }
}
