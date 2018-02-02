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
    public class playercoachController : playerbaseController
    {
        IMemberService _memberService;
        IPlayerCoachService _playerCoachService;
        public playercoachController(IPlayerCoachService playerCoachService,IMemberService memberService)
        {
            _playerCoachService = playerCoachService;
            _memberService = memberService;
        }

        #region ajax
        //模糊查询教练
        [HttpGet]
        public ContentResult listcoach(string key, string value)
        {
            var data = _memberService.SelectCoach(key, value, UserContext.WebUserContext.Id);
            return Result(true, string.Empty, data);
        }

        //1 列表
        [HttpGet]
        public ContentResult list(PlayerCoachQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerCoachService.Player_CoachList(request);
            var res = new ResultDto<PlayerCoachResponse>
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
        public ContentResult insert(PlayerCoachRequest request)
        {
            request.IsCoach = false;
            request.IsPositive = true;
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerCoachService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }
        //3 编辑
        [HttpPost]
        [AjaxOnly]
        public ContentResult edit(PlayerCoachRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerCoachService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }
        //4 删除
        [HttpPost]
        [AjaxOnly]
        public ContentResult delete(int id)
        {
            var msg = string.Empty;
            var flag = _playerCoachService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        //5 审核
        public ContentResult checkcoach(int id, bool isAgree)
        {
            var msg = string.Empty;
            var flag = _playerCoachService.Check(id, isAgree, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        #endregion

        #region view
        //选手 教练页
        public ActionResult index()
        {
            ViewBag.UserContext = UserContext.WebUserContext;
            return View();
        }

        public ActionResult update(int id)
        {
            var detail = _playerCoachService.Detail(id);
            return View(detail);
        }

        public ActionResult add()
        {
            return View();
        }
        #endregion
    }
}