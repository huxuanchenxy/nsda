using Dapper;
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
using nsda.Model.enums;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事管理
    /// </summary>
    public class EventService: IEventService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //新增赛事
        public bool Insert(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.BeginTransaction();


                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventService.Insert", ex);
            }
            return flag;
        }
        //编辑赛事
        public bool Edit(out string msg)
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
                LogUtils.LogError("EventService.Edit", ex);
            }
            return flag;
        }
        //设定赛事级别
        public bool SettingLevel(int id, EventLevelEm eventLevel, int sysUserId, out string msg)
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
                LogUtils.LogError("EventService.SettingLevel", ex);
            }
            return flag;
        }
        //赛事详情
        public EventResponse Detail(int id)
        {
            EventResponse response = null;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(id);
                if (tevent != null)
                {

                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.Detail", ex);
            }
            return response;
        }
        //选手 裁判查询赛事列表
        public PagedList<PlayerOrRefereeEventResponse> PlayerOrRefereeEvent(PlayerOrRefereeEventQueryRequest request)
        {
            PagedList<PlayerOrRefereeEventResponse> list = new PagedList<PlayerOrRefereeEventResponse>();
            try
            {
                StringBuilder sb = new StringBuilder(@"select * from t_event where isdelete=0 and status in (0,1,2,3,4,5) ");
                if (request.ProvinceId.HasValue)
                {
                    sb.Append(" and provinceId=@provinceId ");
                }
                if (request.CityId.HasValue)
                {
                    sb.Append(" and cityId=@CityId ");
                }
                if (request.EventLevel.HasValue)
                {
                    sb.Append(" and eventLevel=@EventLevel ");
                }
                if (request.KeyValue.IsEmpty())
                {
                    request.KeyValue="%"+request.KeyValue+"%";
                    sb.Append(" and (code like @KeyValue or name like @KeyValue)");
                }
                if (request.StartDate.HasValue)
                {
                    sb.Append(" and starteventtime >= @StartDate");
                }
                if (request.EndDate.HasValue)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    sb.Append("  and starteventtime <= @EndDate");
                }
                list = _dbContext.Page<PlayerOrRefereeEventResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventService.PlayerOrRefereeEvent", ex);
            }
            return list;
        }
    }
}
