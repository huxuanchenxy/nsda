using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult index()
        {
            return View();
        }

        public ActionResult about()
        {
            ViewBag.Message = "关于";

            return View();
        }
        public ActionResult aboutp()
        {
            ViewBag.Message = "合作伙伴";
            return View();
        }
        public ActionResult aboutus()
        {
            ViewBag.Message = "联系我们";
            return View();
        }

        public ActionResult registered()
        {
            ViewBag.Message = "会员登录";
            return View();
        }
        public ActionResult forgotpwd()
        {
            ViewBag.Message = "忘记密码";

            return View();
        }
        public ActionResult forgotpwdcert()
        {
            ViewBag.Message = "忘记密码-验证码";

            return View();
        }
        public ActionResult contact()
        {
            ViewBag.Message = "联系我们";

            return View();
        }
    }
}