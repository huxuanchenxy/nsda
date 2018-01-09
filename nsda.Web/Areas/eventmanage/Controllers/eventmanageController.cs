using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.eventmanage.Controllers
{
    public class eventmanageController : baseController
    {
        IMemberService _memberService;
        IEventService _eventService;
        IMemberTempService _memberTempService;
        public eventmanageController(IMemberService memberService, IEventService eventService, IMemberTempService memberTempService)
        {
            _memberService = memberService;
            _eventService = eventService;
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
   
        //新增临时选手
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertplayer(List<TempPlayerRequest> request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempPlayer(request, out msg);
            return Result<string>(flag, msg);
        }

        //新增临时教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertreferee(TempRefereeRequest request)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.InsertTempReferee(request, out msg);
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