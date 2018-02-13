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
                var model = request.FirstOrDefault();
                if (model.EventId <= 0)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                if (request == null || request.Count == 0)
                {
                    msg = "请核对参数后再保存";
                    return flag;
                }
                //循环遍历判断参数合法性
                foreach (var item in request)
                {
                    if (item.Teamnumber <= 0)
                    {
                        msg = "晋级队伍数有误";
                        return flag;
                    }
                    string message = string.Empty;
                    foreach (var items in item.ListKnockout)
                    {
                        if (items.RefereeCount <= 0)
                        {
                            message = "裁判数量有误";
                            break;
                        }
                        if (items.Screenings <= 0)
                        {
                            message = "场次有误";
                            break;
                        }
                        if (items.Screenings != items.ListKnockoutDetail.Count)
                        {
                            message = "场次对应信息有误";
                            break;
                        }
                        string messages = string.Empty;
                        foreach (var itemss in items.ListKnockoutDetail)
                        {
                            if (itemss.StartTime == DateTime.MaxValue || itemss.StartTime == DateTime.MinValue)
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
                    _dbContext.Execute($"delete from t_event_knockout_settings where eventId={model.EventId}");
                    _dbContext.Execute($"delete from t_event_knockout where eventId={model.EventId}");
                    _dbContext.Execute($"delete from t_event_knockout_detail where eventId={model.EventId}");

                    foreach (var item in request)
                    {
                        //淘汰赛设置表
                        int settingsId = _dbContext.Insert(new t_event_knockout_settings
                        {
                            eventGroupId = item.EventGroupId,
                            eventId = item.EventId,
                            teamnumber=item.Teamnumber
                        }).ToObjInt();
                        //淘汰赛表
                        foreach (var items in item.ListKnockout)
                        {
                            int knockoutId = _dbContext.Insert(new t_event_knockout
                            {
                                knockoutStatus=items.KnockoutStatus,
                                knockoutType=items.KnockoutType,
                                refereeCount=items.RefereeCount,
                                eventGroupId = items.EventGroupId,
                                screenings=items.Screenings,
                                eventId = items.EventId,
                                settingsId = settingsId
                            }).ToObjInt();

                            foreach (var itemss in items.ListKnockoutDetail)
                            {
                                _dbContext.Insert(new t_event_knockout_detail
                                {
                                    knockoutId= knockoutId,
                                    eventGroupId = itemss.EventGroupId,
                                    eventId = itemss.EventId,
                                    screenings = itemss.Screenings,
                                    starttime = itemss.StartTime
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
                    LogUtils.LogError("EventknockoutSettingsService.InsertTran", ex);
                }
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
        public List<EventknockoutSettingsResponse> KnockoutSettings(int eventId)
        {
            List<EventknockoutSettingsResponse> list = new List<EventknockoutSettingsResponse>();
            try
            {
                var sql = $@"select * from t_event_knockout_settings where isdelete=0 and eventId={eventId}";
                var data = _dbContext.Query<EventknockoutSettingsResponse>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var itemdata = _dbContext.Select<t_event_knockout>(c => c.settingsId == item.Id).ToList();
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
                                    RefereeCount=items.refereeCount,
                                    Id = items.id                                   
                                };
                                var itemsdata = _dbContext.Select<t_event_knockout_detail>(c => c.knockoutId == items.id).ToList();
                                if (itemsdata != null && itemsdata.Count > 0)
                                {
                                    foreach (var itemss in itemsdata)
                                    {
                                        EventKnockoutDetailResponse responses = new EventKnockoutDetailResponse
                                        {
                                            KnockoutId =itemss.knockoutId,
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
                LogUtils.LogError("EventknockoutSettingsService.KnockoutSettings", ex);
            }
            return list;
        }
    }
}
