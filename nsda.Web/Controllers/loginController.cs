using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.admin;
using nsda.Services.Contract.eventmanage;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class loginController : Controller
    {
        IMemberService _memberService;
        ILoginLogService _loginLogService;
        IEmailLogService _emailLogService;
        public loginController(IMemberService memberService, ILoginLogService loginLogService, IEmailLogService emailLogService)
        {
            _memberService = memberService;
            _loginLogService = loginLogService;
            _emailLogService = emailLogService;
        }

        #region ajax
        //登录
        [HttpPost]
        [AjaxOnly]
        public JsonResult login(LoginRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            if (request.Account.IsEmpty() || request.Pwd.IsEmpty())
            {
                res.flag = false;
                res.msg = "账号或密码不能为空";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            DataTypeEm datatype = DataTypeEm.选手;
            var userContetxt =_memberService.Login(request, out msg);
            if (userContetxt != null)
            {
                res.flag = true;
                if (request.MemberType == MemberTypeEm.选手)
                {
                    //跳转到选手报名页面
                    res.msg = request.RedirectUrl.IsNotEmpty()? request.RedirectUrl: "/player/player/index";
                    datatype = DataTypeEm.选手;
                }
                else if (request.MemberType == MemberTypeEm.教练)
                {
                    res.msg = "/coach/coach/index";
                    datatype = DataTypeEm.教练;
                }
                else if (request.MemberType == MemberTypeEm.裁判)
                {
                    //跳转到裁判报名页面
                    res.msg = request.RedirectUrl.IsNotEmpty() ? request.RedirectUrl : "/referee/referee/index";
                    datatype = DataTypeEm.裁判;
                }
                else if (request.MemberType == MemberTypeEm.赛事管理员)
                {
                    res.msg = "/eventmanage/eventmanage/index";
                    datatype = DataTypeEm.赛事管理员;
                }
                else
                {
                    res.msg = "/home/index";
                }
            }
            else
            {
                res.msg = msg;
                res.flag = false;
            }
            //Task.Factory.StartNew(() => {
            //    _loginLogService.Insert(new LoginLogRequest
            //    {
            //        Account = request.Account,
            //        LoginResult = userContetxt != null ? "ok" : msg,
            //        DataType = datatype
            //    });
            //});
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //发送邮件
        [HttpPost]
        [AjaxOnly]
        public JsonResult sendemail(string email)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            //查看邮箱是否存在
            int id = _memberService.SendEmail(email, out msg);
            if (id > 0)
            {
                Session[Constant.FindPwd] = id;
                //生成验证码
                string code = VerifyCode.GetVerifyCode(6);
                Session[email] = code;
                //发送邮件
                //var send = new EmailUtility(Constant.EmailAccount, Constant.EmailPwd, Constant.Email_smtp, Constant.Email_port);
                //Task.Factory.StartNew(() =>
                //{
                //    send.Send("nsda", email, "找回密码", $"您的验证码是{code}");
                //    _emailLogService.Insert(new EmailLogRequest
                //    {
                //        Account = email,
                //        Content = $"您的验证码是{code}"
                //    });
                //});
                res.flag = true;
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            else
            {
                res.flag = false;
                res.msg = msg;
                return Json(res, JsonRequestBehavior.DenyGet);
            }
        }
        // 校验邮箱验证码
        [HttpPost]
        [AjaxOnly]
        public JsonResult validate(string email,string validateCode)
        {
            var res = new Result<string>();
            res.flag = false;
            if (email.IsEmpty())
            {
                res.msg = "邮箱不能为空";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            if (validateCode.IsEmpty())
            {
                res.msg = "验证码不能为空";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            if (Session[email] == null)
            {
                res.msg = "验证码失效请重新获取";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            if (!string.Equals(Session[email], validateCode))
            {
                res.msg = "输入的验证码有误";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            res.flag = true;
            Session[email] = null;
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //找回密码
        [HttpPost]
        [AjaxOnly]
        public JsonResult findpwd(string pwd)
        {
            int id = 0;
            if (Session[Constant.FindPwd] != null)
            {
                id = Session[Constant.FindPwd].ToObjInt();
            }
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.EditPwd(id, pwd, out msg);
            if (res.flag)
            {
                Session[Constant.FindPwd] = null;
            }
            return Json(res, JsonRequestBehavior.DenyGet);
        }
        //生成验证码
        [HttpGet]
        public ActionResult verifycode(int id)
        {
            string key = "findpwd";
            switch (id)
            {
                case 0:
                    key = "login";//登录
                    break;
                case 1:
                    key = "findpwd";//找回密码
                    break;
                case 2:
                    key = "validatecode";//注册
                    break;
                default:
                    key = "findpwd";//找回密码
                    break;
            }
            return File(new VerifyCode().GetVerifyCode(key), @"image/Gif");
        }
        #endregion

        #region view
        public ActionResult logout()
        {
            UserContext.RemoveWebUserInfo();
            return RedirectToAction("login");
        }

        //选手 教练登录页
        public ActionResult login()
        {
            var usercontext = UserContext.WebUserContext;
            if (usercontext != null)
            {
                if (usercontext.MemberType == (int)MemberTypeEm.选手)
                {
                    return RedirectToAction("index", "player", new { area = "player" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.教练)
                {
                    return RedirectToAction("index", "coach", new { area = "coach" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.裁判)
                {
                    return RedirectToAction("index", "referee", new { area = "referee" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    return RedirectToAction("index", "eventmanage", new { area = "eventmanage" });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View();
        }

        //裁判登录页面
        public ActionResult referee()
        {
            var usercontext = UserContext.WebUserContext;
            if (usercontext != null)
            {
                if (usercontext.MemberType == (int)MemberTypeEm.选手)
                {
                    return RedirectToAction("index", "player", new { area = "player" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.教练)
                {
                    return RedirectToAction("index", "coach", new { area = "coach" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.裁判)
                {
                    return RedirectToAction("index", "referee", new { area = "referee" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    return RedirectToAction("index", "eventmanage", new { area = "eventmanage" });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View();
        }

        //赛事管理员登录页面
        public ActionResult eventmanage()
        {
            var usercontext = UserContext.WebUserContext;
            if (usercontext != null)
            {
                if (usercontext.MemberType == (int)MemberTypeEm.选手)
                {
                    return RedirectToAction("index", "player", new { area = "player" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.教练)
                {
                    return RedirectToAction("index", "coach", new { area = "coach" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.裁判)
                {
                    return RedirectToAction("index", "referee", new { area = "referee" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    return RedirectToAction("index", "eventmanage", new { area = "eventmanage" });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View();
        }

        //找回密码
        public ActionResult findpwd(int id)
        {
            ViewBag.FindType = id;
            return View();
        }

        //修改密码
        public ActionResult findpwdcert(string email,int findType=1)
        {
            return View();
        }
        #endregion
    }
}