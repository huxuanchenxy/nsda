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
        public bool Insert(EventRoomRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Num <= 0)
                {
                    msg = "教室数量有误";
                    return flag;
                }

                if (request.EventId <= 0)
                {
                    msg = "需要赛事信息";
                    return flag;
                }

                //教练赛事
                t_event t_event = _dbContext.Get<t_event>(request.EventId);
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    for (int i = 0; i < request.Num; i++)
                    {
                        _dbContext.Insert(new t_event_room
                        {
                            eventgroupId = request.EventGroupId,
                            code = _dataRepository.EventRoomRepo.RenderCode(request.EventId),
                            eventId = request.EventId,
                            roomStatus = RoomStatusEm.闲置
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
        public bool Edit(EventRoomRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_room room = _dbContext.Get<t_event_room>(request.Id);
                if (room != null)
                {
                    room.name = request.Name;
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                    flag = true;
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
                LogUtils.LogError("EventRoomService.Edit", ex);
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
                t_event_room room = _dbContext.Get<t_event_room>(id);
                if (room != null)
                {
                    if (statusOrGroup == 0)//停用
                    {
                        room.roomStatus = RoomStatusEm.停用;
                    }
                    else if (statusOrGroup == 1)//启用
                    {
                        room.roomStatus = RoomStatusEm.闲置;
                    }
                    else {//组别
                        room.eventgroupId = statusOrGroup;
                    }
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                    flag = true;
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
                t_event_room room = _dbContext.Get<t_event_room>(id);
                if (room != null)
                {
                    room.memberId = 0;
                    room.updatetime = DateTime.Now;
                    _dbContext.Update(room);
                    flag = true;
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
                t_event_room room = _dbContext.Get<t_event_room>(id);
                if (room != null)
                {
                    if (memberId == room.memberId)
                    {
                        msg = "已指定此选手";
                        return flag;
                    }
                    //判断此选手是否是这场比赛的
                    var validate = _dbContext.Select<t_event_player_signup>(c => c.eventId == room.eventId && c.signUpStatus == SignUpStatusEm.报名成功 && c.memberId == memberId).ToList();
                    if (validate == null || validate.Count == 0)
                    {
                        msg = "重新选择选手";
                        return flag;
                    }
                    //在判断此用户是否已经在其他教室
                    var vali = _dbContext.Select<t_event_room>(c => c.memberId == memberId&&c.eventId==room.eventId).ToList();
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
                var sql=$@"select a.*,b.completename MemberName,c.name EventGroupName 
                            from t_event_room a
                            left join t_member b on a.memberId=b.id
                            left join t_event_group c on a.eventgroupId=c.id
                            where a.eventId=@EventId and a.isdelete=0  order by a.createtime desc ";
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
