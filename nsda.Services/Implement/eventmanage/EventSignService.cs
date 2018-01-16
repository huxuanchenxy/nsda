﻿using nsda.Model.enums;
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

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事签到管理
    /// </summary>
    public class EventSignService: IEventSignService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventSignService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //裁判 选手签到
        public bool Sign(int id,int memberId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventsign eventsign = _dbContext.Get<t_eventsign>(id);
                if (eventsign == null || eventsign.memberId != memberId)
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
        //赛事管理员 批量签到/未到
        public bool BatchSign(List<int> id,int eventId,bool isNormal,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sql = "update t_eventsign set eventSignStatus=@EventSignStatus,signtime=@SignTime where Id in @Id and eventId=@EventId ";
                var dy = new DynamicParameters();
                dy.Add("Id", id.ToArray());
                dy.Add("EventId",eventId);
                dy.Add("EventSignStatus",isNormal? EventSignStatusEm.已签到:EventSignStatusEm.未到);
                dy.Add("SignTime", DateTime.Now);
                _dbContext.Execute(sql, dy);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventSignService.BatchSign", ex);
            }
            return flag;
        }
        //选手签到列表
        public List<PlayerSignResponse> PlayerSignList(PlayerSignQueryRequest request)
        {
            List<PlayerSignResponse> list = new List<PlayerSignResponse>();
            try
            {
                StringBuilder sb = new StringBuilder($@"select * from t_eventsign a 
                                                       inner join t_member b on a.memberId=b.id
                                                       inner join t_event c on a.eventId=c.id
                                                       where a.isdelete=0 and b.isdelete=0 and c.isdelete=0
                                                       and a.eventId=@EventId and a.groupId=@GroupId and c.memberId=@MemberId and eventSignType={EventSignTypeEm.选手}
                                                    ");
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (b.code like @KeyValue or b.completename like @KeyValue)");
                }
                int totalCount = 0;
                list = _dbContext.Page<PlayerSignResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
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
                StringBuilder sb = new StringBuilder($@"select * from t_eventsign a 
                                                       inner join t_member b on a.memberId=b.id
                                                       inner join t_event c on a.eventId=c.id
                                                       where a.isdelete=0 and b.isdelete=0 and c.isdelete=0
                                                       and a.eventId=@EventId and c.memberId=@MemberId and eventSignType={EventSignTypeEm.裁判}
                                                    ");
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" +request.KeyValue +"%";
                    sb.Append(" and (b.code like @KeyValue or b.completename like @KeyValue)");
                }
                int totalCount = 0;
                list = _dbContext.Page<RefereeSignResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.RefereeSignList", ex);
            }
            return list;
        }
        //选手/裁判获取签到信息
        public SignResponse GetSign(int eventId, int memberId)
        {
            SignResponse response = null;
            try
            {
                var sql = @"select a.id,b.code EventCode,b.name EventName,a.signdate,a.eventSignStatus from t_eventsign a
                            inner join t_event b on a.eventId=b.id
                            where a.isdelete=0 and a.eventId=@EventId and a.memberId=@MemberId and a.signdate=@SignDate";
                var dy = new DynamicParameters();
                dy.Add("EventId",eventId);
                dy.Add("MemberId",memberId);
                dy.Add("SignDate",DateTime.Now.ToShortDateString());
                response = _dbContext.QueryFirstOrDefault<SignResponse>(sql,dy);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventSignService.GetSign", ex);
            }
            return response;
        }
    }
}
