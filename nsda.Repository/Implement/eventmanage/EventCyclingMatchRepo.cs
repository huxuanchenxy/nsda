using Dapper;
using nsda.Repository.Contract.eventmanage;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.response;
using nsda.Model.dto.request;
using nsda.Models;

namespace nsda.Repository.Implement.eventmanage
{
    public class EventCyclingMatchRepo : IEventCyclingMatchRepo
    {
        IDBContext _dbContext;

        public EventCyclingMatchRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void GenerEventCyclingMatch(List<t_event_cycling_match> list, t_event_cycling cyc)
        {
            try
            {
                _dbContext.BeginTransaction();

                //先把当前轮次，组，比赛的准备录入状态的对垒表删除
                string sql1 = $@" DELETE from  t_event_cycling_match
                                where cyclingMatchStatus = 1 and eventid = {cyc.eventId} and eventgroupid = {cyc.eventGroupId} and cyclingDetailId in (SELECT id from t_event_cycling_detail where cyclingraceid = {cyc.id})
                                ";
                object r1 = _dbContext.ExecuteScalar(sql1);

                string sql2 = $@" insert into t_event_cycling_match
                                    (eventId,eventGroupId,progroupNum
                                    ,congroupNum
                                    ,roomId
                                    ,refereeId
                                    ,cyclingDetailId
                                    ,isBye
                                    ,cyclingMatchStatus
                                    ,createtime
                                    ,updatetime
                                    ,isdelete
                                    ) ";
                string values = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        values += $@" SELECT '{list[i].eventId}','{list[i].eventGroupId}','{list[i].progroupNum}','{list[i].congroupNum}','{list[i].roomId}'
                                    ,'{list[i].refereeId}','{list[i].cyclingDetailId}',{list[i].isBye},1,'{DateTime.Now}','{DateTime.Now}',FALSE ";
                    }
                    else
                    {
                        values += $@" union ALL
                                    SELECT '{list[i].eventId}','{list[i].eventGroupId}','{list[i].progroupNum}','{list[i].congroupNum}','{list[i].roomId}'
                                    ,'{list[i].refereeId}','{list[i].cyclingDetailId}',{list[i].isBye},1,'{DateTime.Now}','{DateTime.Now}',FALSE ";
                    }
                }
                if (list.Count > 0)
                {
                    sql2 = sql2 + values;
                    object r2 = _dbContext.ExecuteScalar(sql2);
                }


                _dbContext.CommitChanges();

            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("EventCyclingMatchRepo.GenerEventCyclingMatch", ex);
            }
        }



        public List<t_event_cycling_match> GetCurEventCyclingMatch(t_event_cycling cyc)
        {
            var curCyc = _dbContext.Select<t_event_cycling>(c => c.eventGroupId == cyc.eventGroupId && c.eventId == cyc.eventId && c.cyclingRaceStatus == Model.enums.CyclingRaceStatusEm.未开始).OrderBy(c => c.currentround).FirstOrDefault();

            List<t_event_cycling_match> ret;

            string sql = $@" select * from t_event_cycling_match
                            where cyclingDetailId in (select id  from t_event_cycling_detail where cyclingraceId = '{curCyc.id}' )
                            ORDER BY id ";
            ret = _dbContext.Query<t_event_cycling_match>(sql).ToList();
            return ret;

        }

        public void GotoNext(int eventId, int eventGroupId, int currentRound)
        {
            try
            {
                _dbContext.BeginTransaction();

                //开启当前轮次比赛
                string sql1 = $@" update t_event_cycling set cyclingRaceStatus = 2 where eventId = {eventId} and eventGroupId = {eventGroupId} and currentround = {currentRound} ";

                object r1 = _dbContext.ExecuteScalar(sql1);


                List<t_event_cycling_match> ret1;//当前轮次的对垒表

                string sql2 = $@" select * from t_event_cycling_match
                            where cyclingDetailId in (select id  from t_event_cycling_detail where cyclingraceId = ( select id from t_event_cycling where eventId = {eventId} and eventGroupId = {eventGroupId} and currentround = {currentRound}) )
                             ";
                ret1 = _dbContext.Query<t_event_cycling_match>(sql2).ToList();

                string sql5 = string.Empty;//循环赛对垒表队伍分数初始化
                sql5 = $@" insert into t_event_cycling_match_teamresult
                            (eventId,eventgroupId,cyclingMatchId,groupNum,totalScore,isWin,winType,createtime,updatetime,isdelete) ";
                string sql6 = string.Empty;//循环赛对垒表选手名次
                sql6 = $@" insert into t_event_cycling_match_playerresult
                            (eventid,eventgroupid,cyclingMatchId,playerId,refereeId,groupNum,ranking,score,createtime,updatetime,isdelete) ";


                for (int i = 0; i < ret1.Count; i++)
                {
                    //先清空当前对垒的分数信息
                    string sql3 = $@" DELETE FROM t_event_cycling_match_teamresult
                            where eventid = {ret1[i].eventId} and eventGroupId = {ret1[i].eventGroupId} and cyclingMatchId = {ret1[i].id} ";
                    _dbContext.ExecuteScalar(sql3);

                    string sql4 = $@" DELETE FROM t_event_cycling_match_playerresult
                            where eventid = {ret1[i].eventId} and eventGroupId = {ret1[i].eventGroupId} and cyclingMatchId = {ret1[i].id} ";
                    _dbContext.ExecuteScalar(sql4);


                    if (i == 0)
                    {
                        sql5 += $@"
                            select  '{ret1[i].eventId}','{ret1[i].eventGroupId}','{ret1[i].id}','{ret1[i].progroupNum}',0,false,0,'{ DateTime.Now}','{ DateTime.Now}',false
                            UNION ALL
                            select  '{ret1[i].eventId}','{ret1[i].eventGroupId}','{ret1[i].id}','{ret1[i].congroupNum}',0,false,0,'{ DateTime.Now}','{ DateTime.Now}',false
                            ";
                    }
                    else
                    {
                        sql5 += $@" 
                            UNION ALL
                            select  '{ret1[i].eventId}','{ret1[i].eventGroupId}','{ret1[i].id}','{ret1[i].progroupNum}',0,false,0,'{DateTime.Now}','{DateTime.Now}',false
                            UNION ALL
                            select  '{ret1[i].eventId}','{ret1[i].eventGroupId}','{ret1[i].id}','{ret1[i].congroupNum}',0,false,0,'{DateTime.Now}','{DateTime.Now}',false
                            ";
                    }
                    //当前对垒player 应该有4个人
                    var currentPlayers = _dbContext.Select<t_event_player_signup>(c => c.eventId == ret1[i].eventId && c.eventGroupId == ret1[i].eventGroupId && (c.groupnum == ret1[i].progroupNum || c.groupnum == ret1[i].congroupNum)).ToList(); 


                    if (i == 0)
                    {
                        int j = 0;
                        foreach (var cc in currentPlayers)
                        {
                            if (j == 0)
                            {
                                sql6 += $@" select '{ret1[i].eventId}','{ret1[i].eventGroupId }' ,'{ret1[i].id}','{cc.memberId}','{ret1[i].refereeId}','{cc.groupnum}',0,0,'{DateTime.Now}','{DateTime.Now}',false ";
                            }
                            else
                            {
                                sql6 += $@" UNION ALL select '{ret1[i].eventId}','{ret1[i].eventGroupId }' ,'{ret1[i].id}','{cc.memberId}','{ret1[i].refereeId}','{cc.groupnum}',0,0,'{DateTime.Now}','{DateTime.Now}',false ";
                            }
                            j++;
                        }
                    }
                    else
                    {
                        foreach (var cc in currentPlayers)
                        {

                            sql6 += $@" UNION ALL select '{ret1[i].eventId}','{ret1[i].eventGroupId }' ,'{ret1[i].id}','{cc.memberId}','{ret1[i].refereeId}','{cc.groupnum}',0,0,'{DateTime.Now}','{DateTime.Now}',false ";

                        }
                    }
                }
                if (ret1.Count > 0)
                {
                    _dbContext.ExecuteScalar(sql5);
                    _dbContext.ExecuteScalar(sql6);
                }


                _dbContext.CommitChanges();

            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("EventCyclingMatchRepo.GenerEventCyclingMatch", ex);
            }
        }

        /// <summary>
        /// 当前轮次的track
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventGroupId"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<TrackCyclingResponse> GetTrackCyclingCur(int eventId, int eventGroupId, string keyValue)
        {
            List<TrackCyclingResponse> list = new List<TrackCyclingResponse>();

            StringBuilder join = new StringBuilder();
            //if (request.KeyValue.IsNotEmpty())
            //{
            //    request.KeyValue = $"%{request.KeyValue}%";
            //    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
            //}
            var sql = $@" SELECT * FROM t_event_cycling_match
                          WHERE cyclingDetailId IN (SELECT id FROM t_event_cycling_detail
                          WHERE eventId = {eventId}  AND eventGroupId = {eventGroupId} AND cyclingraceId = (
                            SELECT id FROM t_event_cycling
                                WHERE eventId = {eventId} AND eventGroupId = {eventGroupId} AND cyclingRaceStatus = 2
                                ) 
                        )";
            List<t_event_cycling_match>  matchs = _dbContext.Query<t_event_cycling_match>(sql).ToList();

            
            //初筛team成绩
            var teams = _dbContext.Select<t_event_cycling_match_teamresult>(c => c.eventId == eventId && c.eventGroupId == eventGroupId).ToList();
            //初筛player成绩
            var players = _dbContext.Select<t_event_cycling_match_playerresult>(c => c.eventId == eventId && c.eventGroupId == eventGroupId).ToList();
            foreach (var li in matchs)
            {
                TrackCyclingResponse obj = new TrackCyclingResponse()
                {
                    id = li.id,
                    eventId = li.eventId,
                    eventGroupId = li.eventGroupId
                ,
                    congroupNum = li.congroupNum,
                    cyclingDetailId = li.cyclingDetailId,
                    cyclingMatchStatus = li.cyclingMatchStatus
                ,
                    isBye = li.isBye,
                    progroupNum = li.progroupNum,
                    refereeId = (int)li.refereeId,
                    roomId = (int)li.roomId
                };
                
                var q1 = from t in teams
                         where (t.groupNum == li.progroupNum || t.groupNum == li.congroupNum ) && t.eventId == eventId && t.eventGroupId == eventGroupId && t.cyclingMatchId == li.id
                         select t;
                //应该有两条记录，说明两个队伍pk
                List<t_event_cycling_match_teamresult> curTeams = q1.Cast<t_event_cycling_match_teamresult>().ToList<t_event_cycling_match_teamresult>();
                List<TrackCyclingTeamResponse> teamResp = new List<TrackCyclingTeamResponse>();
                foreach (var ct in curTeams)
                {
                    TrackCyclingTeamResponse ttp = new TrackCyclingTeamResponse() { id= ct.id, cyclingMatchId = ct.cyclingMatchId
                    , eventGroupId = ct.eventGroupId, eventId = ct.eventId, groupNum = ct.groupNum, isWin = ct.isWin
                    , totalScore = ct.totalScore, winType = ct.winType};

                    var q2 = from pp in players
                             where pp.eventId == eventId && pp.eventGroupId == eventGroupId && pp.cyclingMatchId == li.id && pp.groupNum == ct.groupNum
                             select pp;
                    List<t_event_cycling_match_playerresult> curPlayers = q2.Cast<t_event_cycling_match_playerresult>().ToList<t_event_cycling_match_playerresult>();
                    List<TrackCyclingPlayerResponse> arrPlayers = new List<TrackCyclingPlayerResponse>();
                    foreach (var pl in curPlayers)
                    {
                        var objp = new TrackCyclingPlayerResponse()
                        {
                            id = pl.id,
                            cyclingMatchId = pl.cyclingMatchId
                        ,
                            eventGroupId = pl.eventGroupId,
                            eventId = pl.eventId,
                            groupNum = pl.groupNum
                        ,
                            playerId = pl.playerId,
                            ranking = pl.ranking,
                            refereeId = pl.refereeId,
                            score = pl.score
                        };
                        arrPlayers.Add(objp);
                    }
                    ttp.players = arrPlayers;
                    teamResp.Add(ttp);

                }
                obj.teams = teamResp;

                list.Add(obj);
            }

            return list;
        }

        public t_event_cycling GetCurCycling(int eventId, int eventGroupId)
        {
            //取当前比赛没结束的当前轮次的运行状态,左侧菜单跳转判断用,1就是未开始跳生成对垒表页面，2就是一开始只能跳track确认页面
            var ret = _dbContext.Select<t_event_cycling>(c => c.eventId == eventId && c.eventGroupId == eventGroupId && c.cyclingRaceStatus != Model.enums.CyclingRaceStatusEm.已结束).OrderBy(c => c.currentround).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// track查询
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventGroupId"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<TrackCyclingResponse> GetTrackCycling(int eventId, int eventGroupId, string keyValue)
        {
            List<TrackCyclingResponse> list = new List<TrackCyclingResponse>();

            StringBuilder join = new StringBuilder();
            //if (request.KeyValue.IsNotEmpty())
            //{
            //    request.KeyValue = $"%{request.KeyValue}%";
            //    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
            //}
            var sql = $@" SELECT * FROM t_event_cycling_match
                          WHERE cyclingDetailId IN (SELECT id FROM t_event_cycling_detail
                          WHERE eventId = {eventId}  AND eventGroupId = {eventGroupId} AND cyclingraceId = (
                            SELECT id FROM t_event_cycling
                                WHERE eventId = {eventId} AND eventGroupId = {eventGroupId} AND cyclingRaceStatus = 2
                                ) 
                        )";
            List<t_event_cycling_match> matchs = _dbContext.Query<t_event_cycling_match>(sql).ToList();


            //初筛team成绩
            var teams = _dbContext.Select<t_event_cycling_match_teamresult>(c => c.eventId == eventId && c.eventGroupId == eventGroupId).ToList();
            //初筛player成绩
            var players = _dbContext.Select<t_event_cycling_match_playerresult>(c => c.eventId == eventId && c.eventGroupId == eventGroupId).ToList();
            foreach (var li in matchs)
            {
                TrackCyclingResponse obj = new TrackCyclingResponse()
                {
                    id = li.id,
                    eventId = li.eventId,
                    eventGroupId = li.eventGroupId
                ,
                    congroupNum = li.congroupNum,
                    cyclingDetailId = li.cyclingDetailId,
                    cyclingMatchStatus = li.cyclingMatchStatus
                ,
                    isBye = li.isBye,
                    progroupNum = li.progroupNum,
                    refereeId = (int)li.refereeId,
                    roomId = (int)li.roomId
                };

                var q1 = from t in teams
                         where (t.groupNum == li.progroupNum || t.groupNum == li.congroupNum) && t.eventId == eventId && t.eventGroupId == eventGroupId && t.cyclingMatchId == li.id
                         select t;
                //应该有两条记录，说明两个队伍pk
                List<t_event_cycling_match_teamresult> curTeams = q1.Cast<t_event_cycling_match_teamresult>().ToList<t_event_cycling_match_teamresult>();
                List<TrackCyclingTeamResponse> teamResp = new List<TrackCyclingTeamResponse>();
                foreach (var ct in curTeams)
                {
                    TrackCyclingTeamResponse ttp = new TrackCyclingTeamResponse()
                    {
                        id = ct.id,
                        cyclingMatchId = ct.cyclingMatchId
                    ,
                        eventGroupId = ct.eventGroupId,
                        eventId = ct.eventId,
                        groupNum = ct.groupNum,
                        isWin = ct.isWin
                    ,
                        totalScore = ct.totalScore,
                        winType = ct.winType
                    };

                    var q2 = from pp in players
                             where pp.eventId == eventId && pp.eventGroupId == eventGroupId && pp.cyclingMatchId == li.id && pp.groupNum == ct.groupNum
                             select pp;
                    List<t_event_cycling_match_playerresult> curPlayers = q2.Cast<t_event_cycling_match_playerresult>().ToList<t_event_cycling_match_playerresult>();
                    List<TrackCyclingPlayerResponse> arrPlayers = new List<TrackCyclingPlayerResponse>();
                    foreach (var pl in curPlayers)
                    {
                        var objp = new TrackCyclingPlayerResponse() {id=pl.id, cyclingMatchId = pl.cyclingMatchId
                        , eventGroupId = pl.eventGroupId, eventId = pl.eventId, groupNum = pl.groupNum
                        , playerId = pl.playerId, ranking = pl.ranking, refereeId = pl.refereeId, score = pl.score};
                        arrPlayers.Add(objp);
                    }
                    ttp.players = arrPlayers;
                    teamResp.Add(ttp);

                }
                obj.teams = teamResp;

                list.Add(obj);
            }

            return list;
        }

    }
}
