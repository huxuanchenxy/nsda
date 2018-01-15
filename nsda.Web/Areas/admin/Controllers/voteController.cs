using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class voteController : baseController
    {
        IVoteService _voteService;
        public voteController(IVoteService voteService)
        {
            _voteService = voteService;
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
            var detail = _voteService.Detail(id);
            if (detail == null)
                Response.Redirect("/error/error", true);
            return View();
        }
        #endregion

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _voteService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(VoteRequest request)
        {
            var msg = string.Empty;
            var flag = _voteService.Insert(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(VoteRequest request)
        {
            var msg = string.Empty;
            var flag = _voteService.Update(request, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult list(VoteQueryRequest request)
        {
            var data = _voteService.List(request);
            var res = new ResultDto<VoteResponse>
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