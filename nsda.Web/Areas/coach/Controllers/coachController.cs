using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.coach;
using nsda.Services.member;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.coach.Controllers
{
    public class coachController : coachbaseController
    {
        IPlayerCoachService _playerCoachService;
        IMemberService _memberService;
        ICoachService _coachService;
        public coachController(IMemberService memberService, ICoachService coachService, IPlayerCoachService playerCoachService)
        {
            _memberService = memberService;
            _coachService = coachService;
            _playerCoachService = playerCoachService;
        }

        #region ajax
        //修改个人信息
        [HttpPost]
        [AjaxOnly]
        public ContentResult edit(RegisterCoachRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.EditMemberCoach(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展选手
        [HttpPost]
        [AjaxOnly]
        public ContentResult extendplayer(RegisterPlayerRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendPlayer(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //扩展裁判
        [HttpPost]
        [AjaxOnly]
        public ContentResult extendreferee(RegisterRefereeRequest request)
        {
            string msg = string.Empty;
            var flag = _memberService.ExtendReferee(request, UserContext.WebUserContext, out msg);
            return Result<string>(flag, msg);
        }

        //1 列表
        [HttpGet]
        public ContentResult listplayer(PlayerCoachQueryRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var data = _playerCoachService.Coach_PlayerList(request);
            var res = new ResultDto<CoachPlayerResponse>
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
        public ContentResult insertplayer(PlayerCoachRequest request)
        {
            request.IsPositive = false;
            request.IsCoach = true;
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerCoachService.Insert(request, out msg);
            return Result<string>(flag, msg);
        }
        //3 编辑
        [HttpPost]
        [AjaxOnly]
        public ContentResult editplayer(PlayerCoachRequest request)
        {
            request.MemberId = UserContext.WebUserContext.Id;
            var msg = string.Empty;
            var flag = _playerCoachService.Edit(request, out msg);
            return Result<string>(flag, msg);
        }
        //4 删除
        [HttpPost]
        [AjaxOnly]
        public ContentResult deleteplayer(int id)
        {
            var msg = string.Empty;
            var flag = _playerCoachService.Delete(id, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }
        //5 审核
        public ContentResult checkplayer(int id, bool isAgree)
        {
            var msg = string.Empty;
            var flag = _playerCoachService.Check(id, isAgree, UserContext.WebUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }


        //模糊查询选手
        [HttpGet]
        public ContentResult listplayer(string key,string value)
        {
            var data = _memberService.SelectPlayer(key,value, UserContext.WebUserContext.Id);
            return Result(true,string.Empty,data);
        }
        #endregion

        #region view
        public ActionResult index()
        {
            //ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode($"/coach/coach/qrcode/{UserContext.WebUserContext.Id}");
            return View();
        }

        public ActionResult qrcode(int id)
        {
            return View();
        }

        //个人中心
        public ActionResult info()
        {
            var data = _memberService.MemberCoachDetail(UserContext.WebUserContext.Id);
            return View(data);
        }

        //站内信列表
        public ActionResult mail()
        {
            return View();
        }

        //教练学生列表
        public ActionResult list()
        {
            return View();
        }
        #endregion 
    }
}