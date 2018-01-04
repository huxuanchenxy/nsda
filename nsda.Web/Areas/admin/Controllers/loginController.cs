using nsda.Model.dto;
using nsda.Services.admin;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class loginController : Controller
    {
        ISysUserService _sysUserService;
        public loginController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        public ActionResult logout()
        {
            UserContext.RemoveSysCookie();
            return RedirectToAction("login");
        }

        public ActionResult login()
        {
            if (UserContext.SysUserContext() != null)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }


        [HttpPost]
        public JsonResult login(string account,string pwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _sysUserService.Login(account,pwd, out msg);
            res.msg = msg;
            return Json(res, JsonRequestBehavior.DenyGet);
        }
    }
}