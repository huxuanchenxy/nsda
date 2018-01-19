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
    /// 淘汰赛设置
    /// </summary>
    public class EventknockoutSettingsService:IEventknockoutSettingsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventknockoutSettingsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //淘汰赛设置
        public bool Settints(EventknockoutSettingsRequest request, out string msg)
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
                LogUtils.LogError("EventknockoutSettingsService.Insert", ex);
            }
            return flag;
        }
        //淘汰赛设置详情
        public EventknockoutSettingsResponse Detail(int eventId)
        {
            EventknockoutSettingsResponse response = null;
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventknockoutSettingsService.Detail", ex);
            }
            return response;
        }
    }
}
