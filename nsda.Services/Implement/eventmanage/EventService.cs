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
                //参数信息校验
                try
                {
                    _dbContext.BeginTransaction();
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
                    foreach (var item in request.EventGroup)
                    {
                        _dbContext.Insert(new t_eventgroup
                        {
                            eventId = eventId,
                            maxgrade = item.MaxGrade,
                            maxPoints = item.MaxPoints,
                            maxtimes = item.MaxTimes,
                            mingrade = item.MinGrade,
                            minPoints = item.MinPoints,
                            mintimes = item.MinTimes,
                            name = item.Name,
                            teamnumber = item.TeamNumber
                        });
                    }
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
        //停止报名
        public bool IsOpen(int id, bool isOpen, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null && (tevent.eventStatus == EventStatusEm.报名中 || tevent.eventStatus == EventStatusEm.停止报名))
                {
                    tevent.updatetime = DateTime.Now;
                    tevent.eventStatus = isOpen ? EventStatusEm.报名中 : EventStatusEm.停止报名;
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
                LogUtils.LogError("EventService.IsOpen", ex);
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
                                MaxPoints = item.maxPoints,
                                MaxTimes = item.maxtimes,
                                MinGrade = item.mingrade,
                                MinPoints = item.minPoints,
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
                StringBuilder sb = new StringBuilder($@"select * from t_event where isdelete=0 and eventStatus in ({ParamsConfig._eventstatus}) ");
                if (request.CountryId.HasValue&&request.CountryId>0)
                {
                    sb.Append(" and countryId=@CountryId ");
                }
                if (request.ProvinceId.HasValue && request.ProvinceId > 0)
                {
                    sb.Append(" and provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue && request.CityId > 0)
                {
                    sb.Append(" and cityId=@CityId ");
                }
                if (request.EventLevel.HasValue && request.EventLevel > 0)
                {
                    sb.Append(" and eventLevel=@EventLevel ");
                }
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                if (request.StartDate.HasValue)
                {
                    DateTime dt = Convert.ToDateTime(request.StartDate);
                    sb.Append($" and starteventdate >={Utility.FirstDayOfMonth(dt).ToShortDateString()} and starteventdate<={Utility.LastDayOfMonth(dt).ToShortDateString()}");
                }
                int totalCount = 0;
                list = _dbContext.Page<PlayerOrRefereeEventResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
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
                StringBuilder sb = new StringBuilder($@"select * from t_event where isdelete=0 and memberId=@MemberId ");
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                int totalCount = 0;
                list = _dbContext.Page<EventResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
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
                StringBuilder sb = new StringBuilder($@"select * from t_event where isdelete=0 ");
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                if (request.EventStatus.HasValue && request.EventStatus > 0)
                {
                    sb.Append(" and eventStatus >= @EventStatus");
                }
                if (request.EventType.HasValue && request.EventType > 0)
                {
                    sb.Append(" and eventType >= @EventType");
                }
                if (request.EventStartDate.HasValue)
                {
                    sb.Append(" and starteventdate >= @EventStartDate");
                }
                if (request.EventEndDate.HasValue)
                {
                    request.EventEndDate = request.EventEndDate.Value.AddDays(1).AddSeconds(-1);
                    sb.Append("  and starteventdate <= @EventEndDate");
                }
                int totalCount = 0;
                list = _dbContext.Page<EventResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
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
    }
}
