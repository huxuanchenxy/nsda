using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.enums;
using nsda.Model.dto.response;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 对垒业务操作
    /// </summary>
    public class EventMatchService: IEventMatchService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventMatchService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //替换教室
        public bool ReplaceRoom(int matchId, int roomId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_match tmatch = _dbContext.Get<t_match>(matchId);
                if (tmatch != null)
                {
                    t_eventroom room = _dbContext.Get<t_eventroom>(roomId);
                    if (room != null && room.roomStatus == RoomStatusEm.闲置)
                    {
                        tmatch.roomId = roomId;
                        tmatch.updatetime = DateTime.Now;
                        _dbContext.Update(tmatch);
                        flag = true;
                    }
                    else {
                        msg = "教室信息有误";
                    }
                }
                else {
                    msg = "对垒信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceRoom", ex);
            }
            return flag;
        }
        //替换裁判
        public bool ReplaceReferee(int matchrefereeId, int refereeId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_match_referee tmatchreferee = _dbContext.Get<t_match_referee>(matchrefereeId);
                if (tmatchreferee != null)
                {
                    t_referee_signup referee = _dbContext.Select<t_referee_signup>(c => c.memberId == refereeId && c.eventId == tmatchreferee.eventId).FirstOrDefault();
                    if (referee != null)
                    {
                        tmatchreferee.refereeId = refereeId;
                        tmatchreferee.updatetime = DateTime.Now;
                        _dbContext.Update(tmatchreferee);
                        flag = true;
                    }
                    else
                    {
                        msg = "裁判信息有误";
                    }
                }
                else
                {
                    msg = "需要被替换的裁判信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceReferee", ex);
            }
            return flag;
        }
        //替换对垒位置
        public bool ReplaceMatch(int matchId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_match tmatch = _dbContext.Get<t_match>(matchId);
                if (tmatch != null)
                {
                    _dbContext.Execute($"update t_match set congroupNum={tmatch.progroupNum},progroupNum={tmatch.congroupNum},updatetime={DateTime.Now} where id={matchId}");
                    flag = true;
                }
                else
                {
                    msg = "对垒信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.ReplaceMatch", ex);
            }
            return flag;
        }
        //预览评分标签
        public List<RatingSinglLabelResponse> PreviewRatingSinglLabel(MatchCommonRequest request)
        {
            List<RatingSinglLabelResponse> list = new List<RatingSinglLabelResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.PreviewRatingSinglLabel", ex);
            }
            return list;
        }
        //对垒表
        public List<ListMatchResponse> ListMatch(ListMatchRequest request)
        {
            List<ListMatchResponse> list = new List<ListMatchResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.ListMatch", ex);
            }
            return list;
        }
        //录入成绩
        public bool RecordOfEntry(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventMatchService.RecordOfEntry", ex);
            }
            return flag;
        }
        //评分列表
        public List<RecordOfListResponse> RecordOfList(MatchCommonRequest request)
        {
            List<RecordOfListResponse> list = new List<RecordOfListResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.RecordOfList", ex);
            }
            return list;
        }
        //评分详情
        public RecordOfDetailResponse RecordOfDetail(RecordOfDetailRequest request)
        {
            RecordOfDetailResponse response = null;
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.RecordOfDetail", ex);
            }
            return response;
        }
        //闲置的裁判
        public List<BaseListResponse> ListReferee(MatchCommonRequest request)
        {
            List<BaseListResponse> list = new List<BaseListResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.ListReferee", ex);
            }
            return list;
        }
        //闲置的教室
        public List<BaseListResponse> ListRoom(MatchCommonRequest request)
        {
            List<BaseListResponse> list = new List<BaseListResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventMatchService.ListRoom", ex);
            }
            return list;
        }
    }
}
