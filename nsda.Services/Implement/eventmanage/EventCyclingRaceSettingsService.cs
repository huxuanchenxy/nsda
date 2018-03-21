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
using nsda.Model.enums;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 循环赛设置
    /// </summary>
    public class EventCyclingRaceSettingsService : IEventCyclingRaceSettingsService
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
                if (request == null || request.Count == 0)
                {
                    msg = "请核对参数后再保存";
                    return flag;
                }
                var model = request.FirstOrDefault();
                if (model.EventId <= 0)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                //循环遍历判断参数合法性
                foreach (var item in request)
                {
                    if (item.StartRange < 0)
                    {
                        msg = "开始打分区间有误";
                        break;
                    }

                    if (item.EndRange < 0)
                    {
                        msg = "结束打分区间有误";
                        break;
                    }

                    if (item.EndRange < item.StartRange)
                    {
                        msg = "打分区间有误";
                        break;
                    }

                    if (item.ListCyclingRace == null || item.ListCyclingRace.Count == 0)
                    {
                        msg = "循环赛轮次有误";
                        break;
                    }

                    string message = string.Empty;
                    foreach (var items in item.ListCyclingRace)
                    {
                        if (item.Screenings != items.ListCyclingRaceDetail.Count)
                        {
                            message = "Flight信息有误";
                            break;
                        }
                        string messages = string.Empty;
                        foreach (var itemss in items.ListCyclingRaceDetail)
                        {
                            if (itemss.CompTime == DateTime.MaxValue || itemss.CompTime == DateTime.MinValue)
                            {
                                messages = "Flight开始时间有误";
                                break;
                            }
                        }
                        if (messages.IsNotEmpty())
                        {
                            message = messages;
                            break;
                        }
                    }

                    if (message.IsNotEmpty())
                    {
                        msg = message;
                        break;
                    }
                }

                if (msg.IsNotEmpty())
                {
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    _dbContext.Execute($"delete from t_event_cycling_settings where eventId={model.EventId}");
                    _dbContext.Execute($"delete from t_event_cycling where eventId={model.EventId}");
                    _dbContext.Execute($"delete from t_event_cycling_detail where eventId={model.EventId}");

                    foreach (var item in request)
                    {
                        //循环赛设置表
                        int settingsId = _dbContext.Insert(new t_event_cycling_settings
                        {
                            endrange = item.EndRange,
                            startrange = item.StartRange,
                            eventGroupId = item.EventGroupId,
                            eventId = item.EventId,
                            isallow = item.IsAllow,
                            screenings = item.Screenings,
                            totalround = item.ListCyclingRace.Count,
                        }).ToObjInt();
                        //循环赛表
                        foreach (var items in item.ListCyclingRace)
                        {
                            int cyclingraceId = _dbContext.Insert(new t_event_cycling {
                                    currentround=items.CurrentRound,
                                    cyclingRaceStatus= CyclingRaceStatusEm.未开始,
                                    eventGroupId=items.EventGroupId,
                                    eventId=items.EventId,
                                    nextround=items.NextRound,
                                    pairRule=items.PairRule,
                                    settingsId=settingsId
                            }).ToObjInt();

                            foreach (var itemss in items.ListCyclingRaceDetail)
                            {
                                _dbContext.Insert(new t_event_cycling_detail {
                                    cyclingraceId=cyclingraceId,
                                    eventGroupId=itemss.EventGroupId,
                                    eventId=itemss.EventId,
                                    screenings=itemss.Screenings,
                                    comptime=itemss.CompTime
                                });
                            }
                        }
                    }

                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("EventCyclingRaceSettingsService.InsertTran", ex);
                }

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
        public List<EventCyclingRaceSettingsResponse> CyclingRaceSettings(int eventId)
        {
            List<EventCyclingRaceSettingsResponse> data = new List<EventCyclingRaceSettingsResponse>();
            try
            {
                var sql = $@"select * from t_event_cycling_settings where isdelete=0 and eventId={eventId}";
                data = _dbContext.Query<EventCyclingRaceSettingsResponse>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var itemdata = _dbContext.Select<t_event_cycling>(c => c.settingsId == item.Id).ToList();
                        if (itemdata != null && itemdata.Count > 0)
                        {
                            foreach (var items in itemdata)
                            {
                                EventCyclingRaceResponse response = new EventCyclingRaceResponse
                                {
                                    CurrentRound = items.currentround,
                                    CyclingRaceStatus = items.cyclingRaceStatus,
                                    EventGroupId = items.eventGroupId,
                                    EventId = items.eventId,
                                    NextRound = items.nextround,
                                    PairRule = items.pairRule,
                                    Id = items.id,
                                    SettingsId = items.settingsId
                                };
                                var itemsdata = _dbContext.Select<t_event_cycling_detail>(c => c.cyclingraceId == items.id).ToList();
                                if (itemsdata != null && itemsdata.Count > 0)
                                {
                                    foreach (var itemss in itemsdata)
                                    {
                                        EventCyclingRaceDetailResponse responses = new EventCyclingRaceDetailResponse
                                        {
                                            CyclingRaceId = itemss.cyclingraceId,
                                            Screenings = itemss.screenings,
                                            EventGroupId = itemss.eventGroupId,
                                            EventId = itemss.eventId,
                                            Id = itemss.id,
                                            CompTime = itemss.comptime
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
                LogUtils.LogError("EventCyclingRaceSettingsService.CyclingRaceSettings", ex);
            }
            return data;
        }
    }
}
