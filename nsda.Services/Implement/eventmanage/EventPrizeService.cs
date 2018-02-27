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
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 自定义奖项设置
    /// </summary>
    public class EventPrizeService : IEventPrizeService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventPrizeService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //新增奖项
        public bool Insert(EventPrizeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "奖项名称不能为空";
                    return flag;
                }
                if (request.Num.IsEmpty())
                {
                    msg = "获奖队伍/会员编号不能为空";
                    return flag;
                }
                if (request.PrizeType == Model.enums.PrizeTypeEm.个人)
                {
                    //查询会员编号是否正确
                }
                else {
                    //查询队伍编码是否正确
                }
                _dbContext.Insert(new t_event_prize
                {
                    eventGroupId = request.EventGroupId,
                    eventId = request.EventId,
                    name = request.Name,
                    num = request.Num,
                    prizeType = request.PrizeType,
                    remark = request.Remark
                });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventPrizeService.Insert", ex);
            }
            return flag;
        }
        //编辑奖项
        public bool Edit(EventPrizeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "奖项名称不能为空";
                    return flag;
                }
                if (request.Num.IsEmpty())
                {
                    msg = "获奖队伍/会员编号不能为空";
                    return flag;
                }
                if (request.PrizeType == Model.enums.PrizeTypeEm.个人)
                {
                    //查询会员编号是否正确
                }
                else
                {
                    //查询队伍编码是否正确
                }
                var eventPrize = _dbContext.Get<t_event_prize>(request.Id);
                if (eventPrize != null)
                {
                    eventPrize.remark = request.Remark;
                    eventPrize.name = request.Name;
                    eventPrize.prizeType = request.PrizeType;
                    eventPrize.num = request.Num;
                    eventPrize.updatetime = DateTime.Now;
                    _dbContext.Update(eventPrize);
                    flag = true;
                }
                else
                {
                    msg = "未找到奖项信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventPrizeService.Edit", ex);
            }
            return flag;
        }
        //删除奖项
        public bool Delete(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var eventPrize = _dbContext.Get<t_event_prize>(id);
                if (eventPrize != null)
                {
                    eventPrize.updatetime = DateTime.Now;
                    eventPrize.isdelete = true;
                    _dbContext.Update(eventPrize);
                    flag = true;
                }
                else
                {
                    msg = "未找到奖项信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventPrizeService.Delete", ex);
            }
            return flag;
        }
        //奖项列表
        public List<EventPrizeResponse> List(int eventId, int eventGroupId)
        {
            List<EventPrizeResponse> list = new List<EventPrizeResponse>();
            try
            {
                var sql = $"select * from t_event_prize where isdelete=0 and eventId={eventId} and eventGroupId={eventGroupId}";
                list = _dbContext.Query<EventPrizeResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventPrizeService.List", ex);
            }
            return list;
        }
        //奖项详情
        public EventPrizeResponse Detail(int id)
        {
            EventPrizeResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_event_prize>(id);
                if (detail != null)
                {
                    response = new EventPrizeResponse
                    {
                        EventGroupId = detail.eventGroupId,
                        EventId = detail.eventId,
                        Id = detail.id,
                        Name = detail.name,
                        Num = detail.num,
                        PrizeType = detail.prizeType,
                        Remark = detail.remark
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventPrizeService.Detail", ex);
            }
            return response;
        }
    }
}
