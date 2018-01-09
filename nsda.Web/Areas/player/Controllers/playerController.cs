﻿using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.member;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.player.Controllers
{
    public class playerController : baseController
    {
        IMemberService _memberService;
        IMemberExtendService _memberExtendService;
        IDataSourceService _dataSourceService;
        IEventScoreService _eventScoreService;
        IMemberTempService _memberTempService;
        public playerController(IMemberService memberService, IMemberExtendService memberExtendService,IDataSourceService dataSourceService, IEventScoreService eventScoreService, IMemberTempService memberTempService)
        {
            _memberService = memberService;
            _memberExtendService = memberExtendService;
            _dataSourceService = dataSourceService;
            _eventScoreService = eventScoreService;
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
        public ContentResult editpwd(string oldPwd,string newPwd)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberService.EditPwd(UserContext.WebUserContext.Id,oldPwd,newPwd, out msg);
            return Result<string>(flag, msg);
        }

        // 申请做裁判 或者教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult apply(MemberExtendRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberExtendService.Apply(request,out msg);
            return Result<string>(flag, msg);
        }

        //资料下载
        [HttpGet]
        public ContentResult datasource()
        {
            var data = _dataSourceService.List(new DataSourceQueryRequest { });
            return Result<string>(true, string.Empty);
        }
        
        //评分单下载
        [HttpGet]
        public ContentResult eventscorelist()
        {
            var data = _eventScoreService.PlayerList(new PlayerEventScoreQueryRequest { MemberId=UserContext.WebUserContext.Id});
            return Result<string>(true, string.Empty);
        }

        //模糊查询教练
        [HttpGet]
        public ContentResult listtrainer(string key)
        {
            var data = _memberService.ListPlayer(MemberTypeEm.教练,key);
            return Result<string>(true, string.Empty);
        }

        //去认证
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult gopay()
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberService.GoPay(UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult bindplayer(BindTempPlayerRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _memberTempService.BindTempPlayer(request, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view

        #endregion
    }
}