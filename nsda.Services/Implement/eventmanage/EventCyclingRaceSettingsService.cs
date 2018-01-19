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

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 循环赛设置
    /// </summary>
    public  class EventCyclingRaceSettingsService: IEventCyclingRaceSettingsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventCyclingRaceSettingsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //设置循环赛
        public bool Settints(EventCyclingRaceSettingsRequest request, out string msg)
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
                LogUtils.LogError("EventCyclingRaceSettingsService.Insert", ex);
            }
            return flag;
        }
        //循环赛详情
        public EventCyclingRaceSettingsResponse Detail(int eventId)
        {
            EventCyclingRaceSettingsResponse response = null;
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventCyclingRaceSettingsService.Detail", ex);
            }
            return response;
        }
    }
}
