using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult index()
        {
            return View();
        }

        public ActionResult about()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult contact()
        {
            ViewBag.Message = "Your contact page.";

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
        public ActionResult regform()
        {
            ViewBag.Message = "会员注册";
            return View();
        }

        public ActionResult playermembercenter()
        {
            ViewBag.Message = "会员中心";
            return View();
        }

        public ActionResult playercompesign()
        {
            ViewBag.Message = "比赛报名";
            return View();
        }
        public ActionResult coachboundplayer()
        {
            ViewBag.Message = "比赛报名";
            return View();
        }
        public ActionResult playerhadcompe()
        {
            ViewBag.Message = "已参加比赛";
            return View();
        }
        public ActionResult playerexitcompe()
        {
            ViewBag.Message = "退赛信息";
            return View();
        }
        public ActionResult playerscore()
        {
            ViewBag.Message = "会员积分查询";
            return View();
        }
        public ActionResult playerinformation()
        {
            ViewBag.Message = "修改个人资料";
            return View();
        }
        public ActionResult playerboundcoach()
        {
            ViewBag.Message = "绑定教练";
            return View();
        }
        public ActionResult playersource()
        {
            ViewBag.Message = "会员资源";
            return View();
        }
        public ActionResult playermessageBox()
        {
            ViewBag.Message = "消息盒子";
            return View();
        }
        public ActionResult coachinformation()
        {
            ViewBag.Message = "Coach Personal Information";
            return View();
        }
        public ActionResult eventmanage()
        {
            ViewBag.Message = "比赛进程管理";
            return View();
        }
        public ActionResult eventstart()
        {
            ViewBag.Message = "发起赛事";
            return View();
        }
        public ActionResult eventstartlists()
        {
            ViewBag.Message = "发起赛事";
            return View();
        }
        public ActionResult addedu()
        {
            ViewBag.Message = "添加教育经历";
            return View();
        }
    }
}