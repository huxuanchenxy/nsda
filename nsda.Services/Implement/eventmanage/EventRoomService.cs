using nsda.Model.dto.request;
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
using nsda.Model.enums;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 赛事教室管理
    /// </summary>
    public class EventRoomService: IEventRoomService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventRoomService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //新增n间教室
        public bool Insert(int eventId,int num,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (num <= 0)
                {
                    msg = "教室数量有误";
                    return flag;
                }

                if (eventId <= 0)
                {
                    msg = "需要赛事信息";
                    return flag;
                }

                //教练赛事
                t_event t_event = _dbContext.Get<t_event>(eventId);
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    for (int i = 0; i < num; i++)
                    {
                        _dbContext.Insert(new t_eventroom
                        {
                            eventgroupId = 0,
                            code = _dataRepository.EventRoomRepo.RenderCode(eventId),
                            eventId = eventId,
                            roomStatus = Model.enums.RoomStatusEm.闲置
                        });
                    }
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch(Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("EventRoomService.InsertTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.Insert", ex);
            }
            return flag;
        }
        //修改教室名称
        public bool Eidt(int id, string name, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventroom room = _dbContext.Get<t_eventroom>(id);
                if (room != null)
                {
                    room.name = name;
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                }
                else
                {
                    msg = "未找到教室信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.Eidt", ex);
            }
            return flag;
        }
        //修改设置
        public bool EidtSettings(int id,int statusOrGroup, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventroom room = _dbContext.Get<t_eventroom>(id);
                if (room != null)
                {
                    if (statusOrGroup == 0)//停用
                    {
                        room.roomStatus = RoomStatusEm.停用;
                        room.eventgroupId = 0;
                    }
                    else if (statusOrGroup == 1)//随机
                    {
                        room.roomStatus = RoomStatusEm.使用中;
                        room.eventgroupId = 0;
                    }
                    else {//组别
                        room.eventgroupId = statusOrGroup;
                        room.roomStatus = RoomStatusEm.使用中;
                    }
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                }
                else
                {
                    msg = "未找到教室信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.EidtSettings", ex);
            }
            return flag;
        }
        //清除教室特殊人员设定
        public bool ClearSpec(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventroom room = _dbContext.Get<t_eventroom>(id);
                if (room != null)
                {
                    room.memberId = 0;
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                }
                else {
                    msg = "未找到教室信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.Delete", ex);
            }
            return flag;
        }
        //教室指定选手
        public bool SettingSpec(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_eventroom room = _dbContext.Get<t_eventroom>(id);
                if (room != null)
                {
                    if (memberId == room.memberId)
                    {
                        msg = "已指定此选手";
                        return flag;
                    }
                    //判断此选手是否是这场比赛的
                    //在判断此用户是否已经在其他教室
                    var vali = _dbContext.Select<t_eventroom>(c => c.memberId == memberId&&c.eventId==room.eventId).ToList();
                    if (vali != null && vali.Count > 0)
                    {
                        msg = "此选手已指定到其他教室";
                    }
                    else
                    {
                        room.memberId = memberId;
                        room.updatetime = DateTime.Now;
                        _dbContext.Update(room);
                        flag = true;
                    }
                }
                else
                {
                    msg = "未找到教室信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRoomService.Delete", ex);
            }
            return flag;
        }
        // 教室列表
        public List<EventRoomResponse> List(EventRoomQueryRequest request)
        {
            List<EventRoomResponse> list = new List<EventRoomResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (a.code like @KeyValue or a.name like @KeyValue)");
                }
                var sql=$@"select a.*,b.name MemberName,c.name EventGroupName from t_eventroom a
                            left join t_member b on a.memberId=b.id
                            left join t_eventgroup c on a.eventgroupId=c.id
                            where eventId=@EventId and isdelete=0 {join.ToString()} order by a.createtime desc ";
                int totalCount = 0;
                list = _dbContext.Page<EventRoomResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventRoomService.List", ex);
            }
            return list;
        }
    }
}
