﻿using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.player.Controllers
{
    public class playersignupController : playerbaseController
    {
        IPlayerSignUpService _playerSignUpService;
        IEventService _eventService;
        IEventSignService _eventSignService;
        IMemberTempService _memberTempService;
        public playersignupController(IPlayerSignUpService playerSignUpService, IEventService eventService, IEventSignService eventSignService, IMemberTempService memberTempService)
        {
            _playerSignUpService = playerSignUpService;
            _eventService = eventService;
            _eventSignService = eventSignService;
            _memberTempService = memberTempService;
        }

        #region view
        //报名页面
        public ActionResult index()
        {
            ViewBag.Condition = _eventService.EventCondition();
            ViewBag.UserContext = UserContext.WebUserContext;
            return View();
        }

        //签到页面
        public ActionResult signview(int eventId)
        {
            ViewBag.UserContext = UserContext.WebUserContext;
            var detail = _eventSignService.GetSign(eventId, UserContext.WebUserContext.Id,EventSignTypeEm.选手);
            return View(detail);
        }

        public ActionResult bindtemp()
        {
            return View();
        }

        //已报名页
        public ActionResult list()
        {
            ViewBag.UserContext = UserContext.WebUserContext;
            return View();
        }
        //退赛退费页
        public ActionResult refund()
        {
            ViewBag.UserContext = UserContext.WebUserContext;
            return View();
        }

        //赛事详情页
        public ActionResult eventdetail()
        {
            ViewBag.UserContext = UserContext.WebUserContext;
            return View();
        }
        #endregion

        #region ajax
        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        public ContentResult bindplayer(BindTempPlayerRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            int orderId = _memberTempService.BindTempPlayer(request, out msg);
            return Result(orderId > 0, msg, orderId.ToString());
        }

        //邀请组队成员
        [HttpGet]
        public ContentResult invitation(string keyvalue,int eventId,int groupId)
        {
            var data = _playerSignUpService.Invitation(keyvalue, eventId, groupId, UserContext.WebUserContext.Id);
            return Result(true, "", data);
        }

        //比赛报名
        [HttpPost]
        [AjaxOnly]
        public ContentResult insert(PlayerSignUpRequest request)
        {
            request.FromMemberId = UserContext.WebUserContext.Id;
            string msg = string.Empty;
            var flag = _playerSignUpService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        //是否接受组队
        [HttpPost]
        [AjaxOnly]
        public ContentResult isacceptteam(int id, bool isAgree)
        {
            string msg = string.Empty;
            var flag = _playerSignUpService.IsAcceptTeam(id,isAgree, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //申请退赛
        [HttpPost]
        [AjaxOnly]
        public ContentResult applyretire(int id)
        {
            string msg = string.Empty;
            var flag = _playerSignUpService.ApplyRetire(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //确认退赛
        [HttpPost]
        [AjaxOnly]
        public ContentResult isconfirmretire(int id,bool isAgree)
        {
            string msg = string.Empty;
            var flag = _playerSignUpService.IsConfirmRetire(id, isAgree, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //去支付
        [HttpPost]
        [AjaxOnly]
        public ContentResult gopay(int id)
        {
            string msg = string.Empty;
            int orderId = _playerSignUpService.GoPay(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(orderId>0, msg, orderId.ToString());
        }

        //签到
        [HttpPost]
        [AjaxOnly]
        public ContentResult sign(int id)
        {
            string msg = string.Empty;
            var flag = _eventSignService.Sign(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //报名列表
        [HttpGet]
        public ContentResult signuplist(PlayerSignUpQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerSignUpService.PlayerSignUpList(request);
            var res = new ResultDto<PlayerSignUpListResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //退费列表
        [HttpGet]
        public ContentResult refundlist(PlayerSignUpQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerSignUpService.PlayerRefundList(request);
            var res = new ResultDto<PlayerRefundListResponse>
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