using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
        public refereeController(IMemberService memberService, IRefereeService refereeService, IRefereeSignUpService refereeSignUpService, IMemberTempService memberTempService, IEventService eventService,IEventSignService eventSignService)
        {
            _memberService = memberService;
            _refereeService = refereeService;
            _refereeSignUpService = refereeSignUpService;
            _memberTempService = memberTempService;
            _eventService = eventService;
            _eventSignService = eventSignService;
        }

        #region ajax
        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(RegisterRefereeRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberReferee(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展选手
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult extendplayer(RegisterPlayerRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendPlayer(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public ContentResult apply(int eventId)
        {
            string msg = string.Empty;
            var flag = _refereeSignUpService.Apply(eventId,UserContext.WebUserContext.Id,out msg);
            return Result<string>(flag, msg);
        }

        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/referee/referee/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }

        public ActionResult qrcode(int id)
        {
            return View();
        }

        public ActionResult info()
        {
            var data = _memberService.MemberRefereeDetail(UserContext.WebUserContext.Id);
            return View(data);
        }

        public ActionResult mail()
        {
            return View();
        }

        //报名页面
        public ActionResult signup()
        {
            var data = _eventService.EventCondition();
            ViewBag.Condition = data;
            return View();
        }

        //签到页面
        public ActionResult signview(int eventId)
        {
            var detail = _eventSignService.GetSign(eventId, UserContext.WebUserContext.Id);
            return View(detail);
        }
        #endregion
    }
}