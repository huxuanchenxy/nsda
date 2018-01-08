using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Services.admin;
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
        //登陆
        [HttpPost]
        [AjaxOnly]
        public JsonResult login(string account, string pwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            if (account.IsEmpty() || pwd.IsEmpty())
            {
                res.flag = false;
                res.msg = "账号或密码不能为空";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            DataTypeEm datatype = DataTypeEm.选手;
            var userContetxt =_memberService.Login(account, pwd, out msg);
            if (userContetxt != null)
            {
                res.flag = true;
                if (userContetxt.MemberType == (int)MemberTypeEm.选手)
                {
                    res.msg = "/player/home/index";
                    datatype = DataTypeEm.选手;
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.教练)
                {
                    res.msg = "/trainer/home/index";
                    datatype = DataTypeEm.教练;
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.裁判)
                {
                    res.msg = "/referee/home/index";
                    datatype = DataTypeEm.裁判;
                }
                else if (userContetxt.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    res.msg = "/eventmanage/home/index";
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
            Task.Factory.StartNew(() => {
                _loginLogService.Insert(new LoginLogRequest
                {
                    Account = account,
                    LoginResult = userContetxt != null ? "ok" : msg,
                    DataType = datatype
                });
            });
            return Json(res, JsonRequestBehavior.DenyGet);
        }

        //注册
        [HttpPost]
        [AjaxOnly]
        public JsonResult register(MemberRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.Register(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                if (request.MemberType == MemberTypeEm.选手)
                {
                    res.msg = "/player/home/index";
                }
                else if (request.MemberType == MemberTypeEm.教练)
                {
                    res.msg = "/trainer/home/index";
                }
                else if (request.MemberType == MemberTypeEm.裁判)
                {
                    res.msg = "/referee/home/index";
                }
                else if (request.MemberType == MemberTypeEm.赛事管理员)
                {
                    res.msg = "/eventmanage/home/index";
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
            return Json(res, JsonRequestBehavior.DenyGet);
        }

        //找回密码
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
                var send = new EmailUtility(Constant.EmailAccount,Constant.EmailPwd,Constant.email_smtp,Constant.email_port);
                Task.Factory.StartNew(()=> {
                    send.Send("nsda", email, "找回密码", $"您的验证码是{code}");
                    _emailLogService.Insert(new EmailLogRequest {
                         Account=email,
                         Content= $"您的验证码是{code}"
                      });
                    });
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
            if (string.Equals(Session[email], validateCode))
            {
                res.msg = "输入的验证码有误";
                return Json(res, JsonRequestBehavior.DenyGet);
            }
            res.flag = true;
            return Json(res, JsonRequestBehavior.DenyGet);
        }

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
            res.flag = _memberService.FindPwd(id, pwd, out msg);
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

        public ActionResult login()
        {
            var usercontext = UserContext.WebUserContext;
            if (usercontext != null)
            {
                if (usercontext.MemberType == (int)MemberTypeEm.选手)
                {
                    return RedirectToAction("index", "home", new { area = "player" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.教练)
                {
                    return RedirectToAction("index", "home", new { area = "trainer" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.裁判)
                {
                    return RedirectToAction("index", "home", new { area = "referee" });
                }
                else if (usercontext.MemberType == (int)MemberTypeEm.赛事管理员)
                {
                    return RedirectToAction("index", "home", new { area = "eventmanage" });
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View();
        }

        //赛事管理员
        public ActionResult eventmanage()
        {
            return View();
        }
        //裁判
        public ActionResult referee()
        {
            return View();
        }
        //教练
        public ActionResult trainer()
        {
            return View();
        }
        //选手
        public ActionResult player()
        {
            return View();
        }
        #endregion 
    }
}