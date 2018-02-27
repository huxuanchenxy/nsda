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
        public ActionResult playerscorelists()
        {
            ViewBag.Message = "评分单";

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
        public ActionResult playerregform()
        {
            ViewBag.Message = "会员注册";
            return View();
        }
        public ActionResult eventmanageregform()
        {
            ViewBag.Message = "赛事管理员注册";
            return View();
        }
        public ActionResult coachregform()
        {
            ViewBag.Message = "教练注册";
            return View();
        }
        public ActionResult judgeregform()
        {
            ViewBag.Message = "裁判注册";
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
        public ActionResult judgeinformation()
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
        public ActionResult judgemessagebox()
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
        public ActionResult boundcoach()
        {
            ViewBag.Message = "添加绑定教练";
            return View();
        }
        public ActionResult boundplayer()
        {
            ViewBag.Message = "添加绑定学员";
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
        public ActionResult judgecenter()
        {
            ViewBag.Message = "裁判管理";
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
        public ActionResult judgeapply()
        {
            ViewBag.Message = "申请裁判";
            return View();
        }
        public ActionResult judgelogin()
        {
            ViewBag.Message = "裁判登陆";
            return View();
        }
        public ActionResult eventmanagelogin()
        {
            ViewBag.Message = "裁判登陆";
            return View();
        }
        public ActionResult judgetour()
        {
            ViewBag.Message = "锦标赛";
            return View();
        }
        public ActionResult compedetail()
        {
            ViewBag.Message = "比赛详情";
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
        public ActionResult addtmpcompe()
        {
            ViewBag.Message = "添加临时参赛";
            return View();
        }
        public ActionResult playercomsignup()
        {
            ViewBag.Message = "比赛报名";
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
        public ActionResult oneround()
        {
            ViewBag.Message = "第一轮";
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
        public ActionResult againstknockout()
        {
            ViewBag.Message = "执行对垒 淘汰赛";
            return View();
        }
        public ActionResult roundone()
        {
            ViewBag.Message = "第一轮";
            return View();
        }
    }
}