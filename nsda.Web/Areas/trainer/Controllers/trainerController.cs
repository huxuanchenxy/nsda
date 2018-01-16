using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services.Contract.trainer;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.trainer.Controllers
{
    public class trainerController : baseController
    {
        IPlayerTrainerService _playerTrainerService;
        IMemberService _memberService;
        ITrainerService _trainerService;
        public trainerController(IMemberService memberService, ITrainerService trainerService, IPlayerTrainerService playerTrainerService)
        {
            _memberService = memberService;
            _trainerService = trainerService;
            _playerTrainerService = playerTrainerService;
        }

        #region ajax
        //1 列表
        [HttpGet]
        public ContentResult listplayer(PlayerTrainerQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerTrainerService.TrainerList(request);
            var res = new ResultDto<TrainerPlayerResponse>
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
        public ContentResult insertplayer(PlayerTrainerRequest request)
        {
            request.IsPositive = false;
            request.IsTrainer = true;
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerTrainerService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }
        //3 编辑
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editplayer(PlayerTrainerRequest request)
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
        public ContentResult deleteplayer(int id)
        {
            var msg = string.Empty;
            var flag = _playerTrainerService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        //5 审核
        public ContentResult approplayer(int id, bool isAppro)
        {
            var msg = string.Empty;
            var flag = _playerTrainerService.IsAppro(id, isAppro, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }


        //模糊查询选手
        [HttpGet]
        public ContentResult listplayer(string keyvalue)
        {
            var data = _memberService.SelectPlayer(keyvalue,UserContext.WebUserContext.Id);
            return Result(true,string.Empty,data);
        }
        #endregion

        #region view
        public ActionResult index()
        {
            return View();
        }
        #endregion 
    }
}