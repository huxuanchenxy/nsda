using nsda.Model.dto;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services.Contract.eventmanage;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    //赛事管理
    public class admineventmgController : adminbaseController
    {
        IEventService _eventService;
        IEventRuleService _eventRuleService;
        public admineventmgController(IEventService eventService, IEventRuleService eventRuleService)
        {
            _eventService = eventService;
            _eventRuleService = eventRuleService;
        }

        //设定赛事等级
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult settinglevel(int id, EventLevelEm eventLevel)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.SettingLevel(id,eventLevel,UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //审核赛事
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult check(int id,bool isAppro)
        {
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventService.Check(id, isAppro, UserContext.SysUserContext.Id, out msg);
            return Result<string>(flag, msg);
        }

        //赛事列表
        [HttpGet]
        public ContentResult listevent(EventManageQueryRequest request)
        {
            var data = _eventService.ManageEventList(request);
            var res = new ResultDto<EventResponse>
            {
                page = request.PageIndex,
                total = request.Total,
                records = request.Records,
                rows = data
            };
            return Content(res.Serialize());
        }

        #region 赛事规则
      
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editcyclingracerule(CyclingRaceRuleRequest request)
        {
            request.SysUserId=UserContext.SysUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRuleService.EditCyclingRaceRule(request, out msg);
            return Result<string>(flag, msg);
        }


        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ContentResult editknockoutrule(KnockoutRuleRequest request)
        {
            request.SysUserId = UserContext.SysUserContext.Id;
            var res = new Result<string>();
            string msg = string.Empty;
            var flag = _eventRuleService.EditKnockoutRule(request, out msg);
            return Result<string>(flag, msg);
        }

        public ActionResult knockoutrule(int eventId)
        {
            var data = _eventRuleService.KnockoutRuleDetail(eventId);
            return View(data);
        }

        public ActionResult cyclingracerule(int eventId)
        {
            var data = _eventRuleService.CyclingRaceRuleDetail(eventId);
            return View(data);
        }
        #endregion 
    }
}