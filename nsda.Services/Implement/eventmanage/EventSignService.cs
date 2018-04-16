using nsda.Model.enums;
using nsda.Models;
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
using nsda.Model.dto.response;
using Dapper;
using nsda.Model.dto.request;
using nsda.Repository.Contract.eventmanage;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事签到管理
    /// </summary>
    public class EventSignService : IEventSignService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IRefereeSignRepo _refereeSignRepo;
        IRoomRepo _roomData;
        public EventSignService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService, IRefereeSignRepo refereeSignRepo, IRoomRepo roomData)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _refereeSignRepo = refereeSignRepo;
            _roomData = roomData;
        }
        //裁判 选手签到
        public bool Sign(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_sign eventsign = _dbContext.Get<t_event_sign>(id);
                if (eventsign == null || eventsign.memberId != memberId || eventsign.isStop)
                {
                    msg = "签到信息有误";
                }
                else
                {
                    eventsign.signtime = DateTime.Now;
                    eventsign.eventSignStatus = EventSignStatusEm.已签到;
                    _dbContext.Update(eventsign);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.Sign", ex);
            }
            return flag;
        }
        //赛事管理员 批量签到
        public bool BatchSign(List<int> memberId, int eventId, EventSignTypeEm eventSignType, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sql = "update t_event_sign set eventSignStatus=@EventSignStatus,signtime=@SignTime where eventSignType=@EventSignType and memberId in @MemberId and eventId=@EventId and signDate=@SignDate ";
                var dy = new DynamicParameters();
                dy.Add("MemberId", memberId.ToArray());
                dy.Add("EventId", eventId);
                dy.Add("EventSignType", eventSignType);
                dy.Add("EventSignStatus", EventSignStatusEm.已签到);
                dy.Add("SignTime", DateTime.Now);
                dy.Add("SignDate", DateTime.Now.ToString("yyyy-MM-dd"));
                _dbContext.Execute(sql, dy);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.BatchSign", ex);
            }
            return flag;
        }
        //选手批量签到
        public bool PlayerBatchSign(List<string> groupNum, int eventId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sql = @"update t_event_sign set eventSignStatus=@EventSignStatus,signtime=@SignTime where  eventSignType=@EventSignType and eventId=@EventId and signDate=@SignDate and memberId in (
                            select memberId from t_event_player_signup where eventId=@EventId and groupNum in @GroupNum)";
                var dy = new DynamicParameters();
                dy.Add("GroupNum", groupNum.ToArray());
                dy.Add("EventId", eventId);
                dy.Add("EventSignType", EventSignTypeEm.选手);
                dy.Add("EventSignStatus", EventSignStatusEm.已签到);
                dy.Add("SignTime", DateTime.Now);
                dy.Add("SignDate", DateTime.Now.ToString("yyyy-MM-dd"));
                _dbContext.Execute(sql, dy);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.PlayerBatchSign", ex);
            }
            return flag;
        }
        //停赛
        public bool Stop(string groupNum, int eventId, bool isStop, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sql = $@"update t_event_sign set isStop=@IsStop,updatetime=@UpdateTime  where  eventSignType=@EventSignType and eventId=@EventId  and memberId in (
                            select memberId from t_event_player_signup where eventId=@EventId and groupNum=@GroupNum)";
                var dy = new DynamicParameters();
                dy.Add("GroupNum", groupNum);
                dy.Add("IsStop", isStop);
                dy.Add("EventId", eventId);
                dy.Add("EventSignType", EventSignTypeEm.选手);
                dy.Add("UpdateTime", DateTime.Now);
                _dbContext.Execute(sql, dy);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.Stop", ex);
            }
            return flag;
        }
        //选手签到列表
        public List<PlayerSignResponse> PlayerSignList(PlayerSignQueryRequest request)
        {
            List<PlayerSignResponse> list = new List<PlayerSignResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or d.groupnum like @KeyValue)");
                }
                var sql = $@" select a.isStop IsStop,b.gender Gender,b.grade Grade,b.contactmobile ContactMobile,d.groupnum GroupNum,a.memberId MemberId,b.completename MemberName,b.code MemberCode,GROUP_CONCAT(a.signdate order by a.Id) Signdates,GROUP_CONCAT(a.eventSignStatus order by a.Id) EventSignStatuss
                            from t_event_sign a 
                            inner join t_member_player b on a.memberId=b.memberId
                            inner join t_event c on a.eventId=c.id
                            inner join t_event_player_signup d on d.eventId=a.eventId and d.eventGroupId=a.eventGroupId and a.memberId=d.memberId
                            where a.isdelete=0 and b.isdelete=0 and c.isdelete=0
                            and a.eventId=@EventId and a.eventGroupId=@EventGroupId and c.memberId=@MemberId and eventSignType={(int)EventSignTypeEm.选手}
                            {join.ToString()} group by a.memberId order by d.GroupNum desc
                        ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerSignResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var data = _dbContext.Query<dynamic>($"select b.chinessname,c.name from t_player_edu  a inner join t_sys_school b on a.schoolId=b.id inner join t_sys_city c on c.id=b.cityId  where a.memberid={item.MemberId} and a.isdelete=0 order by a.startdate desc limit 1").FirstOrDefault();
                        if (data != null)
                        {
                            item.SchoolName = data.chinessname;
                            item.CityName = data.name;
                        }
                        List<string> ltSignDate = item.SignDates.Split(',').ToList();
                        List<string> ltSignStatus = item.EventSignStatuss.Split(',').ToList();
                        for (int i = 0; i < ltSignDate.Count; i++)
                        {
                            var dt = ltSignDate[i].ToDateTime();
                            var model = new PlayerSignSplitResponse
                            {
                                SignDate = ltSignDate[i].ToDateTime().ToString("MM/dd"),
                                EventSignStatus = (EventSignStatusEm)ltSignStatus[i].ToInt32()
                            };
                            if (dt < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                model.SignType = 1;
                            }
                            else if (dt.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                model.SignType = 2;
                            }
                            else
                            {
                                model.SignType = 3;
                            }
                            item.List.Add(model);
                        }
                    }
                }
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.PlayerSignList", ex);
            }
            return list;
        }
        //裁判签到列表
        public List<RefereeSignResponse> RefereeSignList(RefereeSignQueryRequest request)
        {
            List<RefereeSignResponse> list = new List<RefereeSignResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue)");
                }
                if (request.EventGroupId != null && request.EventGroupId > 0)
                {
                    join.Append(" and d.eventGroupId=@EventGroupId ");
                }
                if (request.RefereeStatus != null && request.RefereeStatus > 0)
                {
                    if (request.RefereeStatus != 1)
                    {
                        join.Append(" and a.RefereeStatus = @RefereeStatus ");
                    }
                }
                var sql = $@" select a.eventGroupId,a.RefereeStatus,e.name EventGroupName,a.memberId MemberId,b.completename MemberName,b.code MemberCode,b.contactmobile ContactMobile,GROUP_CONCAT(a.signdate order by a.Id) Signdates,GROUP_CONCAT(a.eventSignStatus order by a.Id) EventSignStatuss
                            from t_event_sign a 
                            inner join t_member_referee b on a.memberId=b.memberId
                            inner join t_event c on a.eventId=c.id
                            inner join t_event_referee_signup d on d.eventId=a.eventId and d.memberId=a.memberId
                            LEFT join t_event_group e on e.id=d.eventGroupId
                            where a.isdelete=0 and b.isdelete=0 and c.isdelete=0
                            and a.eventId=@EventId and c.memberId=@MemberId and a.eventSignType={(int)EventSignTypeEm.裁判}
                            {join.ToString()} group by a.memberId order by a.createtime desc
                        ";
                int totalCount = 0;
                list = _dbContext.Page<RefereeSignResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        List<string> ltSignDate = item.SignDates.Split(',').ToList();
                        List<string> ltSignStatus = item.EventSignStatuss.Split(',').ToList();
                        for (int i = 0; i < ltSignDate.Count; i++)
                        {
                            var dt = ltSignDate[i].ToDateTime();
                            var model = new RefereeSignSplitResponse
                            {
                                SignDate = dt.ToString("MM/dd"),
                                EventSignStatus = (EventSignStatusEm)ltSignStatus[i].ToInt32()
                            };
                            if (dt < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                model.SignType = 1;
                            }
                            else if (dt.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                model.SignType = 2;
                            }
                            else
                            {
                                model.SignType = 3;
                            }
                            item.List.Add(model);
                        }
                    }
                }
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.RefereeSignList", ex);
            }
            return list;
        }
        //选手/裁判获取签到信息
        public SignResponse GetSign(int eventId, int memberId, EventSignTypeEm eventSignType)
        {
            SignResponse response = null;
            try
            {
                var sql = $@"select a.id,b.code EventCode,b.name EventName,a.signdate,a.eventSignStatus from t_event_sign a
                            inner join t_event b on a.eventId=b.id
                            where a.isdelete=0 and a.eventId=@EventId and a.memberId=@MemberId 
                            and a.signdate=@SignDate and a.isStop=@IsStop and a.eventSignType={(int)eventSignType}";
                var dy = new DynamicParameters();
                dy.Add("EventId", eventId);
                dy.Add("IsStop", false);
                dy.Add("MemberId", memberId);
                dy.Add("SignDate", DateTime.Now.ToShortDateString());
                response = _dbContext.QueryFirstOrDefault<SignResponse>(sql, dy);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.GetSign", ex);
            }
            return response;
        }
        //裁判批量签到或设置组别
        public bool BatchReferee(List<int> memberId, int eventId, int status, out string msg,int RefereeStatus)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (status == -1)//签到
                {
                    var sql = "update t_event_sign set eventSignStatus=@EventSignStatus,signtime=@SignTime where eventSignType=@EventSignType and memberId in @MemberId and eventId=@EventId and signDate=@SignDate ";
                    var dy = new DynamicParameters();
                    dy.Add("MemberId", memberId.ToArray());
                    dy.Add("EventId", eventId);
                    dy.Add("EventSignType", EventSignTypeEm.裁判);
                    dy.Add("EventSignStatus", EventSignStatusEm.已签到);
                    dy.Add("SignTime", DateTime.Now);
                    dy.Add("SignDate", DateTime.Now.ToString("yyyy-MM-dd"));
                    _dbContext.Execute(sql, dy);
                }
                else//批处理
                {
                    //var sql = "update t_event_referee_signup set  eventGroupId=@EventGroupId,updateTime=@UpdateTime where  eventId=@EventId and  memberId in @MemberId ";
                    var sql = "update t_event_sign set RefereeStatus = @RefereeStatus,  eventGroupId=@EventGroupId,updateTime=@UpdateTime where  eventId=@EventId and  memberId in @MemberId ";
                    var dy = new DynamicParameters();
                    dy.Add("MemberId", memberId.ToArray());
                    dy.Add("EventId", eventId);
                    dy.Add("EventGroupId", status);
                    dy.Add("UpdateTime", DateTime.Now);
                    dy.Add("RefereeStatus", RefereeStatus);
                    _dbContext.Execute(sql, dy);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.BatchReferee", ex);
            }
            return flag;
        }
        // 裁判数据统计
        public RefereeSignDataResponse RefereeSignData(int eventId, string manMemberId)
        {
            RefereeSignDataResponse response = new RefereeSignDataResponse();
            try
            {
                //数据统计
                List<t_event_sign> list = _refereeSignRepo.RefereeSignData(eventId, manMemberId);
                List<EventRoomResponse> list1 = _roomData.GetList(new EventRoomQueryRequest() { EventId = eventId });
                if (list1 != null && list1.Count > 0)
                {
                    response.LeastCount = list1.Where(c => c.RoomStatus == RoomStatusEm.闲置).Count();
                }
                if (list != null && list.Count > 0)
                {
                    response.SignCount = list.Where(c => c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.eventSignStatus == EventSignStatusEm.已签到).Count();
                    response.UsedCount = list.Where(c => c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.refereeStatus == RefereeStatusEm.使用中).Count();
                    response.UnusedCount = list.Where(c => c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.refereeStatus == RefereeStatusEm.闲置).Count();
                    response.StopCount = list.Where(c => c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.isStop).Count();

                }
                var eventGroup = _dbContext.Select<t_event_group>(c => c.eventId == eventId);
                foreach (var item in eventGroup)
                {
                    response.RefereeSignGroup.Add(new RefereeSignGroupResponse
                    {
                        EventGroupId = item.id,
                        LeastCount = list1.Where(c => c.EventGroupId == item.id).Count(),
                        SignCount = list.Where(c => c.eventSignStatus == EventSignStatusEm.已签到 && c.eventGroupId == item.id).Count()
                    });
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.RefereeSignData", ex);
            }
            return response;
        }
        //查询当天组别签到人数
        public int SignUpCount(int eventId, int eventGroupId)
        {
            int signUpCount = 0;
            try
            {
                signUpCount = _dbContext.ExecuteScalar($"select count(*) from t_event_sign where eventId={eventId} and eventGroupId={eventGroupId} and eventSignType={(int)EventSignTypeEm.选手} and signdate={DateTime.Now.ToString("yyyy-MM-dd")}").ToObjInt();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.SignUpCount", ex);
            }
            return signUpCount;
        }

        /// <summary>
        /// 裁判签到改变状态
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="manMemberId"></param>
        /// <param name="refereeStatus"></param>
        public void RefereeSignSetting(int memberId, int manMemberId, int statusSet,int eventGroupId,int eventid)
        {
            RefereeStatusEm refereeStatus;
            if (statusSet == -1)//页面传来停用操作否则就是闲置
            {
                refereeStatus = RefereeStatusEm.停用;
                eventGroupId = 0;
            }
            else
            {
                refereeStatus = RefereeStatusEm.闲置;

            }
            _refereeSignRepo.RefereeSignSetting(memberId, manMemberId,(int)refereeStatus,eventGroupId, eventid);
        }

    }
}
