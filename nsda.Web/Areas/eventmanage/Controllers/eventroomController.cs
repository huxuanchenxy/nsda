using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
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
    public class eventroomController : eventbaseController
    {
        IEventRoomService _eventRoomService;
        IPlayerSignUpService _playerSignUpService;
        public eventroomController(IEventRoomService eventRoomService, IPlayerSignUpService playerSignUpService)
        {
            _eventRoomService = eventRoomService;
            _playerSignUpService = playerSignUpService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        public ContentResult insert(EventRoomRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag =_eventRoomService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult edit(EventRoomRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _eventRoomService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult editsettings(int id, int status)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.EidtSettings(id, status, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult settingspec(int id,int memberId)
        {
            string msg = string.Empty;
            var flag = _eventRoomService.SettingSpec(id,memberId, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        public ContentResult clearspec(int id)
        {
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

        [HttpGet]
        public ContentResult selectplayer(int eventId,string keyvalue)
        {
            var data = _playerSignUpService.SelectPlayer(eventId,keyvalue);
            return Result(true, string.Empty, data);
        }
        #endregion 
    }
}