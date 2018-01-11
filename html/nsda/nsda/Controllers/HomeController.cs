using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Aboutp()
        {
            ViewBag.Message = "合作伙伴";
            return View();
        }
        public ActionResult Aboutus()
        {
            ViewBag.Message = "联系我们";
            return View();
        }

        public ActionResult Registered()
        {
            ViewBag.Message = "会员登录";
            return View();
        }
        public ActionResult Regform()
        {
            ViewBag.Message = "会员注册";
            return View();
        }
    }
}