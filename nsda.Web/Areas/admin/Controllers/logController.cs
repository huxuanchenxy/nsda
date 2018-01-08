using nsda.Model.dto.request;
using nsda.Services.admin;
using nsda.Services.Contract.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    //日志管理
    public class logController : baseController
    {
        IMemberOperLogService _memberOperLogService;
        ISysOperLogService _sysOperLogService;
        IEmailLogService _emailLogService;
        public logController(ISysOperLogService sysOperLogService, IMemberOperLogService memberOperLogService, IEmailLogService emailLogService)
        {
            _sysOperLogService = sysOperLogService;
            _memberOperLogService = memberOperLogService;
            _emailLogService = emailLogService;
        }

        #region view
        public ActionResult memberoperlog()
        {
            return View();
        }

        public ActionResult sysoperloglist()
        {
            return View();
        }

        public ActionResult emailloglist()
        {
            return View();
        }
        #endregion

        #region ajax
        [HttpGet]
        public ContentResult memberoperloglist(MemberOperLogQueryRequest request)
        {
            var data = _memberOperLogService.List(request);
            return Result<string>(true, string.Empty);
        }


        [HttpGet]
        public ContentResult sysoperloglist(SysOperLogQueryRequest request)
        {
            var data = _sysOperLogService.List(request);
            return Result<string>(true, string.Empty);
        }


        [HttpGet]
        public ContentResult emailloglist(EmailLogQueryRequest request)
        {
            var data = _emailLogService.List(request);
            return Result<string>(true, string.Empty);
        }
        #endregion
    }
}