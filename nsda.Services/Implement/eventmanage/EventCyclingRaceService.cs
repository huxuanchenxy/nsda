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
    /// 循环赛管理
    /// </summary>
    public class EventCyclingRaceService: IEventCyclingRaceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventCyclingRaceService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //开始循环赛  生成循环赛对垒表
        public bool Start(int eventId,int eventGroupId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var cyclingracesettings = _dbContext.Select<t_event_cycling_settings>(c => c.eventGroupId == eventGroupId && c.eventId == eventId).FirstOrDefault();
                var cyclingrace = _dbContext.Select<t_event_cycling>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.currentround == 1).FirstOrDefault();
                var cyclingracedetail = _dbContext.Select<t_event_cycling_detail>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.cyclingraceId == cyclingrace.id).FirstOrDefault();

                //获取报名队伍信息
                //获取裁判信息
                //教室信息
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
    }
}
