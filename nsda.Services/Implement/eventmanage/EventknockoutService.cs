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
    /// 淘汰赛管理
    /// </summary>
    public class EventknockoutService: IEventknockoutService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventknockoutService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //开始淘汰赛  生成淘汰赛对垒表
        public bool Start(int eventId, int eventGroupId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var knockoutsettings = _dbContext.Select<t_event_knockout_settings>(c => c.eventGroupId == eventGroupId && c.eventId == eventId).FirstOrDefault();
                var knockout = _dbContext.Select<t_event_knockout>(c => c.eventGroupId == eventGroupId && c.eventId == eventId).FirstOrDefault();
                var knockoutdetail = _dbContext.Select<t_event_knockout_detail>(c => c.eventGroupId == eventGroupId && c.eventId == eventId && c.knockoutId == knockout.id).FirstOrDefault();
                //获取签到 以前排名前多少位的队伍
                //获取裁判信息
                //教室信息
                //排对垒
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventknockoutService.Start", ex);
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

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventknockoutService.Next", ex);
            }
            return flag;
        }
    }
}
