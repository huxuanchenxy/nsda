using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Controllers
{
    public class EventmanageController : Controller
    {
        // GET: Eventmanage

        public ActionResult eventmanagelogin()
        {
            ViewBag.Message = "裁判登陆";
            return View();
        }
        public ActionResult eventmanageregform()
        {
            ViewBag.Message = "赛事管理员注册";
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

        public ActionResult eventmanageplayer()
        {
            ViewBag.Message = "选手报名管理";
            return View();
        }
        public ActionResult eventmanageumpire()
        {
            ViewBag.Message = "裁判报名管理";
            return View();
        }
        public ActionResult manageumpire()
        {
            ViewBag.Message = "裁判管理";
            return View();
        }
        public ActionResult addjudge()
        {
            ViewBag.Message = "添加临时裁判";
            return View();
        }
        public ActionResult eventmanageroom()
        {
            ViewBag.Message = "Room管理";
            return View();
        }
        public ActionResult addroom()
        {
            ViewBag.Message = "添加Room";
            return View();
        }
        public ActionResult addassign()
        {
            ViewBag.Message = "添加指定";
            return View();
        }
        public ActionResult modifyroom()
        {
            ViewBag.Message = "修改Room";
            return View();
        }
        public ActionResult eventmanageknockout()
        {
            ViewBag.Message = "淘汰赛规则设置";
            return View();
        }
        public ActionResult eventmanageround()
        {
            ViewBag.Message = "循环赛规则设置";
            return View();
        }

        public ActionResult eventnewinfoconfirm()
        {
            ViewBag.Message = "开始执行对垒";
            return View();
        }
        public ActionResult eventopeninfocomfirm()
        {
            ViewBag.Message = "开始执行对垒";
            return View();
        }
        public ActionResult eventnewplayersignin()
        {
            ViewBag.Message = "新手组选手签到";
            return View();
        }
        public ActionResult eventopenplayersignin()
        {
            ViewBag.Message = "公开组选手签到";
            return View();
        }
        public ActionResult eventumpiresignin()
        {
            ViewBag.Message = "裁判签到管理";
            return View();
        }
        public ActionResult eventjudgesignin()
        {
            ViewBag.Message = "裁判签到";
            return View();
        }


        public ActionResult eventmanagecenter()
        {
            ViewBag.Message = "赛事管理员个人中心";
            return View();
        }
        public ActionResult addtmpplayer()
        {
            ViewBag.Message = "添加临时选手";
            return View();
        }

        public ActionResult doublecheck()
        {
            ViewBag.Message = "核对评分单";
            return View();
        }
        public ActionResult writegrades()
        {
            ViewBag.Message = "录入评分单";
            return View();
        }
        public ActionResult track()
        {
            ViewBag.Message = "查询";
            return View();
        }
        public ActionResult resulttrackaround()
        {
            ViewBag.Message = "循环赛赛果查询";
            return View();
        }
        public ActionResult resulttrackkonckout()
        {
            ViewBag.Message = "淘汰赛赛果查询";
            return View();
        }
        public ActionResult winnerlist()
        {
            ViewBag.Message = "获奖名单";
            return View();
        }
        public ActionResult prize()
        {
            ViewBag.Message = "奖项设置";
            return View();
        }
        public ActionResult addprize()
        {
            ViewBag.Message = "添加自定义奖项";
            return View();
        }
        public ActionResult octafinals()
        {
            ViewBag.Message = "Octafinals 评分单录入";
            return View();
        }
        public ActionResult againstaround()
        {
            ViewBag.Message = "执行对垒 循环赛";
            return View();
        }
        public ActionResult againstknockout()
        {
            ViewBag.Message = "执行对垒 淘汰赛";
            return View();
        }
        public ActionResult mapone()
        {
            ViewBag.Message = "对垒";
            return View();
        }
        public ActionResult maptwo()
        {
            ViewBag.Message = "对垒";
            return View();
        }
        public ActionResult mapfour()
        {
            ViewBag.Message = "对垒";
            return View();
        }
        public ActionResult mapeight()
        {
            ViewBag.Message = "对垒";
            return View();
        }
        public ActionResult mapsixteen()
        {
            ViewBag.Message = "对垒";
            return View();
        }
        public ActionResult mapthirtytwo()
        {
            ViewBag.Message = "对垒";
            return View();
        }
    }
}