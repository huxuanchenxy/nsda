using nsda.Repository;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.enums;
using nsda.Services.Contract.member;
using nsda.Services.Contract.admin;
using nsda.Model;
using System.Text;

namespace nsda.Services.member
{
    /// <summary>
    /// 选手教练绑定管理
    /// </summary>
    public class PlayerCoachService: IPlayerCoachService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IMailService _mailService;
        public PlayerCoachService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }
        //绑定教练/学生
        public bool Insert(PlayerCoachRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ObjMemberId <= 0)
                {
                    if (request.IsCoach)
                    {
                        msg = "教练不能为空";
                        return flag;
                    }
                    else
                    {
                        msg = "学生不能为空";
                        return flag;
                    }
                }

                if (request.StartDate.IsEmpty())
                {
                    msg = "开始时间不能为空";
                    return flag;
                }

                var playerCoach = new t_player_coach
                {
                    memberId = request.MemberId,
                    toMemberId = request.ObjMemberId,
                    startdate = request.StartDate,
                    enddate = request.EndDate,
                    isPositive=request.IsPositive,
                    isCoach=request.IsCoach,
                    playerCoachStatus = PlayerCoachStatusEm.待同意,
                };
                _dbContext.Insert(playerCoach);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerCoachService.Insert", ex);
            }
            return flag;
        }
        //编辑绑定教练/学生
        public bool Edit(PlayerCoachRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.StartDate.IsEmpty())
                {
                    msg = "开始时间不能为空";
                    return flag;
                }

                var playerCoach = _dbContext.Get<t_player_coach>(request.Id);
                if (playerCoach != null&& playerCoach.memberId==request.MemberId)
                {
                    playerCoach.startdate = request.StartDate;
                    playerCoach.enddate = request.EndDate;
                    playerCoach.playerCoachStatus =  PlayerCoachStatusEm.待同意;
                    playerCoach.updatetime = DateTime.Now;
                    _dbContext.Update(playerCoach);
                    flag = true;
                }
                else
                {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerCoachService.Edit", ex);
            }
            return flag;
        }
        //删除
        public bool Delete(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playerCoach = _dbContext.Get<t_player_coach>(id);
                if (playerCoach != null&& playerCoach.memberId==memberId)
                {
                    playerCoach.isdelete = true;
                    playerCoach.updatetime = DateTime.Now;
                    _dbContext.Update(playerCoach);
                    flag = true;
                }
                else {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerCoachService.Delete", ex);
            }
            return flag;
        }
        //是否同意 教练或者学生的申请
        public bool Check(int id,bool isAgree, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {   
                var playerCoach = _dbContext.Get<t_player_coach>(id);
                if (playerCoach != null&& playerCoach.toMemberId==memberId)
                {
                    playerCoach.playerCoachStatus = isAgree ? PlayerCoachStatusEm.同意 : PlayerCoachStatusEm.拒绝;
                    playerCoach.updatetime = DateTime.Now;
                    _dbContext.Update(playerCoach);
                    flag = true;
                }
                else
                {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerCoachService.Check", ex);
            }
            return flag;
        }
        //教练下的学生列表
        public List<CoachPlayerResponse> Coach_PlayerList(PlayerCoachQueryRequest request)
        {
            List<CoachPlayerResponse> list = new List<CoachPlayerResponse>();
            try
            {
                var sql = @"select * from 
                            (
                            select a.*,b.completepinyin PlayerPinYinName,b.completename PlayerName,b.code PlayerCode,c.PlayerPoints
                                                        from t_player_coach a 
                                                        inner join t_member_player b on a.MemberId=b.memberId
                                                        inner join t_member_points c on a.memberId=c.memberId
                                                        where a.isdelete=0 and a.isCoach=0 and a.isPositive=1 and a.toMemberId=@MemberId
                            union all
                            select a.*,b.completepinyin PlayerPinYinName,b.completename PlayerName,b.code PlayerCode,c.PlayerPoints
                                                        from t_player_coach a 
                                                        inner join t_member_player b on a.ToMemberId=b.memberId
                                                        inner join t_member_points c on a.memberId=c.memberId
                                                        where a.isdelete=0 and a.isCoach=1 and a.isPositive=0 and a.memberId=@MemberId
                            ) a order by a.createTime desc 
                          ";
                int totalCount = 0;
                list = _dbContext.Page<CoachPlayerResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
                if (list != null && list.Count>0)
                {
                    foreach (var item in list)
                    {
                        if (item.MemberId == request.MemberId)
                        {
                            item.Flag = true;
                        }
                        if (item.PlayerCoachStatus == PlayerCoachStatusEm.同意)
                        {
                            var playerId = item.Flag ? item.ToMemberId : item.MemberId;
                            //参与比赛次数
                            item.Times = _dbContext.ExecuteScalar($"select count(distinct(eventGroupId)) from t_event_cycling_match_playerresult where playerId={playerId}").ToObjInt();
                            //指教期间获胜次数
                            StringBuilder sb = new StringBuilder();
                            if (item.EndDate.IsEmpty())
                            {
                                sb.Append($" and c.starteventdate>={item.StartDate} ");
                            }
                            else
                            {
                                sb.Append($" and c.starteventdate>={item.StartDate} and c.endeventdate<={item.EndDate}");
                            }
                            item.WinTimes = _dbContext.ExecuteScalar($@"select 
                            (select count(a.id) Count from t_event_cycling_match_playerresult a
                            inner join  t_event_cycling_match_teamresult b
                            on a.eventId = b.eventId and a.eventGroupId = b.eventGroupId and a.cyclingMatchId = b.cyclingMatchId
                            and a.groupNum = b.groupNum
                            inner join t_event c on a.eventId=c.id
                            where a.playerId = {playerId} and b.isWin = 1 {sb.ToString()})
                            +
                            (select count(a.id) Count from t_event_knockout_match_teamresult  a
                            left join t_event_player_signup b on a.groupNum=b.groupNum and a.eventGroupId=b.eventGroupId and a.eventId=a.eventId
                            inner join t_event c on a.eventId=c.id
                            where b.memberId ={playerId} and a.isWin = 1 {sb.ToString()})  as Count").ToObjInt();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerCoachService.Coach_PlayerList", ex);
            }
            return list;
        }
        //学生下的教练列表
        public List<PlayerCoachResponse> Player_CoachList(PlayerCoachQueryRequest request)
        {
            List< PlayerCoachResponse > list = new List<PlayerCoachResponse>();
            try
            {
                var sql = @"select * from 
                            (
                            select a.*,
		                            b.completepinyin CoachName,b.code CoachCode 
		                            from t_player_coach a 
		                            inner join t_member_coach  b on a.toMemberId=b.memberId
		                            where a.isdelete=0 and a.isCoach=0 and a.isPositive=1 and a.memberId=@MemberId
                            union all
                            select a.*,
		                            b.completepinyin CoachName,b.code CoachCode 
		                            from t_player_coach a 
		                            inner join t_member_coach  b on a.memberId=b.memberId
		                            where a.isdelete=0 and a.isCoach=1 and a.isPositive=0 and a.toMemberId=@MemberId
                            ) a order by a.createtime desc 
                          ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerCoachResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
                if (list != null && list.Count>0)
                {
                    foreach (var item in list)
                    {
                        if (item.MemberId == request.MemberId)
                        {
                            item.Flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerCoachService.Player_CoachList", ex);
            }
            return list;
        }
        // 学生当前教练
        public CurrentCoachResponse Player_CoachDetail(int memberId)
        {
            CurrentCoachResponse response = null;
            try
            {
                var sql = $@"select * from 
								(
								select a.Id,a.toMemberId MemberId,a.enddate,
										b.completepinyin CoachName
										from t_player_coach a 
										inner join t_member_coach  b on a.toMemberId=b.memberId
										where a.isdelete=0 and a.isCoach=0 and a.isPositive=1 and a.memberId={memberId} and a.playerCoachStatus={(int)PlayerCoachStatusEm.同意} and (isnull(a.enddate) or a.enddate>='{DateTime.Now.ToString("yyyy-MM")}')
								union all
								select  a.Id,a.memberId MemberId,a.enddate,
										b.completepinyin CoachName
										from t_player_coach a 
										inner join t_member_coach  b on a.memberId=b.memberId
										where a.isdelete=0 and a.isCoach=1 and a.isPositive=0 and a.toMemberId={memberId} and a.playerCoachStatus={(int)PlayerCoachStatusEm.同意} and (isnull(a.enddate) or a.enddate>='{DateTime.Now.ToString("yyyy-MM")}')
								) a order by a.enddate limit 1";
                var data = _dbContext.QueryFirstOrDefault<dynamic>(sql);
                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.enddate.ToObjStr()))
                    {
                        response = new CurrentCoachResponse
                        {
                            Id = data.Id,
                            CoachId = data.MemberId,
                            CoachName = data.CoachName
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerCoachService.Player_CoachDetail", ex);
            }
            return response;
        }
        //详情
        public PlayerCoachResponse Detail(int id)
        {
            PlayerCoachResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_player_coach>(id);
                if (detail != null)
                {
                    response = new PlayerCoachResponse
                    {
                        Id=id,
                        StartDate=detail.startdate,
                        EndDate=detail.enddate                           
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerCoachService.Detail", ex);
            }
            return response;
        }
    }
}
