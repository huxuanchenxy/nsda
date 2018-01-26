﻿using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
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
    public class playerController : playerbaseController
    {
        IMemberService _memberService;
        IDataSourceService _dataSourceService;
        IEventScoreService _eventScoreService;
        IMemberPointsService _memberPointsService;
        IPlayerSignUpService _playerSignUpService;
        IPlayerEduService _playerEduService;
        IPlayerCoachService _playerCoachService;
        public playerController(IMemberService memberService,IDataSourceService dataSourceService, IEventScoreService eventScoreService, IMemberTempService memberTempService, IMemberPointsService memberPointsService, IPlayerSignUpService playerSignUpService,IPlayerEduService playerEduService, IPlayerCoachService playerCoachService)
        {
            _memberService = memberService;
            _dataSourceService = dataSourceService;
            _eventScoreService = eventScoreService;
            _memberPointsService = memberPointsService;
            _playerSignUpService = playerSignUpService;
            _playerEduService = playerEduService;
            _playerCoachService = playerCoachService;
        }

        #region ajax
        //轮询账号 看是否已认证
        [HttpGet]
        public ContentResult polling()
        {
            var data = _memberService.MemberPlayerPolling(UserContext.WebUserContext);
            return Result(true, "", data);
        }
        
        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(RegisterPlayerRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberPlayer(request,UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展教练
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult extendcoach(RegisterCoachRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendCoach(request,UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);         
        }

        //扩展裁判
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult extendreferee(RegisterRefereeRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendReferee(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //当前比赛列表
        [HttpGet]
        public ContentResult current()
        {
            var data = _playerSignUpService.CurrentPlayerEvent(UserContext.WebUserContext.Id);
            return Result(true, "", data);
        }

        //辩题资料下载
        [HttpGet]
        public ContentResult datasourcelist(DataSourceQueryRequest request)
        {
            var data = _dataSourceService.List(request);
            var res = new ResultDto<DataSourceResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        
        //评分单下载
        [HttpGet]
        public ContentResult eventscorelist(PlayerEventScoreQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _eventScoreService.PlayerList(request);
            var res = new ResultDto<EventScoreResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //模糊查询教练
        [HttpGet]
        public ContentResult listcoach(string key, string value)
        {
            var data = _memberService.SelectCoach(key,value, UserContext.WebUserContext.Id);
            return Result(true, string.Empty,data);
        }

        //去认证
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult goauth()
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var orderId = _memberService.GoAuth(UserContext.WebUserContext.Id, out msg);
            return Result<string>(orderId>0, msg, orderId.ToString());
        }

        //积分记录查询
        [HttpGet]
        public ContentResult pointsrecord(PlayerPointsRecordQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _memberPointsService.PlayerPointsRecord(request);
            var res = new ResultDto<PlayerPointsRecordResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        //积分记录详情
        [HttpGet]
        public ContentResult pointsrecorddetail(int recordId)
        {
            var data = _memberPointsService.PointsRecordDetail(recordId, UserContext.WebUserContext.Id);
            return Result(true, string.Empty,data);
        }

        #region 教育经历
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult deleteedu(int id)
        {
            var msg = string.Empty;
            var flag = _playerEduService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertedu(PlayerEduRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editedu(PlayerEduRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listedu(PlayerEduExperQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerEduService.List(request);
            var res = new ResultDto<PlayerEduResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        #endregion 

        #endregion

        #region view
        public ActionResult index()
        {
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/player/player/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }

        //处理所生成的二维码扫描回调事件
        public ActionResult qrcode(int id)
        {
            return View();
        }

        public ActionResult info()
        {
            var userContext = UserContext.WebUserContext;
            var data = _memberService.MemberPlayerDetail(userContext.Id);
            ViewBag.CoachInfo = _playerCoachService.Player_CoachDetail(userContext.Id);
            return View(data);
        }

        //资料页面
        public ActionResult datasource()
        {
            return View();
        }

        //评分单页面
        public ActionResult eventscore()
        {
            return View();
        }

        //站内信页面
        public ActionResult mail()
        {
            return View();
        }
        #endregion
    }
}