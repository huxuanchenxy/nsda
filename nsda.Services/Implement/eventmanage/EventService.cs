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

                if (request.EndRefundDate > request.EndEventDate)
                {
                    msg = "退费截止日期不能超过赛事结束日期";
                }

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

                if (request.EventType <= 0 || request.EventTypeName <= 0)
                {
                    msg = "请选择赛事类型";
                    return flag;
                }

                if (request.Address.IsEmpty())
                {
                    msg = "赛事地址不能为空";
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
                        englishname=request.EnglishName,
                        address = request.Address,
                        cityId = request.CityId,
                        code = _dataRepository.EventRepo.RenderCode(),
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
                        eventTypeName = request.EventTypeName
                    }).ToObjInt();
                    #endregion

                    #region 赛事组别
                    foreach (var item in request.EventGroup)
                    {
                        _dbContext.Insert(new t_event_group
                        {
                            eventId = eventId,
                            maxgrade = item.MaxGrade,
                            maxtimes = item.MaxTimes,
                            mingrade = item.MinGrade,
                            mintimes = item.MinTimes,
                            name = item.Name,
                            teamnumber =request.EventType==EventTypeEm.辩论?2:1
                        });
                    }
                    #endregion

                    InsertEventRule(eventId);

                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    msg = "服务异常";
                    _dbContext.Rollback();
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
            _dbContext.Insert(new t_event_teamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.队伍获胜的场数,
                viewindex = 1
            });
            _dbContext.Insert(new t_event_teamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.队伍中两位辩手的个人总分,
                viewindex = 2
            });
            _dbContext.Insert(new t_event_teamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.所遇到的对手获胜的场数总和,
                viewindex = 3
            });
            _dbContext.Insert(new t_event_teamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.对手的个人总分,
                viewindex = 4
            });
            _dbContext.Insert(new t_event_teamscoringrule
            {
                eventId = eventId,
                teamScoringRules = TeamScoringRulesEm.队伍中两位辩手的个人排名总和,
                viewindex = 5
            });

            _dbContext.Insert(new t_event_playerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.选手个人总分,
                viewindex = 1
            });
            _dbContext.Insert(new t_event_playerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.个人排名总和,
                viewindex = 2
            });
            _dbContext.Insert(new t_event_playerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.队伍获胜场数总和,
                viewindex = 3
            });
            _dbContext.Insert(new t_event_playerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.所遇到的对手获胜的场数总和,
                viewindex = 4
            });
            _dbContext.Insert(new t_event_playerscoringrule
            {
                eventId = eventId,
                scoringRules = ScoringRulesEm.对手的个人总分,
                viewindex = 5
            });

            _dbContext.Insert(new t_event_cycling_avoidrule
            {
                avoidRules = AvoidRulesEm.尽量规避同教练,
                eventId = eventId,
                viewindex = 1
            });
            _dbContext.Insert(new t_event_cycling_avoidrule
            {
                avoidRules = AvoidRulesEm.尽量规避同校,
                eventId = eventId,
                viewindex = 2
            });

            _dbContext.Insert(new t_event_refereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.循环赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己已经裁判过的学生,
                viewindex = 1
            });
            _dbContext.Insert(new t_event_refereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.循环赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己的学生,
                viewindex = 2
            });
            #endregion

            #region 淘汰赛设置
            _dbContext.Insert(new t_event_refereeavoidrule
            {
                eventId = eventId,
                objEventType = ObjEventTypeEm.淘汰赛,
                refereeAvoidRules = RefereeAvoidRulesEm.尽量规避自己已经裁判过的学生,
                viewindex = 1
            });
            _dbContext.Insert(new t_event_refereeavoidrule
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

                if (request.EndRefundDate > request.EndEventDate)
                {
                    msg = "退费截止日期不能超过赛事结束日期";
                }

                if (request.Maxnumber <= 0)
                {
                    msg = "报名队伍上限有误";
                    return flag;
                }

                if (request.EventType <= 0 || request.EventTypeName <= 0)
                {
                    msg = "请选择赛事类型";
                    return flag;
                }

                if (request.Address.IsEmpty())
                {
                    msg = "赛事地址不能为空";
                    return flag;
                }
                t_event tevent = _dbContext.Get<t_event>(request.Id);
                if (tevent != null && tevent.memberId == request.MemberId)
                {
                    tevent.address = request.Address;
                    tevent.cityId = request.CityId;
                    tevent.endeventdate = request.EndEventDate;
                    tevent.endrefunddate = request.EndRefundDate;
                    tevent.endsigndate = request.EndSignDate;
                    tevent.remark = request.Remark;
                    tevent.eventStatus = tevent.eventStatus == EventStatusEm.拒绝 ? EventStatusEm.待审核 : tevent.eventStatus;
                    tevent.eventType = request.EventType;
                    tevent.filepath = request.Filepath;
                    tevent.isInter = request.IsInter;
                    tevent.maxnumber = request.Maxnumber;
                    tevent.starteventdate = request.StartEventDate;
                    tevent.memberId = request.MemberId;
                    tevent.name = request.Name;
                    tevent.provinceId = request.ProvinceId;
                    tevent.signfee = request.Signfee;
                    tevent.eventTypeName = request.EventTypeName;
                    tevent.updatetime = DateTime.Now;
                    _dbContext.Update(tevent);
                    flag = true;
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
        //修改组别信息
        public bool EditGroup(EventGroupRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "赛事组别名称不能为空";
                    return flag;
                }
                t_event_group eventgroup = _dbContext.Get<t_event_group>(request.Id);
                if (eventgroup != null)
                {
                    eventgroup.name = request.Name;
                    eventgroup.maxgrade = request.MaxGrade;
                    eventgroup.mingrade = request.MinGrade;
                    eventgroup.mintimes = request.MinTimes;
                    eventgroup.maxtimes = request.MaxTimes;
                    eventgroup.updatetime = DateTime.Now;
                    _dbContext.Update(eventgroup);
                    flag = true;
                }
                else
                {
                    msg = "未找到赛事组别信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.EditGroup", ex);
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
                    flag = true;
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
        public bool Check(int id, bool isAgree, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null && tevent.eventStatus == EventStatusEm.待审核)
                {
                    tevent.updatetime = DateTime.Now;
                    tevent.eventStatus = isAgree ? EventStatusEm.报名中 : EventStatusEm.拒绝;
                    _dbContext.Update(tevent);
                    flag = true;
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
                        StartEventDate = tevent.starteventdate
                    };
                    var eventgroup = _dbContext.Select<t_event_group>(c => c.eventId == id).ToList();
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
                if (request.ProvinceId.HasValue && request.ProvinceId > 0)
                {
                    join.Append(" and a.provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue && request.CityId > 0)
                {
                    join.Append(" and a.cityId=@CityId ");
                }
                if (request.EventLevel.HasValue && request.EventLevel > 0)
                {
                    join.Append(" and a.eventLevel=@EventLevel ");
                }
                if (request.StartDate.HasValue)
                {
                    DateTime dt = Convert.ToDateTime(request.StartDate);
                    join.Append($" and a.starteventdate >='{Utility.FirstDayOfMonth(dt).ToShortDateString()}' and a.starteventdate<='{Utility.LastDayOfMonth(dt).ToShortDateString()}'");
                }
                var sql = $@"select a.id EventId,a.code EventCode, a.name EventName,a.eventType EventType, a.eventLevel EventLevel,
                             a.signfee SignFee, a.eventStatus EventStatus,a.starteventdate StartEventDate, a.endsigndate EndSignDate,
                             b.name ProvinceName,c.name CityName
                             from t_event a 
                             left join t_sys_province b on a.provinceId=b.id
                             left join t_sys_city     c on a.cityId=c.id
                             where a.isdelete=0 and a.eventStatus in ({ParamsConfig._eventstatus}) {join.ToString()} order by a.createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<PlayerOrRefereeEventResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null&&list.Count>0)
                {
                    foreach (var item in list)
                    {
                        if (item.EventStatus == EventStatusEm.停止报名)
                        {
                            item.IsVisiable = false;
                            break;
                        }
                        if (DateTime.Now > item.EndSignDate)
                        {
                            item.IsVisiable = false;
                            break;
                        }
                        item.IsVisiable = true;
                    }
                }
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
                if (request.EventType != null && request.EventType > 0)
                {
                    join.Append(" and eventtype=@EventType");
                }

                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                var sql = $@"select * from t_event where isdelete=0 and memberId=@MemberId {join.ToString()} order by createtime desc ";

                int totalCount = 0;
                list = _dbContext.Page<EventResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        //计算报名人数或者队伍
                        if (item.EventStatus != EventStatusEm.待审核 && item.EventStatus != EventStatusEm.拒绝)
                        {
                            item.SignUpCount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={item.Id}").ToObjInt();
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
        public List<EventResponse> AdminEventList(EventAdminQueryRequest request)
        {
            List<EventResponse> list = new List<EventResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
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
                            item.SignUpCount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={item.Id}").ToObjInt();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.AdminEventList", ex);
            }
            return list;
        }
        //赛事查询条件
        public List<EventConditionResponse> EventCondition()
        {
            List<EventConditionResponse> list = new List<EventConditionResponse>();
            try
            {
                var sql = $@"select distinct a.isInter IsInter,a.provinceId ProvinceId,a.cityId CityId,
                            b.name ProvinceName,c.name CityName
                            from t_event a
                            left join  t_sys_province b on a.provinceId=b.id
                            left join  t_sys_city     c on a.cityId=c.id
                            where a.isdelete=0 and a.eventStatus not in ({ParamsConfig._eventnoquerystatus})";
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
                var sql = $"select id EventId,name EventName from t_event where isdelete=0 and eventStatus ={EventStatusEm.报名中} and starteventdate>={DateTime.Now.ToShortDateString()}";
                list = _dbContext.Query<EventSelectResponse>(sql).ToList();
                list.Insert(0, new EventSelectResponse
                {
                    EventId = 0,
                    EventName = "暂时不确定"
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
                var sql = $"select * from t_event_group where isdelete=0 and eventId={eventId}";
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
                    flag = true;
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
        // 赛事组别列表
        public List<EventGroupResponse> ListEventGroup(int eventId, int memberId)
        {
            List<EventGroupResponse> list = new List<EventGroupResponse>();
            try
            {
                var sql = $"select * from t_event_group where isdelete=0 and eventId={eventId}";
                list = _dbContext.Query<EventGroupResponse>(sql).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        item.SignUpCount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where eventId={item.EventId} and eventGroupId={item.Id} ").ToObjInt();
                        item.SignUpSuccessCount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where eventId={item.EventId} and eventGroupId={item.Id} and signUpStatus={SignUpStatusEm.报名成功}").ToObjInt();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.ListEventGroup", ex);
            }
            return list;
        }
        //赛事组别详情
        public EventGroupResponse EventGroupDetail(int eventGroupId)
        {
            EventGroupResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_event_group>(eventGroupId);
                if (detail != null)
                {
                    response = new EventGroupResponse
                    {
                        EventId = detail.eventId,
                        Id = detail.id,
                        MaxGrade = detail.maxgrade,
                        MinGrade = detail.mingrade,
                        MaxTimes = detail.maxtimes,
                        MinTimes = detail.mintimes,
                        Name = detail.name,
                        TeamNumber = detail.teamnumber
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.EventGroupDetail", ex);
            }
            return response;
        }
    }
}
