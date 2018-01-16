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
                if (request.Title.IsEmpty())
                {
                    msg = "评分单标题不能为空";
                    return flag;
                }

                if (request.FilePath.IsEmpty())
                {
                    msg = "附件不能为空";
                    return flag;
                }

                _dbContext.Insert(new t_event_score {
                      eventid=request.EventId,
                      eventGroupId=request.EventGroupId,
                      filepath=request.FilePath,
                      remark=request.Remark,
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

        public bool Edit(EventScoreRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Title.IsEmpty())
                {
                    msg = "评分单标题不能为空";
                    return flag;
                }

                if (request.FilePath.IsEmpty())
                {
                    msg = "附件不能为空";
                    return flag;
                }
                var eventscore = _dbContext.Get<t_event_score>(request.Id);
                if (eventscore != null)
                {
                    eventscore.filepath = request.FilePath;
                    eventscore.title = request.Title;
                    eventscore.remark = request.Remark;
                    eventscore.updatetime = DateTime.Now;
                    _dbContext.Update(eventscore);
                    flag = true;
                }
                else
                {
                    msg = "资料信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventScoreService.Edit", ex);
            }
            return flag;
        }

        public bool Delete(int id, out string msg)
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

        public EventScoreResponse Detail(int id)
        {
            EventScoreResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_event_score>(id);
                if (detail != null)
                {
                    response = new EventScoreResponse
                    {
                        CreateTime = detail.createtime,
                        FilePath = detail.filepath,
                        Id = detail.id,
                        EventId=detail.eventid,
                        EventGroupId=detail.eventGroupId,
                        Remark = detail.remark,
                        Title = detail.title,
                        UpdateTime = detail.updatetime
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventScoreService.Detail", ex);
            }
            return response;
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
                             groupId in (select groupId from t_player_signup where memberId=@MemberId and signUpStatus in ({ParamsConfig._signup_in})) order by createtime desc";
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
