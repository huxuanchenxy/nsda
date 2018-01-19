using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Services.Contract.eventmanage;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.eventmanage.Controllers
{
    /// <summary>
    /// 奖项设置
    /// </summary>
    public class eventprizeController : eventbaseController
    {
        IEventPrizeService _eventPrizeService;
        public eventprizeController(IEventPrizeService eventPrizeService)
        {
            _eventPrizeService = eventPrizeService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(EventPrizeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventPrizeService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(EventPrizeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventPrizeService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }
    

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventPrizeService.Delete(id,UserContext.WebUserContext.Id,out msg);
            return Result<string>(flag, msg);
        }

        // 赛事列表
        [HttpGet]
        public ContentResult list(int eventId,int eventGroupId)
        {
            var data = _eventPrizeService.List(eventId, eventGroupId);
            return Result(true, string.Empty, data);
        }
        #endregion 

    }
}