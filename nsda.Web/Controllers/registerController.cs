using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Services.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class registerController : Controller
    {
        IMemberService _memberService;
        public registerController(IMemberService memberService)
        {
            _memberService = memberService;
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

        #region ajax
        public JsonResult register(RegisterRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            res.flag = _memberService.Register(request, out msg);
            if (res.flag)
            {
                res.flag = true;
                if (request.MemberType == (int)MemberTypeEm.选手)
                {
                    res.msg = "/player/home/index";
                }
                else if (request.MemberType == (int)MemberTypeEm.教练)
                {
                    res.msg = "/trainer/home/index";
                }
                else if (request.MemberType == (int)MemberTypeEm.裁判)
                {
                    res.msg = "/referee/home/index";
                }
                else if (request.MemberType == (int)MemberTypeEm.赛事管理员)
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
        #endregion
    }
}