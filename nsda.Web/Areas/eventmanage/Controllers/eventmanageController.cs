using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Services.Contract.referee;
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
    public class eventmanageController : eventbaseController
    {
        IMemberService _memberService;
        IEventService _eventService;
        IMemberTempService _memberTempService;
        IEventSignService _eventSignService;
        IPlayerSignUpService _playerSignUpService;
        IRefereeSignUpService _refereeSignUpService;
        public eventmanageController(IMemberService memberService, IEventService eventService, IMemberTempService memberTempService,IEventSignService eventSignService, IPlayerSignUpService playerSignUpService,IRefereeSignUpService refereeSignUpService)
        {
            _memberService = memberService;
            _eventService = eventService;
            _memberTempService = memberTempService;
            _eventSignService = eventSignService;
            _playerSignUpService = playerSignUpService;
            _refereeSignUpService = refereeSignUpService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(RegisterEventRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberEvent(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //新增赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ContentResult insertevent(EventRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        //编辑赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ContentResult editevent(EventRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editeventgroup(EventGroupRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.EditGroup(request, out msg);
            return Result<string>(flag, msg);
        }

        //修改赛事状态
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editeventstatus(int id,EventStatusEm eventStatus)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.EditEventStatus(id, eventStatus, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //赛事列表
        [HttpGet]
        public ContentResult listevent(EventQueryRequest request)
        {
            var data = _eventService.EventList(request);
            var res = new ResultDto<EventResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //新增临时选手
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertplayer(List<TempPlayerRequest> request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempPlayer(request, UserContext.WebUserContext.Id, out msg);
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
            var flag = _memberTempService.InsertTempReferee(request, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //裁判审核
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult checkreferee(int id, bool isAgree)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _refereeSignUpService.Check(id, isAgree, UserContext.WebUserContext.Id, out msg);
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
        public ContentResult playersignlist(PlayerSignQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data=_eventSignService.PlayerSignList(request);
            var res = new ResultDto<PlayerSignResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //裁判签到列表
        [HttpGet]
        public ContentResult refereesignlist(RefereeSignQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data=_eventSignService.RefereeSignList(request);
            var res = new ResultDto<RefereeSignResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //选手报名列表
        [HttpGet]
        public ContentResult listplayersignup(EventPlayerSignUpQueryRequest request)
        {
            var data = _playerSignUpService.EventPlayerList(request);
            var res = new ResultDto<EventPlayerSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //裁判报名列表
        [HttpGet]
        public ContentResult listrefereesignup(EventRefereeSignUpQueryRequest request)
        {
            var data = _refereeSignUpService.EventRefereeList(request);
            var res = new ResultDto<EventRefereeSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //赛事组别信息
        [HttpGet]
        public ContentResult listeventgroup(int eventId)
        {
            var data = _eventService.SelectEventGroup(eventId,UserContext.WebUserContext.Id);
            return Result(true, string.Empty, data);
        }

        //生成签到信息
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult rendersign(int eventId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.RenderSign(eventId, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view
        public ActionResult index()
        {
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/eventmanage/eventmanage/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }

        public ActionResult qrcode(int id)
        {
            return View();
        }

        public ActionResult eventgroup(int eventGroupId)
        {
            var detail = _eventService.EventGroupDetail(eventGroupId);
            return View(detail);
        }

        public ActionResult info()
        {
            var data = _memberService.MemberEventDetail(UserContext.WebUserContext.Id);
            return View(data);
        }

        public ActionResult mail()
        {
            return View();
        }
        #endregion 
    }
}