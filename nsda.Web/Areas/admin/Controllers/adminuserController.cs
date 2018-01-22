using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services;
using nsda.Services.admin;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class adminuserController : adminbaseController
    {
        ISysUserService _sysUserService;
        public adminuserController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        #region view
        public ActionResult index()
        {
            return View();
        }

        public ActionResult add()
        {
            return View();
        }

        public ActionResult update(int id)
        {
            var detail = _sysUserService.Detail(id);
            if(detail==null)
                Response.Redirect("/error/error", true);
            return View(detail);
        }
        #endregion

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(SysUserRequest request)
        {
            var msg = string.Empty;
            var flag = _sysUserService.Insert(request,UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(SysUserRequest request)
        {
            var msg = string.Empty;
            var flag = _sysUserService.Edit(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _sysUserService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult reset(int id)
        {
            var msg = string.Empty;
            var flag = _sysUserService.Reset(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult isenable(int id,bool isEnable)
        {
            var msg = string.Empty;
            var flag = _sysUserService.IsEnable(id, isEnable,UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult list(SysUserQueryRequest request)
        {
            var data = _sysUserService.List(request);
            var res = new ResultDto<SysUserResponse>
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