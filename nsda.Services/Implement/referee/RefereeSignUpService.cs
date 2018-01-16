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
                if (t_event != null)
                {
                    //进一步判断赛事状态
                    var data = _dbContext.Select<t_referee_signup>(c => c.eventId == eventId && c.memberId == memberId&&c.refereeSignUpStatus!=RefereeSignUpStatusEm.申请失败).ToList();
                    if (data != null && data.Count > 0)
                    {
                        msg = "您已提交过申请";
                        return flag;
                    }
                    _dbContext.Insert(new t_referee_signup {
                         eventId=eventId,
                         isTemp=false,
                         memberId=memberId,
                         refereeSignUpStatus=RefereeSignUpStatusEm.待审核
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
                LogUtils.LogError("RefereeSignUpService.Apply", ex);
            }
            return flag;
        }

        //裁判当前比赛列表
        public List<CurrentEventResponse> CurrentRefereeEvent(int memberId)
        {
            List<CurrentEventResponse> list = new List<CurrentEventResponse>();
            try
            {
                var sql = $@"select b.id EventId,b.name EventName,b.code EventCode from t_referee_signup a
                             inner join t_event b on a.eventId=b.id
                             where a.isdelete=0 and (b.starteventdate={DateTime.Now.ToShortDateString()} or b.endeventdate={DateTime.Now.ToShortDateString()}) and a.refereeSignUpStatus in ({ParamsConfig._refereestatus}) and  a.memberId={memberId}";
                return _dbContext.Query<CurrentEventResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.CurrentRefereeEvent", ex);
            }
            return list;
        }

        //裁判申请列表
        public List<EventRefereeSignUpListResponse> EventRefereeList(EventRefereeSignUpQueryRequest request)
        {
            List<EventRefereeSignUpListResponse> list = new List<EventRefereeSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue)");
                }
                var sql= $@"select a.*,b.code MemberCode,b.completename MemberName from t_referee_signup a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_event c on a.eventId=c.id
                            where a.isdelete=0 and b.isdelete=0 and c.isdelete=0 
                            and c.memberId=@MemberId and a.eventId=@EventId 
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

        //裁判审核
        public bool Check(int id,bool isAppro,int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_referee_signup referee_signup = _dbContext.Get<t_referee_signup>(id);
                if (referee_signup != null)
                {
                    t_event t_event = _dbContext.Get<t_event>(referee_signup.eventId);
                    referee_signup.updatetime = DateTime.Now;
                    referee_signup.refereeSignUpStatus = isAppro ? RefereeSignUpStatusEm.申请成功 : RefereeSignUpStatusEm.申请失败;
                    _dbContext.Update(referee_signup);
                    if (isAppro)
                    {
                        _dbContext.Insert(new t_eventsign {
                            eventId = referee_signup.eventId,
                            eventSignStatus = EventSignStatusEm.待签到,
                            eventSignType = EventSignTypeEm.裁判,
                            memberId = referee_signup.memberId,
                            signdate = t_event.starteventdate
                        });
                        if (t_event.starteventdate != t_event.endeventdate)
                        {
                            _dbContext.Insert(new t_eventsign
                            {
                                eventId = referee_signup.eventId,
                                eventSignStatus = EventSignStatusEm.待签到,
                                eventSignType = EventSignTypeEm.裁判,
                                memberId = referee_signup.memberId,
                                signdate = t_event.endeventdate
                            });
                        }
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
                LogUtils.LogError("RefereeSignUpService.Check", ex);
            }
            return flag;
        }

        //修改设置
        public bool Settings(int id, int statusOrGroup, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_referee_signup referee_signup = _dbContext.Get<t_referee_signup>(id);
                if (referee_signup != null)
                {
                    if (statusOrGroup == 0)
                    {
                        referee_signup.refereeSignUpStatus = RefereeSignUpStatusEm.停用;
                    }
                    else if (statusOrGroup == 1)
                    {
                        referee_signup.groupId = 0;
                        referee_signup.refereeSignUpStatus = RefereeSignUpStatusEm.启用;
                    }
                    else
                    {
                        referee_signup.groupId = statusOrGroup;
                        referee_signup.refereeSignUpStatus = RefereeSignUpStatusEm.启用;
                    }
                    referee_signup.updatetime = DateTime.Now;
                    _dbContext.Update(referee_signup);
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
                LogUtils.LogError("RefereeSignUpService.Settings", ex);
            }
            return flag;
        }

        public List<RefereeSignUpListResponse> RefereeSignUpList(RefereeSignUpQueryRequest request)
        {
            List<RefereeSignUpListResponse> list = new List<RefereeSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.CountryId.HasValue && request.CountryId > 0)
                {
                    join.Append(" and b.countryId=@CountryId ");
                }
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
                    join.Append($" and b.starteventdate >={Utility.FirstDayOfMonth(dt).ToShortDateString()} and b.starteventdate<={Utility.LastDayOfMonth(dt).ToShortDateString()}");
                }
                var sql= $@"select a.id Id,b.id EventId,b.name EventName,b.code EventCode,refereeSignUpStatus,
                                                      b.EventType,c.name CountryName,d.name ProvinceName,e.name CityName
                                                      from t_referee_signup a 
                                                      inner join t_event b on a.eventId=b.id
                                                      left  join t_country c on b.countryId=c.id
                                                      left  join t_province d on b.provinceId=d.id
                                                      left  join t_city e on e.id=b.cityId
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
    }
}
