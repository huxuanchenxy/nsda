using nsda.Model.dto;
using nsda.Model.dto.request;
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
        public playersignupController(IPlayerSignUpService playerSignUpService)
        {
            _playerSignUpService = playerSignUpService;
        }

        #region ajax
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
        #endregion 
    }
}