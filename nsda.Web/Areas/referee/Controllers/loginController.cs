using nsda.Model.dto;
using nsda.Services.member;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.referee.Controllers
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
            if (UserContext.WebUserContext() != null)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }


        [HttpPost]
        public JsonResult login(string account, string pwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.Login(account, pwd, out msg);
            res.msg = msg;
            return Json(res, JsonRequestBehavior.DenyGet);
        }
    }
}