using nsda.Model.dto.request;
using nsda.Services.Contract.member;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class memberController : baseController
    {
        IMemberService _memberService;
        IMemberExtendService _memberExtendService;
        public memberController(IMemberService memberService, IMemberExtendService memberExtendService)
        {
            _memberService = memberService;
            _memberExtendService = memberExtendService;
        }

        #region view
        public ActionResult index()
        {
            return View();
        }


        #endregion

        #region ajax
        [HttpGet]
        public ContentResult listmemberextend(MemberExtendQueryRequest request)
        {
            var data = _memberExtendService.List(request);
            return Result<string>(true, string.Empty);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult processmemberextend(int id,string remark, bool isAppro)
        {
            var msg = string.Empty;
            var flag = _memberExtendService.Process(id, UserContext.SysUserContext.Id, remark,isAppro, out msg);
            return Result<string>(flag, msg);
        }

        #endregion
    }
}