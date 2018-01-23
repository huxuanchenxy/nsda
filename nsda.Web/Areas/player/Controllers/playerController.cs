using nsda.Model.dto;
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
        IMemberExtendService _memberExtendService;
        IDataSourceService _dataSourceService;
        IEventScoreService _eventScoreService;
        IMemberTempService _memberTempService;
        IMemberPointsService _memberPointsService;
        IMailService _mailService;
        public playerController(IMemberService memberService, IMemberExtendService memberExtendService,IDataSourceService dataSourceService, IEventScoreService eventScoreService, IMemberTempService memberTempService, IMemberPointsService memberPointsService,IMailService mailService)
        {
            _memberService = memberService;
            _memberExtendService = memberExtendService;
            _dataSourceService = dataSourceService;
            _eventScoreService = eventScoreService;
            _memberTempService = memberTempService;
            _memberPointsService = memberPointsService;
            _mailService = mailService;
        }

        #region ajax
        //辩题资料下载
        [HttpGet]
        public ContentResult datasource(DataSourceQueryRequest request)
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
        public ContentResult listtrainer(string key, string value)
        {
            var data = _memberService.SelectTrainer(key,value, UserContext.WebUserContext.Id);
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

        //绑定临时账号
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult bindplayer(BindTempPlayerRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            int orderId = _memberTempService.BindTempPlayer(request, out msg);
            return Result<string>(orderId>0, msg,orderId.ToString());
        }


        //积分记录查询
        [HttpGet]
        public ContentResult pointsrecord(PlayerPointsRecordQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            decimal totalPoints = 0m;
            var data = _memberPointsService.PlayerPointsRecord(request, out totalPoints);
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
        #endregion

        #region view

        #endregion
    }
}