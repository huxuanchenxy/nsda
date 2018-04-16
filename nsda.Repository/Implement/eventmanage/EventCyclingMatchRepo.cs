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
    }
}
