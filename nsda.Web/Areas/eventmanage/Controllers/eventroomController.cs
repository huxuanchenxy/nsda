using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.Contract.eventmanage;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.eventmanage.Controllers
{
    public class eventroomController : baseController
    {
        IMemberService _memberService;
        IEventService _eventService;
        IEventRoomService _eventRoomService;
        public eventroomController(IMemberService memberService, IEventService eventService, IEventRoomService eventRoomService)
        {
            _memberService = memberService;
            _eventService = eventService;
            _eventRoomService = eventRoomService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(int eventId,int num)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag =_eventRoomService.Insert(eventId,num,out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(int id,string name)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRoomService.Eidt(id,name, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editsettings(int id, int status)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRoomService.EidtSettings(id, status, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult settingspec(int id,int memberId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRoomService.SettingSpec(id,memberId, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult clearspec(int id)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRoomService.ClearSpec(id, out msg);
            return Result<string>(flag, msg);
        }

        // 赛事列表
        [HttpGet]
        public ContentResult list(EventRoomQueryRequest request)
        {
            var data = _eventRoomService.List(request);
            var res = new ResultDto<EventRoomResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        #endregion 
    }
}