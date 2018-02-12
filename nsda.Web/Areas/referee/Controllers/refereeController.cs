using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.admin;
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

namespace nsda.Web.Areas.referee.Controllers
{
    public class refereeController : refereebaseController
    {
        IMemberService _memberService;
        IRefereeService _refereeService;
        IRefereeSignUpService _refereeSignUpService;
        IMemberTempService _memberTempService;
        IEventService _eventService;
        IEventSignService _eventSignService;
        IMailService _mailService;
        public refereeController(IMailService mailService,IMemberService memberService, IRefereeService refereeService, IRefereeSignUpService refereeSignUpService, IMemberTempService memberTempService, IEventService eventService,IEventSignService eventSignService)
        {
            _memberService = memberService;
            _refereeService = refereeService;
            _refereeSignUpService = refereeSignUpService;
            _memberTempService = memberTempService;
            _eventService = eventService;
            _eventSignService = eventSignService;
            _mailService = mailService;
        }

        #region ajax
        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        public ContentResult edit(RegisterRefereeRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberReferee(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展选手
        [HttpPost]
        [AjaxOnly]
        public ContentResult extendplayer(RegisterPlayerRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendPlayer(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展教练
        [HttpPost]
        [AjaxOnly]
        public ContentResult extendcoach(RegisterCoachRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendCoach(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //当前比赛列表
        [HttpGet]
        public ContentResult current()
        {
            var data = _refereeSignUpService.CurrentRefereeEvent(UserContext.WebUserContext.Id);
            return Result(true, "", data);
        }

        //申请做裁判
        [HttpPost]
        [AjaxOnly]
        public ContentResult apply(int eventId)
        {
            string msg = string.Empty;
            var flag = _refereeSignUpService.Apply(eventId,UserContext.WebUserContext.Id,out msg);
            return Result<string>(flag, msg);
        }

        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        public ContentResult bindreferee(BindTempRefereeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _memberTempService.BindTempReferee(request, out msg);
            return Result<string>(flag, msg);
        }


        //签到
        [HttpPost]
        [AjaxOnly]
        public ContentResult sign(int id)
        {
            string msg = string.Empty;
            var flag = _eventSignService.Sign(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //报名列表
        [HttpGet]
        public ContentResult signuplist(RefereeSignUpQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _refereeSignUpService.RefereeSignUpList(request);
            var res = new ResultDto<RefereeSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        #endregion

        #region view
        public ActionResult index()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            ViewBag.Mail = _mailService.List(userContext.Id);
            ViewBag.CurrentRefereeEvent = _refereeSignUpService.CurrentRefereeEvent(userContext.Id);
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/referee/referee/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }

        public ActionResult qrcode(int id)
        {
            return View();
        }

        public ActionResult info()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            var data = _memberService.MemberRefereeDetail(UserContext.WebUserContext.Id);
            return View(data);
        }

        public ActionResult mail()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            return View();
        }

        //报名页面
        public ActionResult signup()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            //var data = _eventService.EventCondition();
            //ViewBag.Condition = data;
            return View();
        }

        //签到页面
        public ActionResult signview(int eventId)
        {
            var detail = _eventSignService.GetSign(eventId, UserContext.WebUserContext.Id,EventSignTypeEm.裁判);
            return View(detail);
        }

        public ActionResult list()
        {
            var userContext = UserContext.WebUserContext;
            ViewBag.UserContext = userContext;
            return View();
        }
        #endregion
    }
}