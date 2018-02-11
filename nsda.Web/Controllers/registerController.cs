using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Services.admin;
using nsda.Services.Contract.eventmanage;
using nsda.Services.member;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class registerController : Controller
    {
        IMemberService _memberService;
        IEmailLogService _emailLogService;
        IEventService _eventService;
        public registerController(IMemberService memberService, IEmailLogService emailLogService, IEventService eventService)
        {
            _memberService = memberService;
            _emailLogService = emailLogService;
            _eventService = eventService;
        }

        //查询账号是否存在
        [HttpGet]
        [AjaxOnly]
        public JsonResult isexist(string account)
        {
            var res = new Result<string>();
            res.flag = _memberService.IsExist(account);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //注册选手
        [HttpPost]
        [AjaxOnly]
        public JsonResult registerplayer(RegisterPlayerRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.RegisterMemberPlayer(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                res.msg = "/player/player/index";
            }
            else
            {
                res.msg = msg;
                res.flag = false;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //注册教练
        [HttpPost]
        [AjaxOnly]
        public JsonResult registercoach(RegisterCoachRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.RegisterMemberCoach(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                res.msg = "/coach/coach/index";
            }
            else
            {
                res.msg = msg;
                res.flag = false;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //注册裁判
        [HttpPost]
        [AjaxOnly]
        public JsonResult registerreferee(RegisterRefereeRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.RegisterMemberReferee(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                res.msg = "/referee/referee/index";
            }
            else
            {
                res.msg = msg;
                res.flag = false;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //注册赛事管理员
        [HttpPost]
        [AjaxOnly]
        public JsonResult registerevent(RegisterEventRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.RegisterMemberEvent(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                res.msg = "/eventmanage/eventmanage/index";
            }
            else
            {
                res.msg = msg;
                res.flag = false;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //赛事管理员
        public ActionResult eventmanage()
        {
            var data = _eventService.RefereeRegisterEvent();
            ViewBag.EventData = data;
            return View();
        }
        //裁判
        public ActionResult referee()
        {
            ViewBag.Condition = _eventService.RefereeRegisterEvent();
            return View();
        }
        //教练
        public ActionResult coach()
        {
            return View();
        }
        //选手
        public ActionResult player()
        {
            return View();
        }
    }
}