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

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 淘汰赛对垒
    /// </summary>
    public class EventKnockoutMatchService: IEventKnockoutMatchService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventKnockoutMatchService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //替换教室
        public bool ReplaceRoom(int knockoutMatchId, int roomId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_knockout_match tmatch = _dbContext.Get<t_event_knockout_match>(knockoutMatchId);
                if (tmatch != null)
                {
                    t_event_room room = _dbContext.Get<t_event_room>(roomId);
                    if (room != null && room.roomStatus == RoomStatusEm.闲置)
                    {
                        tmatch.roomId = roomId;
                        tmatch.updatetime = DateTime.Now;
                        _dbContext.Update(tmatch);
                        flag = true;
                    }
                    else
                    {
                        msg = "教室信息有误";
                    }
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
                LogUtils.LogError("EventKnockoutMatchService.ReplaceRoom", ex);
            }
            return flag;
        }
        //替换裁判
        public bool ReplaceReferee(int knockoutMatchRefereeId, int refereeId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_knockout_match_referee tmatchreferee = _dbContext.Get<t_event_knockout_match_referee>(knockoutMatchRefereeId);
                if (tmatchreferee != null)
                {
                    t_event_referee_signup referee = _dbContext.Select<t_event_referee_signup>(c => c.memberId == refereeId && c.eventId == tmatchreferee.eventId).FirstOrDefault();
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
        //正反方互换
        public bool ReplaceMatch(int knockoutMatchId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_knockout_match tmatch = _dbContext.Get<t_event_knockout_match>(knockoutMatchId);
                if (tmatch != null)
                {
                    _dbContext.Execute($"update t_event_knockout_match set congroupNum={tmatch.progroupNum},progroupNum={tmatch.congroupNum},updatetime={DateTime.Now} where id={knockoutMatchId}");
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
                LogUtils.LogError("EventKnockoutMatchService.ReplaceMatch", ex);
            }
            return flag;
        }
    }
}
