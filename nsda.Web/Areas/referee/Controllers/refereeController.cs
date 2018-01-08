using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Services.member;
using nsda.Services.trainer;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.referee.Controllers
{
    public class refereeController : baseController
    {
        IMemberService _memberService;
        IRefereeService _refereeService;
        public refereeController(IMemberService memberService, IRefereeService refereeService)
        {
            _memberService = memberService;
            _refereeService = refereeService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(MemberRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editpwd(string oldPwd, string newPwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberService.EditPwd(UserContext.WebUserContext.Id, oldPwd, newPwd, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view
        public ActionResult index()
        {
            return View();
        }
        #endregion 
    }
}