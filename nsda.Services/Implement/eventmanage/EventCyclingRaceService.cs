using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Repository.Contract.eventmanage;
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
    /// 循环赛管理
    /// </summary>
    public class EventCyclingRaceService : IEventCyclingRaceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IEventCyclingMatchRepo _eventCyclingMatchRepo;

        public EventCyclingRaceService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService, IEventCyclingMatchRepo eventCyclingMatchRepo)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _eventCyclingMatchRepo = eventCyclingMatchRepo;
        }

        //开始循环赛  生成循环赛对垒表
        public bool Start(int eventId, int eventGroupId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //循环赛对垒表
                List<t_event_cycling_match> arrobj = new List<t_event_cycling_match>();

                var cyclingracesettings = _dbContext.Select<t_event_cycling_settings>(c => c.eventGroupId == eventGroupId && c.eventId == eventId).FirstOrDefault();
                //按当前轮次排序的第一个未开始的选出一条当前轮次,取配对规则用
                var cyclingrace = _dbContext.Select<t_event_cycling>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.cyclingRaceStatus == Model.enums.CyclingRaceStatusEm.未开始).OrderBy(c => c.currentround).FirstOrDefault();
                //当前轮次的flight
                var cyclingracedetail = _dbContext.Select<t_event_cycling_detail>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.cyclingraceId == cyclingrace.id).OrderBy(c => c.id).ToList();
                var dcmax = cyclingracedetail.Count;//flight下标
                //获取报名队伍信息 签到里取
                //var playersSign = _dbContext.Select<t_event_sign>(c => c.eventId == eventId && c.eventSignType == Model.enums.EventSignTypeEm.选手 && c.isdelete == false && c.isStop == false && c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.signtime.ToString() != string.Empty).ToList();
                var playersSignQ = _dbContext.Select<t_event_sign>(c => c.eventId == eventId && c.eventSignType == Model.enums.EventSignTypeEm.选手 && c.isdelete == false && c.isStop == false).ToList();
                var qpsq = from p in playersSignQ
                           where p.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && !string.IsNullOrEmpty(p.signtime.ToString())
                           select new t_event_sign
                           {
                               id = p.id,
                               eventGroupId = p.eventGroupId,
                               eventId = p.eventId
                           ,
                               eventSignStatus = p.eventSignStatus,
                               memberId = p.memberId
                           };
                List<t_event_sign> playersSign = qpsq.Cast<t_event_sign>().ToList<t_event_sign>();

                //取组队信息
                var players = _dbContext.Select<t_event_player_signup>(c => c.eventId == eventId && c.eventGroupId == eventGroupId && c.isdelete == false && c.signUpStatus == Model.enums.SignUpStatusEm.报名成功).ToList();

                //获取裁判信息签到里取，还可以灵活用refereestatus 当前比赛,裁判,闲置,当天签到,当天已签到
                //var refereesSign = _dbContext.Select<t_event_sign>(c => (c.eventGroupId == 0 || c.eventGroupId == eventGroupId) && c.eventId == eventId && c.eventSignType == Model.enums.EventSignTypeEm.裁判 && c.isdelete == false && c.isStop == false && c.refereeStatus == Model.enums.RefereeStatusEm.闲置 && c.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && c.signtime.ToString() != string.Empty).ToList();

                var refereesSignQ = _dbContext.Select<t_event_sign>(c => (c.eventGroupId == 0 || c.eventGroupId == eventGroupId) && c.eventId == eventId && c.eventSignType == Model.enums.EventSignTypeEm.裁判 && c.isdelete == false && c.isStop == false && c.refereeStatus == Model.enums.RefereeStatusEm.闲置).ToList();
                var qfsq = from p in refereesSignQ
                           where p.signdate.ToShortDateString() == DateTime.Now.ToShortDateString() && !string.IsNullOrEmpty(p.signtime.ToString())
                           select new t_event_sign
                           {
                               id = p.id,
                               eventGroupId = p.eventGroupId,
                               eventId = p.eventId,
                               eventSignStatus = p.eventSignStatus,
                               memberId = p.memberId
                           };
                List<t_event_sign> refereesSign = qfsq.Cast<t_event_sign>().ToList<t_event_sign>();
                //裁判取groupid意向用
                var referee = _dbContext.Select<t_event_referee_signup>(c => c.eventId == eventId && c.isdelete == false && c.refereeSignUpStatus == Model.enums.RefereeSignUpStatusEm.已录取).ToList();

                //教室信息 选随机的或者已经限制过当前组的room
                var room = _dbContext.Select<t_event_room>(c => c.isdelete == false && c.eventId == eventId && c.roomStatus == Model.enums.RoomStatusEm.闲置 && (c.eventgroupId == 0 || c.eventgroupId == eventGroupId)).ToList();


                //计算当前pk最少要多少裁判,房间
                var query = from p in players
                            group p by new { p.groupnum } into g
                            select new t_event_player_signup { groupnum = g.Key.groupnum };
                List<t_event_player_signup> teamlist = query.Cast<t_event_player_signup>().ToList<t_event_player_signup>();
                //(1)先两两分组,先计算一共有几场pk
                var pk = 0;
                if (teamlist.Count % 2 == 0)//正好2的整数倍
                {
                    pk = teamlist.Count / 2;
                }
                else
                {
                    pk = (teamlist.Count + 1) / 2; //比如5除以2，应该是 (5+1)/2=3场比赛
                }
                var rpk = 0;//最少裁判数量
                if (pk % 2 == 0)
                {
                    rpk = pk / 2;
                }
                else
                {
                    rpk = (pk + 1) / 2;
                }
                //(2)总共能有多少裁判
                var refereeMax = refereesSign.Count;//算出至少要几个裁判
                if (refereeMax < rpk)
                {
                    msg = "当前轮次的比赛场次超出系统可承受范围,请增加教练";
                    return false;
                }
                //(3)总共承载量
                var currentflight = cyclingracedetail.Count;
                var roomMax = room.Count * currentflight;
                if (roomMax < pk)
                {
                    msg = "当前轮次的比赛场次超出系统可承受范围,请增加教室或加Flight";
                    return false;
                }
                //用户设几间房就充分利用,设了是用户自己的事情
                //如果是第一轮则随机配对
                var newteamlist = Utility.RandomSortList(teamlist);
                for (int i = 0; i < newteamlist.Count; i = i + 2)//选手总归要都待分配的
                {

                    string _progroupNum = string.Empty;
                    string _congroupNum = string.Empty;
                    bool _isbye = false;
                    _progroupNum = newteamlist[i].groupnum;
                    try
                    {
                        _congroupNum = newteamlist[i + 1].groupnum;
                    }
                    catch (Exception ex) { _isbye = true; }

                    arrobj.Add(new t_event_cycling_match()
                    {
                        eventId = eventId
                        ,
                        eventGroupId = eventGroupId
                        ,
                        progroupNum = _progroupNum
                        ,
                        congroupNum = _congroupNum
                        ,
                        isBye = _isbye
                    });

                }
                //填t_event_cycling_detail信息
                var dcindex = 0;
                var rindex = 0;//分配room用，裁判也能用
                foreach (var o in arrobj)
                {
                    if (dcindex == dcmax)
                    {
                        dcindex = 0;
                        rindex++;//当前flight结束则找下一间房间
                    }
                    o.cyclingDetailId = cyclingracedetail[dcindex].id;
                    if (o.isBye == false)//不轮空则分配房间和裁判，轮空就不分配房间和裁判
                    {
                        o.roomId = room[rindex].id;
                        o.refereeId = refereesSign[rindex].id;
                    }
                    dcindex++;
                }

                //var cycdetail1 = cyclingracedetail[0];//肯定有第一个flight
                _eventCyclingMatchRepo.GenerEventCyclingMatch(arrobj, cyclingrace);

                string ss = string.Empty;
                //获取教练信息
                //排对垒 第一轮 可以忽略对垒规则

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventCyclingRaceService.Start", ex);
            }
            return flag;
        }
        //开始下一轮
        public bool Next(int eventId, int eventGroupId, int current, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //先检测是否上一轮成绩都录入完
                //看是否有双赢双输队伍
                //查看上一轮所有的对垒 
                var cyclingracesettings = _dbContext.Select<t_event_cycling_settings>(c => c.eventGroupId == eventGroupId && c.eventId == eventId).FirstOrDefault();
                var cyclingrace = _dbContext.Select<t_event_cycling>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.currentround == 1).FirstOrDefault();
                var cyclingracedetail = _dbContext.Select<t_event_cycling_detail>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.cyclingraceId == cyclingrace.id).FirstOrDefault();

                //获取报名队伍信息
                //获取裁判信息
                //教室信息
                var room = _dbContext.Query<t_event_room>($"").ToList();
                //获取教练信息
                //排对垒 根据对垒规则 如果随机就无需查对垒规则


            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventCyclingRaceService.Next", ex);
            }
            return flag;
        }
        public List<TrackCyclingResponse> TrackCycling(int eventId, int eventGroupId, string keyValue)
        {
            List<TrackCyclingResponse> list = new List<TrackCyclingResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventCyclingRaceService.TrackKnockout", ex);
            }
            return list;
        }

    }
}
