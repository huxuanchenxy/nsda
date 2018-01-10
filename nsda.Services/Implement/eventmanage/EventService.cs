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
    /// <summary>
    /// 赛事管理
    /// </summary>
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
                _dbContext.BeginTransaction();


                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
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
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.Edit", ex);
            }
            return flag;
        }

        public bool SettingLevel(int id, EventLevelEm eventLevel, int sysUserId, out string msg)
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
                LogUtils.LogError("EventService.SettingLevel", ex);
            }
            return flag;
        }
    }
}
