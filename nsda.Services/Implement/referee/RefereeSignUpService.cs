using nsda.Model;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.member;
using nsda.Services.Contract.referee;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.referee
{
    /// <summary>
    /// 裁判报名管理
    /// </summary>
    public class RefereeSignUpService: IRefereeSignUpService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IMailService _mailService;
        public RefereeSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }
      
        #region 裁判
        //申请当裁判
        public bool Apply(int eventId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (eventId <= 0)
                {
                    msg = "请选择赛事";
                    return flag;
                }
                t_event t_event = _dbContext.Get<t_event>(eventId);
                if (t_event != null &&t_event.eventStatus!=EventStatusEm.审核中 && t_event.eventStatus!=EventStatusEm.拒绝&& t_event.endsigndate>DateTime.Now)
                {
                    var data = _dbContext.Select<t_event_referee_signup>(c => c.eventId == eventId && c.memberId == memberId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        msg = "您已提交过申请";
                        return flag;
                    }
                    _dbContext.Insert(new t_event_referee_signup {
                         eventId=eventId,
                         isTemp=false,
                         memberId=memberId,
                         eventGroupId=0,
                         refereeSignUpStatus=RefereeSignUpStatusEm.待审核,
                         isFlag=false
                    });
                    flag = true;
                }
                else
                {
                    msg = "赛事有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("RefereeSignUpService.Apply", ex);
            }
            return flag;
        }
        //裁判当前比赛列表
        public List<RefereeCurrentEventResponse> CurrentRefereeEvent(int memberId)
        {
            List<RefereeCurrentEventResponse> list = new List<RefereeCurrentEventResponse>();
            try
            {
                var sql = $@"select d.name ProvinceName,e.name CityName,b.starteventdate EventStartDate,b.id EventId,b.name EventName,b.code EventCode,b.eventType EventType,b.eventLevel EventLevel from t_event_referee_signup a
                             inner join t_event b on a.eventId=b.id
                             left join t_event_matchdate c on  a.eventId=c.eventId
                             left join t_sys_province d on b.provinceId=d.id
                             left join t_sys_city     e on b.cityId=e.id
                             where a.isdelete=0 and c.eventMatchDate='{DateTime.Now.ToShortDateString()}'
                             and a.refereeSignUpStatus in ({ParamsConfig._refereestatus}) and  a.memberId={memberId}
                           ";
                return _dbContext.Query<RefereeCurrentEventResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.CurrentRefereeEvent", ex);
            }
            return list;
        }
        //裁判报名列表
        public List<RefereeSignUpListResponse> RefereeSignUpList(RefereeSignUpQueryRequest request)
        {
            List<RefereeSignUpListResponse> list = new List<RefereeSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.ProvinceId.HasValue && request.ProvinceId > 0)
                {
                    join.Append(" and b.provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue && request.CityId > 0)
                {
                    join.Append(" and b.cityId=@CityId ");
                }
                if (request.StartDate.HasValue)
                {
                    DateTime dt = Convert.ToDateTime(request.StartDate);
                    join.Append($" and b.starteventdate >='{Utility.FirstDayOfMonth(dt).ToShortDateString()}' and b.starteventdate<='{Utility.LastDayOfMonth(dt).ToShortDateString()}'");
                }
                var sql= $@"select b.starteventdate StartEventDate,b.eventLevel EventLevel,a.id Id,b.id EventId,b.name EventName,b.code EventCode,refereeSignUpStatus,
                                                      b.EventType,c.name ProvinceName,d.name CityName
                                                      from t_event_referee_signup a 
                                                      inner join t_event b on a.eventId=b.id
                                                      left  join t_sys_province c on b.provinceId=c.id
                                                      left  join t_sys_city d on b.cityId=d.id
                                                      where a.isdelete=0 and b.isdelete=0 
                                                      and a.memberId=@MemberId {join.ToString()}  order by a.createtime desc
                                                     ";
                int totalCount = 0;
                list = _dbContext.Page<RefereeSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.RefereeSignUpList", ex);
            }
            return list;
        }
        #endregion

        #region 赛事管理员
        //裁判申请列表
        public List<EventRefereeSignUpListResponse> EventRefereeList(EventRefereeSignUpQueryRequest request)
        {
            List<EventRefereeSignUpListResponse> list = new List<EventRefereeSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue)");
                }
                if (request.RefereeSignUpStatus != null && request.RefereeSignUpStatus > 0)
                {
                    if ((int)request.RefereeSignUpStatus == 9)//临时裁判
                    {
                        join.Append($" and a.isTemp=1 ");
                    }
                    else if ((int)request.RefereeSignUpStatus == 8)//标记
                    {
                        join.Append($" and a.isFlag=1 ");
                    }
                    else
                    {
                        join.Append(" and a.refereeSignUpStatus=@RefereeSignUpStatus ");
                    }
                }
                var sql = $@"select a.*,b.code MemberCode,b.completename MemberName,c.account Email,b.contactmobile ContactMobile from t_event_referee_signup a 
                            inner join t_member_referee b on a.memberId=b.memberId
                            inner join t_member         c on a.memberId=c.id
                            inner join t_event d on a.eventId=d.id
                            where a.isdelete=0 and b.isdelete=0 and c.isdelete=0 and d.isdelete=0
                            and   d.memberId=@MemberId and a.eventId=@EventId 
                            {join.ToString()} order by a.createtime desc
                          ";
                int totalCount = 0;
                list = _dbContext.Page<EventRefereeSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.EventRefereeList", ex);
            }
            return list;
        }
        //裁判统计数据
        public RefereeDataResponse RefereeData(int eventId, int memberId)
        {
            RefereeDataResponse response = new RefereeDataResponse();
            try
            {
                var sql = $@"select a.* from t_event_referee_signup a 
                              inner join t_event b on a.eventId=b.id
                              where a.isdelete=0 and b.memberId={memberId} and a.eventId={eventId}
                           ";
                var list = _dbContext.Query<t_event_referee_signup>(sql).ToList();
                if (list != null && list.Count > 0)
                {
                    response.Total = list.Count;
                    response.Pending = list.Where(c => c.refereeSignUpStatus == RefereeSignUpStatusEm.待审核).Count();
                    response.NoPassed = list.Where(c => c.refereeSignUpStatus == RefereeSignUpStatusEm.未录取).Count();
                    response.TempReferee = list.Where(c => c.isTemp).Count();
                    response.Flag = list.Where(c => c.isFlag).Count();
                    response.Passed = list.Where(c => c.refereeSignUpStatus == RefereeSignUpStatusEm.已录取).Count();
                    response.Candidate = list.Where(c => c.refereeSignUpStatus == RefereeSignUpStatusEm.候选名单).Count();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.RefereeData", ex);
            }
            return response;
        }
        //标记
        public bool Flag(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_referee_signup referee_signup = _dbContext.Get<t_event_referee_signup>(id);
                if (referee_signup != null)
                {
                    if (referee_signup.isFlag)
                    {
                        referee_signup.isFlag = false;
                    }
                    else
                    {
                        referee_signup.isFlag = true;
                    }
                    referee_signup.updatetime = DateTime.Now;
                    _dbContext.Update(referee_signup);
                    flag = true;
                }
                else
                {
                    msg = "未找到裁判信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("RefereeSignUpService.Flag", ex);
            }
            return flag;
        }
        //裁判审核
        public bool Check(int id, CheckRefereeEnum checkReferee, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_referee_signup referee_signup = _dbContext.Get<t_event_referee_signup>(id);
                if (referee_signup != null)
                {
                    referee_signup.updatetime = DateTime.Now;
                    referee_signup.refereeSignUpStatus = checkReferee == CheckRefereeEnum.通过 ? RefereeSignUpStatusEm.已录取 : (checkReferee == CheckRefereeEnum.拒绝 ? RefereeSignUpStatusEm.未录取 : RefereeSignUpStatusEm.候选名单);
                    if (checkReferee == CheckRefereeEnum.通过)
                    {
                        t_event t_event = _dbContext.Get<t_event>(referee_signup.eventId);
                        if (t_event.eventStatus == EventStatusEm.比赛中)
                        {
                            var t_event_sign = _dbContext.Select<t_event_sign>(c => c.eventId == referee_signup.eventId && c.memberId == referee_signup.memberId).ToList();
                            if (t_event_sign == null && t_event_sign.Count > 0)
                            {
                                var eventdate = _dbContext.Select<t_event_matchdate>(c => c.eventId == t_event.id);
                                foreach (var itemdate in eventdate)
                                {
                                    _dbContext.Insert(new t_event_sign
                                    {
                                        eventGroupId = 0,
                                        eventId = referee_signup.eventId,
                                        eventSignType = EventSignTypeEm.裁判,
                                        eventSignStatus = EventSignStatusEm.待签到,
                                        signdate = itemdate.eventMatchDate,
                                        memberId = referee_signup.memberId,
                                        isStop = false
                                    });
                                }
                            }
                        }
                        _dbContext.Update(referee_signup);
                        if (checkReferee == CheckRefereeEnum.通过)
                        {
                            _dbContext.Insert(new t_sys_mail
                            {
                                title = $"You meet the qualification to be a judge.",
                                content = $"Congratulations, your application to be a judge of {t_event.name} has been accepted.",
                                isRead = false,
                                mailType = MailTypeEm.赛事报名邀请,
                                memberId = referee_signup.memberId,
                                sendMemberId = memberId
                            });

                        }
                    }
                    else
                    {
                        _dbContext.Update(referee_signup);
                    }
                    flag = true;
                }
                else
                {
                    msg = "报名信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("RefereeSignUpService.Check", ex);
            }
            return flag;
        }
        #endregion 
    }
}
