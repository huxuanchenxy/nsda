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
        IMemberPointsService _memberPointsService;
        IPlayerSignUpService _playerSignUpService;
        IPlayerEduExperService _playerEduExperService;

        public playerController(IMemberService memberService, IMemberExtendService memberExtendService,IDataSourceService dataSourceService, IEventScoreService eventScoreService, IMemberTempService memberTempService, IMemberPointsService memberPointsService, IPlayerSignUpService playerSignUpService,IPlayerEduExperService playerEduExperService)
        {
            _memberService = memberService;
            _memberExtendService = memberExtendService;
            _dataSourceService = dataSourceService;
            _eventScoreService = eventScoreService;
            _memberPointsService = memberPointsService;
            _playerSignUpService = playerSignUpService;
            _playerEduExperService = playerEduExperService;
        }

        #region ajax
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
            var flag = _playerEduExperService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insertedu(PlayerEduExperRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduExperService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editedu(PlayerEduExperRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduExperService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpGet]
        public ContentResult listedu(PlayerEduExperQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerEduExperService.List(request);
            var res = new ResultDto<PlayerEduExperResponse>
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
            var data = _memberService.Detail(UserContext.WebUserContext.Id);
            ViewBag.CoachInfo = null;
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