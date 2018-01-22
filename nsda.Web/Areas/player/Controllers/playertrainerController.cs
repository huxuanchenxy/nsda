using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services;
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
    public class playertrainerController : playerbaseController
    {
        IPlayerTrainerService _playerTrainerService;
        public playertrainerController(IPlayerTrainerService playerTrainerService)
        {
            _playerTrainerService = playerTrainerService;
        }

        #region ajax
        //1 列表
        [HttpGet]
        public ContentResult list(PlayerTrainerQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerTrainerService.Player_TrainerList(request);
            var res = new ResultDto<PlayerTrainerResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }
        //2 新增
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult insert(PlayerTrainerRequest request)
        {
            request.IsTrainer = false;
            request.IsPositive = true;
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerTrainerService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }
        //3 编辑
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult edit(PlayerTrainerRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerTrainerService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }
        //4 删除
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _playerTrainerService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        //5 审核
        public ContentResult checktrainer(int id, bool isAgree)
        {
            var msg = string.Empty;
            var flag = _playerTrainerService.Check(id, isAgree, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view

        #endregion
    }
}