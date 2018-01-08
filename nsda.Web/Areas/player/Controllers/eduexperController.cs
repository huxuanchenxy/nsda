using nsda.Model.dto.request;
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
    public class eduexperController : baseController
    {
        IPlayerEduExperService _playerEduExperService;
        public eduexperController(PlayerEduExperService playerEduExperService)
        {
            _playerEduExperService = playerEduExperService;
        }

        #region ajax
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _playerEduExperService.Delete(id,UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(PlayerEduExperRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduExperService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(PlayerEduExperRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerEduExperService.Edit(request, out msg);
            return Result<string>(flag,msg);
        }


        [HttpGet]
        public ContentResult list(PlayerEduExperQueryRequest request)
        {
            var data = _playerEduExperService.List(request);
            return Result<string>(true, string.Empty);
        }
        #endregion

        #region view
        #endregion
    }
}