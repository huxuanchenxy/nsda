﻿using nsda.Repository;
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
using nsda.Services.Contract.member;
using nsda.Services.Contract.admin;
using nsda.Model;

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
                var sql = @"select a.*,b.name MemberName,b.code MemberCode,d.points MemberPoints,e.points ToMemberPoints,c.name ToMemberName,c.code ToMemberCode  from t_player_coach a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_member c on a.toMemberId=c.id
                            inner join t_member_points d on a.memberId=d.memberId
                            inner join t_member_points e on a.toMemberId=e.memberId
                            where a.isdelete=0 and ((a.isCoach=1 and a.isPositive=0 anda. memberId=@MemberId) or (a.isCoach=0 and a.isPositive=1 and a.toMemberId=@MemberId))
                            order by a.createtime desc
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
                            //所在学校
                            item.School=_dbContext.ExecuteScalar($"select b.chinessname from t_player_edu  a inner join t_sys_school b on a.schoolId=b.id where a.memberid={(item.Flag?item.ToMemberId:item.MemberId)} and a.isdelete=0 order by startdate desc limit 1").ToObjStr();
                            //参与比赛次数
                            item.Times = _dbContext.ExecuteScalar($"select count(1) from t_event_player_signup where isdelete=0 and  signUpStatus in ({ParamsConfig._signup_in})").ToObjInt();
                            //指教期间获胜次数
                            item.WinTimes = _dbContext.ExecuteScalar($"select count(1) from t_event_player_signup where isdelete=0 and  signUpStatus in ({ParamsConfig._signup_in})").ToObjInt();
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
                var sql = @"select a.*,b.name MemberName,b.code MemberCode,c.name ToMemberName,c.code ToMemberCode from t_player_coach a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_member c on a.toMemberId=c.id
                            where a.isdelete=0 and ((a.isCoach=0 and a.isPositive=1 and a.memberId=@MemberId) or (a.isCoach=1 and a.isPositive=0 and a.toMemberId=@MemberId))
                            order by a.startdate desc
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
                var sql = $@"select a.*,b.completename MemberName,b.code MemberCode,c.completename ToMemberName,c.code ToMemberCode from t_player_coach a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_member c on a.toMemberId=c.id
                            where a.isdelete=0 and a.playerCoachStatus={PlayerCoachStatusEm.同意} and ((a.isCoach=0 and a.isPositive=1 and a.memberId={memberId}) or (a.isCoach=1 and a.isPositive=0 and a.toMemberId={memberId}))
                            order by a.startdate desc limit 1";
                var data = _dbContext.QueryFirstOrDefault<PlayerCoachResponse>(sql);
                if (data != null)
                {
                    response = new CurrentCoachResponse {
                        Id = data.Id,
                        CoachId = data.MemberId == memberId ? data.ToMemberId:data.MemberId,
                        CoachName=data.MemberId==memberId?data.ToMemberName:data.MemberName
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerCoachService.Player_CoachDetail", ex);
            }
            return response;
        }
    }
}
