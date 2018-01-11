using nsda.Model.dto;
using nsda.Model.dto.request;
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
    public class eventmanageController : baseController
    {
        IMemberService _memberService;
        IEventService _eventService;
        IMemberTempService _memberTempService;
        IEventSignService _eventSignService;
        public eventmanageController(IMemberService memberService, IEventService eventService, IMemberTempService memberTempService,IEventSignService eventSignService)
        {
            _memberService = memberService;
            _eventService = eventService;
            _memberTempService = memberTempService;
            _eventSignService = eventSignService;
        }

        #region ajax
        //新增临时选手
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertplayer(List<TempPlayerRequest> request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempPlayer(request, out msg);
            return Result<string>(flag, msg);
        }

        //新增临时教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertreferee(TempRefereeRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempReferee(request, out msg);
            return Result<string>(flag, msg);
        }

        //批量签到
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult sign(List<int> id,int eventId,bool isNormal)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventSignService.BatchSign(id,eventId,isNormal,out msg);
            return Result<string>(flag, msg);
        }

        //选手签到列表
        [HttpGet]
        public ContentResult playersignlist(int eventId, string key)
        {
            _eventSignService.PlayerSignList(eventId,key);
            return Result<string>(true, string.Empty);
        }

        //裁判签到列表
        [HttpGet]
        public ContentResult refereesignlist(int eventId, string key)
        {
             _eventSignService.RefereeSignList(eventId, key);
            return Result<string>(true, string.Empty);
        }
        #endregion

        #region view
        public ActionResult index()
        {
            return View();
        }
        #endregion 
    }
}