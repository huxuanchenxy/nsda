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
    /// <summary>
    /// 淘汰赛设置
    /// </summary>
    public class EventknockoutSettingsService:IEventknockoutSettingsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventknockoutSettingsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //淘汰赛设置
        public bool Settints(List<EventknockoutSettingsRequest> request, out string msg)
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
                LogUtils.LogError("EventknockoutSettingsService.Insert", ex);
            }
            return flag;
        }
        //淘汰赛设置详情
        public List<EventknockoutSettingsResponse> ListKnockoutSettings(int eventId)
        {
            List<EventknockoutSettingsResponse> list = new List<EventknockoutSettingsResponse>();
            try
            {
                var sql = $@"select a.*,b.name EventGroupName from t_eventknockoutsettings a
                            inner join t_eventgroup b on a.eventGroupId=b.id
                            where a.isdelete=0 and a.eventId={eventId}";
                var data = _dbContext.Query<EventknockoutSettingsResponse>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var itemdata = _dbContext.Select<t_eventknockout>(c => c.settingsId == item.Id).ToList();
                        if (itemdata != null && itemdata.Count > 0)
                        {
                            foreach (var items in itemdata)
                            {
                                EventKnockoutResponse response = new EventKnockoutResponse
                                {
                                    EventGroupId=items.eventGroupId,
                                    EventId=items.eventId,
                                    KnockoutStatus=items.knockoutStatus,
                                    KnockoutType=items.knockoutType,
                                    SettingsId=items.settingsId,
                                    TrainerCount=items.trainerCount,
                                    Id = items.id                                   
                                };
                                var itemsdata = _dbContext.Select<t_eventknockoutdetail>(c => c.knockoutId == items.id).ToList();
                                if (itemsdata != null && itemsdata.Count > 0)
                                {
                                    foreach (var itemss in itemsdata)
                                    {
                                        EventKnockoutDetailResponse responses = new EventKnockoutDetailResponse
                                        {
                                            KnockoutId =itemss.knockoutId,
                                            EndTime = itemss.endtime,
                                            Screenings = itemss.screenings,
                                            EventGroupId = itemss.eventGroupId,
                                            EventId = itemss.eventId,
                                            Id = itemss.id,
                                            StartTime = itemss.starttime
                                        };
                                        response.ListKnockoutDetail.Add(responses);
                                    }
                                }
                                item.ListKnockout.Add(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventknockoutSettingsService.ListKnockoutSettings", ex);
            }
            return list;
        }
    }
}
