using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Controllers
{
    public class JudgeController : Controller
    {
        // GET: Judge
        public ActionResult judgelogin()
        {
            ViewBag.Message = "裁判登陆";
            return View();
        }
        public ActionResult judgeregform()
        {
            ViewBag.Message = "裁判注册";
            return View();
        }
        public ActionResult judgecenter()
        {
            ViewBag.Message = "裁判管理";
            return View();
        }
        public ActionResult judgemessagebox()
        {
            ViewBag.Message = "消息盒子";
            return View();
        }
       
        public ActionResult judgeapply()
        {
            ViewBag.Message = "申请裁判";
            return View();
        }
        public ActionResult judgeinformation()
        {
            ViewBag.Message = "修改个人资料";
            return View();
        }
        
        public ActionResult judgetour()
        {
            ViewBag.Message = "锦标赛";
            return View();
        }
        public ActionResult judgetoursteptwo()
        {
            ViewBag.Message = "第二步";
            return View();
        }
        public ActionResult judgetourstepthree()
        {
            ViewBag.Message = "第三步";
            return View();
        }
    }
}