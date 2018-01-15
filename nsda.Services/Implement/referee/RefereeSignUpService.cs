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
                    var data = _dbContext.Select<t_referee_signup>(c => c.eventId == eventId && c.memberId == memberId).ToList();
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
                }
                else
                {
                    msg = "赛事有误";
                    return flag;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.Apply", ex);
            }
            return flag;
        }

        //裁判报名列表
        public List<RefereeSignUpResponse> List(RefereeSignUpQueryRequest request)
        {
            List<RefereeSignUpResponse> list = new List<RefereeSignUpResponse>();
            try
            {
                StringBuilder sb = new StringBuilder(@"select a.id Id,b.id EventId,b.name EventName,b.starteventdate StartEventDate,c.name CityName,
                                                        endeventdate EndEventDate,eventType EventType,eventStatus EventStatus,a.refereeSignUpStatus SignUpStatus
                                                        from t_referee_signup a
                                                        inner join t_event b on a.eventId=b.id
                                                        inner join t_city  c on b.cityId=c.id
                                                        where a.isdelete=0 and a.memberId=@MemberId");
                if (request.EventStartDate.HasValue)
                {
                    sb.Append(" and b.starteventdate>@EventStartDate ");
                }
                if (request.EventEndDate.HasValue)
                {
                    request.EventEndDate = request.EventEndDate.Value.AddDays(1).AddSeconds(-1);
                    sb.Append(" and b.endeventdate<@EventEndDate ");
                }
                if (request.CountryId.HasValue)
                {
                    sb.Append(" and b.countryId=@CountryId ");
                }
                if (request.ProvinceId.HasValue)
                {
                    sb.Append(" and b.provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue)
                {
                    sb.Append(" and b.cityId=@CityId ");
                }
                int totalCount = 0;
                list = _dbContext.Page<RefereeSignUpResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.List", ex);
            }
            return list;
        }
        //当前裁判比赛列表
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

        public List<RefereeSignUpListResponse> EventRefereeList(RefereeSignUpListQueryRequest request)
        {
            List<RefereeSignUpListResponse> list = new List<RefereeSignUpListResponse>();
            try
            {
                StringBuilder sb = new StringBuilder($@"select a.*,b.code MemberCode,b.completename MemberName from t_referee_signup a 
                                                      inner join t_member b on a.memberId=b.id
                                                      inner join t_event c on a.eventId=c.id
                                                      where a.isdelete=0 and b.isdelete=0 and c.isdelete=0 
                                                      and c.memberId=@MemberId and a.eventId=@EventId 
                                                     ");

                if (request.KeyValue.IsEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (code like @KeyValue or completename like @KeyValue)");
                }
                int totalCount = 0;
                list = _dbContext.Page<RefereeSignUpListResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("RefereeSignUpService.EventRefereeList", ex);
            }
            return list;
        }
    }
}
