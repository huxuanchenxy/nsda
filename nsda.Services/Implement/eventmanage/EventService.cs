﻿using Dapper;
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
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Model;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事管理
    /// </summary>
    public class EventService : IEventService
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
        //新增赛事
        public bool Insert(EventRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //参数信息校验
                if (request.Name.IsEmpty())
                {
                    msg = "赛事名不能为空";
                    return flag;
                }

                if (request.StartEventDate == DateTime.MinValue || request.StartEventDate == DateTime.MaxValue)
                {
                    msg = "赛事开始时间有误";
                    return flag;
                }

                if (request.EndEventDate == DateTime.MinValue || request.EndEventDate == DateTime.MaxValue)
                {
                    msg = "赛事结束时间有误";
                    return flag;
                }

                if (request.StartEventDate > request.EndEventDate)
                {
                    msg = "赛事结束时间不能早于开始时间";
                    return flag;
                }

                //报名截止日期 退费截止日期

                if (request.Maxnumber <= 0)
                {
                    msg = "报名队伍上限有误";
                    return flag;
                }

                if (request.EventGroup == null || request.EventGroup.Count == 0)
                {
                    msg = "赛事组别信息不能为空";
                    return flag;
                }

                foreach (var item in request.EventGroup)
                {
                    if (item.Name.IsEmpty())
                    {
                        msg = "赛事组别名称不能为空";
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

                    #region 赛事
                    int eventId = _dbContext.Insert(new t_event
                    {
                        address = request.Address,
                        cityId = request.CityId,
                        code = _dataRepository.EventRepo.RenderCode(),
                        countryId = request.CountryId,
                        endeventdate = request.EndEventDate,
                        endrefunddate = request.EndRefundDate,
                        endsigndate = request.EndSignDate,
                        remark = request.Remark,
                        eventStatus = EventStatusEm.待审核,
                        eventType = request.EventType,
                        filepath = request.Filepath,
                        isInter = request.IsInter,
                        maxnumber = request.Maxnumber,
                        starteventdate = request.StartEventDate,
                        memberId = request.MemberId,
                        name = request.Name,
                        provinceId = request.ProvinceId,
                        signfee = request.Signfee,
                        startsigndate = request.StartSignDate,
                        eventTypeName=request.EventTypeName
                    }).ToObjInt();
                    #endregion

                    #region 赛事组别
                    foreach (var item in request.EventGroup)
                    {
                        _dbContext.Insert(new t_eventgroup
                        {
                            eventId = eventId,
                            maxgrade = item.MaxGrade,
                            maxtimes = item.MaxTimes,
                            mingrade = item.MinGrade,
                            mintimes = item.MinTimes,
                            name = item.Name,
                            teamnumber = item.TeamNumber
                        });
                    }
                    #endregion

                    InsertEventRule(eventId);

                    _dbContext.CommitChanges();
                    flag = true;
                    _dbContext.Rollback();
                }
                catch (Exception ex)
                {
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("EventService.InsertTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.Insert", ex);
            }
            return flag;
        }
        private void InsertEventRule(int eventId)
        {
            #region 循环赛设置
            _dbContext.Insert(new t_eventteamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.获胜场数,
                viewindex = 1
            });
            _dbContext.Insert(new t_eventteamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.对方获胜场数,
                viewindex =2
            });
            _dbContext.Insert(new t_eventteamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.队伍选手总分,
                viewindex = 3
            });
            _dbContext.Insert(new t_eventteamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.对方队伍选手总分,
                viewindex = 4
            });
            _dbContext.Insert(new t_eventteamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.队伍选手排名总和,
                viewindex = 5
            });

            _dbContext.Insert(new t_eventplayerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.获胜场数,
                viewindex = 1
            });
            _dbContext.Insert(new t_eventplayerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.对方获胜场数,
                viewindex = 2
            });
            _dbContext.Insert(new t_eventplayerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.选手排名总和,
                viewindex = 3
            });
            _dbContext.Insert(new t_eventplayerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.对方选手排名,
                viewindex = 4
            });
            _dbContext.Insert(new t_eventplayerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.随机数值,
                viewindex = 5
            });

            _dbContext.Insert(new t_eventavoidrule
            {
                avoidRules = AvoidRulesEm.尽量规避同教练,
                eventId = eventId,
                viewindex = 1
            });
            _dbContext.Insert(new t_eventavoidrule
            {
                avoidRules = AvoidRulesEm.尽量规避同校,
                eventId = eventId,
                viewindex = 2
            });

            _dbContext.Insert(new t_eventrefereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.循环赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己已经裁判过的学生,
                viewindex = 1
            });
            _dbContext.Insert(new t_eventrefereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.循环赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己的学生,
                viewindex = 2
            });
            #endregion

            #region 淘汰赛设置
            _dbContext.Insert(new t_eventrefereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.淘汰赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己已经裁判过的学生,
                viewindex = 1
            });
            _dbContext.Insert(new t_eventrefereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.淘汰赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己的学生,
                viewindex = 2
            });
            #endregion
        }
        //编辑赛事
        public bool Edit(EventRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(request.Id);
                if (tevent != null && tevent.memberId == request.MemberId)
                {
                    //看具体能修改什么信息
                    tevent.updatetime = DateTime.Now;
                    tevent.eventStatus = tevent.eventStatus == EventStatusEm.拒绝 ? EventStatusEm.待审核 : tevent.eventStatus;
                    _dbContext.Update(tevent);
                }
                else
                {
                    msg = "未找到赛事信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.Edit", ex);
            }
            return flag;
        }
        //设定赛事级别
        public bool SettingLevel(int id, EventLevelEm eventLevel, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null)
                {
                    tevent.updatetime = DateTime.Now;
                    tevent.eventLevel = eventLevel;
                    _dbContext.Update(tevent);
                }
                else
                {
                    msg = "未找到赛事信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.SettingLevel", ex);
            }
            return flag;
        }
        // 审核赛事
        public bool Check(int id, bool isAppro, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null && tevent.eventStatus == EventStatusEm.待审核)
                {
                    tevent.updatetime = DateTime.Now;
                    tevent.eventStatus = isAppro ? EventStatusEm.报名中 : EventStatusEm.拒绝;
                    _dbContext.Update(tevent);
                }
                else
                {
                    msg = "未找到赛事信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.Check", ex);
            }
            return flag;
        }
        //赛事详情
        public EventResponse Detail(int id)
        {
            EventResponse response = null;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null)
                {
                    response = new EventResponse
                    {
                        Address = tevent.address,
                        Name = tevent.name,
                        CityId = tevent.cityId,
                        Code = tevent.code,
                        CountryId = tevent.countryId,
                        EndEventDate = tevent.endeventdate,
                        EndRefundDate = tevent.endrefunddate,
                        EndSignDate = tevent.endsigndate,
                        EventLevel = tevent.eventLevel,
                        Filepath = tevent.filepath,
                        EventStatus = tevent.eventStatus,
                        EventType = tevent.eventType,
                        Id = tevent.id,
                        IsInter = tevent.isInter,
                        Maxnumber = tevent.maxnumber,
                        ProvinceId = tevent.provinceId,
                        Remark = tevent.remark,
                        Signfee = tevent.signfee,
                        StartEventDate = tevent.starteventdate,
                        StartSignDate = tevent.startsigndate
                    };
                    var eventgroup = _dbContext.Select<t_eventgroup>(c => c.eventId == id).ToList();
                    if (eventgroup != null && eventgroup.Count > 0)
                    {
                        foreach (var item in eventgroup)
                        {
                            response.EventGroup.Add(new EventGroupResponse
                            {
                                EventId = item.eventId,
                                Id = item.id,
                                MaxGrade = item.maxgrade,
                                MaxTimes = item.maxtimes,
                                MinGrade = item.mingrade,
                                MinTimes = item.mintimes,
                                Name = item.name,
                                TeamNumber = item.teamnumber
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.Detail", ex);
            }
            return response;
        }
        //选手 裁判查询赛事列表
        public List<PlayerOrRefereeEventResponse> PlayerOrRefereeEvent(PlayerOrRefereeEventQueryRequest request)
        {
            List<PlayerOrRefereeEventResponse> list = new List<PlayerOrRefereeEventResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.CountryId.HasValue && request.CountryId > 0)
                {
                    join.Append(" and countryId=@CountryId ");
                }
                if (request.ProvinceId.HasValue && request.ProvinceId > 0)
                {
                    join.Append(" and provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue && request.CityId > 0)
                {
                    join.Append(" and cityId=@CityId ");
                }
                if (request.EventLevel.HasValue && request.EventLevel > 0)
                {
                    join.Append(" and eventLevel=@EventLevel ");
                }
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                if (request.StartDate.HasValue)
                {
                    DateTime dt = Convert.ToDateTime(request.StartDate);
                    join.Append($" and starteventdate >={Utility.FirstDayOfMonth(dt).ToShortDateString()} and starteventdate<={Utility.LastDayOfMonth(dt).ToShortDateString()}");
                }
                var sql=$@"select * from t_event where isdelete=0 and eventStatus in ({ParamsConfig._eventstatus}) {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<PlayerOrRefereeEventResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.PlayerOrRefereeEvent", ex);
            }
            return list;
        }
        //赛事管理员赛事列表
        public List<EventResponse> EventList(EventQueryRequest request)
        {
            List<EventResponse> list = new List<EventResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.EventType != null&&request.EventType>0)
                {
                    join.Append(" and eventtype=@EventType");
                }

                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                var sql=$@"select * from t_event where isdelete=0 and memberId=@MemberId {join.ToString()} order by createtime desc ";
                
                int totalCount = 0;
                list = _dbContext.Page<EventResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        //计算报名人数或者队伍
                        if (item.EventStatus != EventStatusEm.待审核 && item.EventStatus != EventStatusEm.拒绝)
                        {
                            item.SignUpCount = _dbContext.ExecuteScalar($"select distinct(groupnum) from t_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={item.Id}").ToObjInt();
                        }
                    }
                }
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.EventList", ex);
            }
            return list;
        }
        //管理平台赛事列表
        public List<EventResponse> ManageEventList(EventManageQueryRequest request)
        {
            List<EventResponse> list = new List<EventResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                if (request.EventStatus.HasValue && request.EventStatus > 0)
                {
                    join.Append(" and eventStatus >= @EventStatus");
                }
                if (request.EventType.HasValue && request.EventType > 0)
                {
                    join.Append(" and eventType >= @EventType");
                }
                if (request.EventStartDate.HasValue)
                {
                    join.Append(" and starteventdate >= @EventStartDate");
                }
                if (request.EventEndDate.HasValue)
                {
                    request.EventEndDate = request.EventEndDate.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and starteventdate <= @EventEndDate");
                }
                var sql = $@"select * from t_event where isdelete=0 {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<EventResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        //计算报名人数或者队伍
                        if (item.EventStatus != EventStatusEm.待审核 && item.EventStatus != EventStatusEm.拒绝)
                        {
                            item.SignUpCount = _dbContext.ExecuteScalar($"select distinct(groupnum) from t_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={item.Id}").ToObjInt();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.ManageEventList", ex);
            }
            return list;
        }
        //赛事查询条件
        public List<EventConditionResponse> EventCondition()
        {
            List<EventConditionResponse> list = new List<EventConditionResponse>();
            try
            {
                var sql = $@"select a.isInter IsInter,a.countryId CountryId,a.provinceId ProvinceId,a.cityId CityId,
                            b.name CountryName,c.name ProvinceName,d.name CityName
                            from t_event a
                            inner join t_country b on a.countryId=b.id
                            left join  t_province c on a.provinceId=b.id
                            left join  t_city     d on a.cityId=d.id
                            where a.isdelete=0 and a.eventStatus in ({ParamsConfig._eventnoquerystatus})";
                list = _dbContext.Query<EventConditionResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.EventCondition", ex);
            }
            return list;
        }
        //裁判注册时 可报名赛事
        public List<EventSelectResponse> RefereeRegisterEvent()
        {
            List<EventSelectResponse> list = new List<EventSelectResponse>();
            try
            {
                var sql = $"select id EventId,name EventName from t_event where isdelete=0 and eventStatus in ({ParamsConfig._eventstatus}) and starteventdate>={DateTime.Now.ToShortDateString()}";
                list = _dbContext.Query<EventSelectResponse>(sql).ToList();
                list.Insert(0,new EventSelectResponse {
                    EventId=-1,
                    EventName="暂时不确定"
                });
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.RefereeRegisterEvent", ex);
            }
            return list;
        }
        //赛事组别信息
        public List<EventGroupResponse> SelectEventGroup(int eventId, int memberId)
        {
            List<EventGroupResponse> list = new List<EventGroupResponse>();
            try
            {
                var sql = $"select * from t_eventgroup where isdelete=0 and eventId={eventId}";
                list = _dbContext.Query<EventGroupResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.SelectEventGroup", ex);
            }
            return list;
        }
        //修改赛事状态
        public bool EditEventStatus(int eventId, EventStatusEm eventStatus, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(eventId);
                if (tevent != null)
                {
                    tevent.updatetime = DateTime.Now;
                    tevent.eventStatus = eventStatus;
                    _dbContext.Update(tevent);
                }
                else
                {
                    msg = "未找到赛事信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.EditEventStatus", ex);
            }
            return flag;
        }

     }
}
