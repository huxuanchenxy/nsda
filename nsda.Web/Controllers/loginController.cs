using nsda.Model.dto;
using nsda.Model.enums;
using nsda.Services.member;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class loginController : Controller
    {
        IMemberService _memberService;
        public loginController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult logout()
        {
            UserContext.RemoveWebCookie();
            return RedirectToAction("login");
        }

        public ActionResult login()
        {
            var usercontext = UserContext.WebUserContext();
            if (usercontext != null)
            {
                if (usercontext.MemberType == (int)MemberTypeEm.选手)
                {
                    return RedirectToAction("index", "home", new { area = "player" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.教练)
                {
                    return RedirectToAction("index", "home", new { area = "trainer" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.裁判)
                {
                    return RedirectToAction("index", "home", new { area = "referee" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    return RedirectToAction("index", "home", new { area = "eventmanage" });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult login(string account, string pwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var userContetxt=_memberService.Login(account, pwd, out msg);
            if (userContetxt != null)
            {
                res.flag = true;
                if (userContetxt.MemberType == (int)MemberTypeEm.选手)
                {
                    res.msg = "/player/home/index";
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.教练)
                {
                    res.msg = "/trainer/home/index";
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.裁判)
                {
                    res.msg = "/referee/home/index";
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    res.msg = "/eventmanage/home/index";
                }
                else
                {
                    res.msg = "/home/index";
                }
            }
            else {
                res.msg = msg;
                res.flag = false;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
    }
}