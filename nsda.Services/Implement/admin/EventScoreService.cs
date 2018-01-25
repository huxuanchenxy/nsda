using nsda.Repository;
using nsda.Services.Contract.admin;
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
using nsda.Services.admin;
using nsda.Model;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 赛事评分表管理
    /// </summary>
    public class EventScoreService: IEventScoreService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public EventScoreService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        public bool Insert(EventScoreRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            { 
                _dbContext.Insert(new t_event_score {
                      eventId=request.EventId,
                      eventGroupId=request.EventGroupId,
                      filepath=request.FilePath,
                      title=request.Title
                 });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventScoreService.Insert", ex);
            }
            return flag;
        }

        public bool Delete(int id,int sysUserId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_event_score>(id);
                if (detail != null)
                {
                    detail.isdelete = true;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
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
                LogUtils.LogError("EventScoreService.Delete", ex);
            }
            return flag;
        }

        public List<EventScoreResponse> List(int eventId,int eventGroupId)
        {
            List<EventScoreResponse> list = new List<EventScoreResponse>();
            try
            {
                var sql = $"select * from t_event_score where isdelete=0 and eventId={eventId} and groupId={eventGroupId} order by createtime desc";
                list = _dbContext.Query<EventScoreResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventScoreService.List", ex);
            }
            return list;
        }

        //评分单下载
        public List<EventScoreResponse> PlayerList(PlayerEventScoreQueryRequest request)
        {
            List<EventScoreResponse> list = new List<EventScoreResponse>();
            try
            {
                var sql = $@"select * from t_event_score where isdelete=0 and 
                             eventGroupId in (select eventGroupId from t_event_player_signup where memberId=@MemberId and signUpStatus in ({ParamsConfig._signup_in})) order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<EventScoreResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventScoreService.PlayerList", ex);
            }
            return list;
        }
    }
}
