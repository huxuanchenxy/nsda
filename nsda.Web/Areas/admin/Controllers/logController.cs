using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.admin;
using nsda.Services.Contract.member;
using nsda.Utilities;
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
            var res = new ResultDto<MemberOperLogResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }


        [HttpGet]
        public ContentResult sysoperloglist(SysOperLogQueryRequest request)
        {
            var data = _sysOperLogService.List(request);
            var res = new ResultDto<SysOperLogResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }


        [HttpGet]
        public ContentResult emailloglist(EmailLogQueryRequest request)
        {
            var data = _emailLogService.List(request);
            var res = new ResultDto<EmailLogResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        #endregion
    }
}