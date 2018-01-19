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
    /// 循环赛设置
    /// </summary>
    public  class EventCyclingRaceSettingsService: IEventCyclingRaceSettingsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventCyclingRaceSettingsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //设置循环赛
        public bool Settints(List<EventCyclingRaceSettingsRequest> request, out string msg)
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
                LogUtils.LogError("EventCyclingRaceSettingsService.Insert", ex);
            }
            return flag;
        }
        //循环赛详情
        public List<EventCyclingRaceSettingsResponse> ListCyclingRaceSettings(int eventId)
        {
            List<EventCyclingRaceSettingsResponse> list = new List<EventCyclingRaceSettingsResponse>();
            try
            {
                var sql = $@"select a.*,b.name EventGroupName from t_eventcyclingracesettings a
                            inner join t_eventgroup b on a.eventGroupId=b.id
                            where a.isdelete=0 and a.eventId={eventId}";
                var data = _dbContext.Query<EventCyclingRaceSettingsResponse>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var itemdata = _dbContext.Select<t_eventcyclingrace>(c => c.settingsId == item.Id).ToList();
                        if (itemdata != null && itemdata.Count > 0)
                        {
                            foreach (var items in itemdata)
                            {
                                EventCyclingRaceResponse response = new EventCyclingRaceResponse
                                {
                                  CurrentRound=items.currentround,
                                  CyclingRaceStatus=items.cyclingRaceStatus,
                                  EventGroupId=items.eventGroupId,
                                  EventId=items.eventId,
                                  NextRound=items.nextround,
                                  PairRule=items.pairRule,
                                  Id=items.id,
                                  SettingsId=items.settingsId 
                                };
                                var itemsdata = _dbContext.Select<t_eventcyclingracedetail>(c => c.cyclingraceId == items.id).ToList();
                                if (itemsdata != null && itemsdata.Count > 0)
                                {
                                    foreach (var itemss in itemsdata)
                                    {
                                        EventCyclingRaceDetailResponse responses = new EventCyclingRaceDetailResponse
                                        {
                                             CyclingRaceId=itemss.cyclingraceId,
                                             EndTime=itemss.endtime,
                                             Screenings=itemss.screenings,
                                             EventGroupId=itemss.eventGroupId,
                                             EventId=itemss.eventId,
                                             Id=itemss.id,
                                             StartTime=itemss.starttime
                                        };
                                        response.ListCyclingRaceDetail.Add(responses);
                                    }
                                 }
                                item.ListCyclingRace.Add(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventCyclingRaceSettingsService.ListCyclingRaceSettings", ex);
            }
            return list;
        }
    }
}
