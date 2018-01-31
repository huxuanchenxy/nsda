using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services;
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
    public class adminmemberController : adminbaseController
    {
        IMemberService _memberService;
        IMemberTempService _memberTempService;
        public adminmemberController(IMemberService memberService, IMemberTempService memberTempService)
        {
            _memberService = memberService;
            _memberTempService = memberTempService;
        }

        #region view
        public ActionResult index()
        {
            return View();
        }


        #endregion

        #region ajax
        //重置密码
        [HttpPost]
        [AjaxOnly]
        public ContentResult reset(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Reset(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //删除会员
        [HttpPost]
        [AjaxOnly]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //强制认证选手
        [HttpPost]
        [AjaxOnly]
        public ContentResult force(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Force(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //会员列表
        [HttpGet]
        public ContentResult listmember(MemberQueryRequest request)
        {
            var data = _memberService.List(request);
            var res = new ResultDto<MemberResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //临时选手会员
        [HttpGet]
        public ContentResult listtempplayer(TempMemberQueryRequest request)
        {
            var data = _memberTempService.ListPlayer(request);
            var res = new ResultDto<MemberTempResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //临时教练会员
        [HttpGet]
        public ContentResult listtempreferee(TempMemberQueryRequest request)
        {
            var data = _memberTempService.ListReferee(request);
            var res = new ResultDto<MemberTempResponse>
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