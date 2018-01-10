using nsda.Model.dto;
using nsda.Model.dto.request;
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
    public class playersignupController : baseController
    {
        IPlayerSignUpService _playerSignUpService;
        IEventService _eventService;
        public playersignupController(IPlayerSignUpService playerSignUpService, IEventService eventService)
        {
            _playerSignUpService = playerSignUpService;
            _eventService = eventService;
        }

        #region ajax
        // 赛事列表
        [HttpGet]
        public ContentResult listevent(PlayerOrRefereeEventQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            request.IsReferee = false;
            var data = _eventService.PlayerOrRefereeEvent(request);
            return Result<string>(true, string.Empty);
        }

        //比赛报名
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(PlayerSignUpRequest request)
        {
            request.FromMemberId = UserContext.WebUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        //是否接受组队
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult isacceptteam(int id, bool isAgree)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.IsAcceptTeam(id, UserContext.WebUserContext.Id,isAgree, out msg);
            return Result<string>(flag, msg);
        }

        //替换队友
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult replaceteammate(int id, int newMemberId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.ReplaceTeammate(id, UserContext.WebUserContext.Id, newMemberId, out msg);
            return Result<string>(flag, msg);
        }

        //去支付
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult gopay(int id, int newMemberId)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _playerSignUpService.GoPay(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion 
    }
}