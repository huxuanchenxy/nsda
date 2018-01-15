using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
        IMemberTempService _memberTempService;
        public memberController(IMemberService memberService, IMemberExtendService memberExtendService, IMemberTempService memberTempService)
        {
            _memberService = memberService;
            _memberExtendService = memberExtendService;
            _memberTempService = memberTempService;
        }

        #region view
        public ActionResult index()
        {
            return View();
        }


        #endregion

        #region ajax
        //会员扩展
        [HttpGet]
        public ContentResult listmemberextend(MemberExtendQueryRequest request)
        {
            var data = _memberExtendService.List(request);
            var res = new ResultDto<MemberExtendResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //处理会员扩展
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult processmemberextend(int id,string remark, bool isAppro)
        {
            var msg = string.Empty;
            var flag = _memberExtendService.Process(id, remark,isAppro, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }


        //重置密码
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult reset(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Reset(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //删除会员
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Delete(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //强制认证选手
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult force(int id)
        {
            var msg = string.Empty;
            var flag = _memberService.Force(id, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //审核赛事管理员账号
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult check(int id,string remark,bool isAppro)
        {
            var msg = string.Empty;
            var flag = _memberService.Check(id,remark,isAppro, UserContext.SysUserContext.Id, out msg);
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

        //临时会员
        [HttpGet]
        public ContentResult listtempmember(TempMemberQueryRequest request)
        {
            var data = _memberTempService.List(request);
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