using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Services.admin;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class adminloginController : Controller
    {
        ISysUserService _sysUserService;
        ILoginLogService _loginLogService;
        public adminloginController(ISysUserService sysUserService, ILoginLogService loginLogService)
        {
            _sysUserService = sysUserService;
            _loginLogService = loginLogService;
        }

        public ActionResult logout()
        {
            UserContext.RemoveSysCookie();
            return RedirectToAction("login");
        }

        public ActionResult login()
        {
            if (UserContext.SysUserContext != null)
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
            if (account.IsEmpty() || pwd.IsEmpty())
            {
                res.flag = false;
                res.msg = "账号或密码不能为空";
                return Json(res, JsonRequestBehavior.DenyGet);
            }

            res.flag = _sysUserService.Login(account,pwd, out msg);
            res.msg = msg;
            Task.Factory.StartNew(() => {
                _loginLogService.Insert(new LoginLogRequest
                {
                    Account = account,
                    LoginResult = res.flag  ? "ok" : msg,
                    DataType = DataTypeEm.平台管理员
                });
            });
            return Json(res, JsonRequestBehavior.DenyGet);
        }
    }
}