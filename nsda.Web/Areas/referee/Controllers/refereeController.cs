using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Services.Contract.member;
using nsda.Services.Contract.referee;
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
        IRefereeSignUpService _refereeSignUpService;
        IMemberTempService _memberTempService;
        public refereeController(IMemberService memberService, IRefereeService refereeService, IRefereeSignUpService refereeSignUpService, IMemberTempService memberTempService)
        {
            _memberService = memberService;
            _refereeService = refereeService;
            _refereeSignUpService = refereeSignUpService;
            _memberTempService = memberTempService;
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


        //申请做裁判
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult apply(int eventId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _refereeSignUpService.Apply(eventId,UserContext.WebUserContext.Id,out msg);
            return Result<string>(flag, msg);
        }

        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult bindreferee(BindTempRefereeRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.BindTempReferee(request, out msg);
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