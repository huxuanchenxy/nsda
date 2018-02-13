using nsda.Repository;
using nsda.Services.Contract.eventmanage;
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
using nsda.Models;

namespace nsda.Services.Implement.eventmanage
{
    public class EventRegularAwardsService: IEventRegularAwardsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventRegularAwardsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //奖项设置
        public bool Settings(EventRegularAwardsRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                try
                {
                    _dbContext.BeginTransaction();
                    _dbContext.Execute($"delete from t_event_regularawards where eventId={request.EventId} and eventGroupId={request.EventGroupId}");
                    _dbContext.Insert(new t_event_regularawards {
                        eventGroupId=request.EventGroupId,
                        eventId=request.EventId,
                        group=request.Group,
                        personal=request.Personal 
                    });
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("EventRegularAwardsService.SettingsTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRegularAwardsService.Settings", ex);
            }
            return flag;
        }

        //设置详情
        public EventRegularAwardsResponse Detail(int eventId, int eventGroupId)
        {
            EventRegularAwardsResponse response = new EventRegularAwardsResponse {
                 EventGroupId=eventGroupId,
                 EventId=eventId
            };
            try
            {
                var detail = _dbContext.Select<t_event_regularawards>(c=>c.eventId==eventId&&c.eventGroupId==eventGroupId).FirstOrDefault();
                if (detail != null)
                {
                    response.Personal = detail.personal;
                    response.Group = detail.group;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventRegularAwardsService.Detail", ex);
            }
            return response;
        }
    }
}
