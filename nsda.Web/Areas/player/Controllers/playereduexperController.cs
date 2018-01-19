using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.Contract.admin;
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
    public class playereduexperController : playerbaseController
    {
        IPlayerEduExperService _playerEduExperService;
        ISchoolService _schoolService;
        public playereduexperController(PlayerEduExperService playerEduExperService, ISchoolService schoolService)
        {
            _playerEduExperService = playerEduExperService;
            _schoolService = schoolService;
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

        #region view
        #endregion
    }
}