using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player

        public ActionResult playerregform()
        {
            ViewBag.Message = "会员注册";
            return View();
        }
        public ActionResult playermembercenter()
        {
            ViewBag.Message = "会员中心";
            return View();
        }
        public ActionResult playermessageBox()
        {
            ViewBag.Message = "消息盒子";
            return View();
        }

        public ActionResult playercompesign()
        {
            ViewBag.Message = "比赛报名";
            return View();
        }
        public ActionResult compedetail()
        {
            ViewBag.Message = "比赛详情";
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
        public ActionResult addedu()
        {
            ViewBag.Message = "添加教育经历";
            return View();
        }
        public ActionResult playerboundcoach()
        {
            ViewBag.Message = "绑定教练";
            return View();
        }
        public ActionResult boundcoach()
        {
            ViewBag.Message = "绑定教练";
            return View();
        }
        public ActionResult addtmpcompe()
        {
            ViewBag.Message = "添加临时参赛";
            return View();
        }
        public ActionResult playerscorelists()
        {
            ViewBag.Message = "评分单";

            return View();
        }
        public ActionResult playersource()
        {
            ViewBag.Message = "会员资源";
            return View();
        }
        public ActionResult playerscoredetail()
        {
            ViewBag.Message = "查看明细";
            return View();
        }
        public ActionResult playercomsignup()
        {
            ViewBag.Message = "比赛报名";
            return View();
        }
    }
}