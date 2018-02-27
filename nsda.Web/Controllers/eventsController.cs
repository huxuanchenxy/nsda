using nsda.Model.dto.request;
using nsda.Services;
using nsda.Services.admin;
using nsda.Services.Contract.eventmanage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class eventsController : Controller
    {
        IEventService _eventService;
        public eventsController(IEventService eventService)
        {
            _eventService = eventService;
        }
        public ActionResult detail(int id)
        {
            var userContext = UserContext.WebUserContext;
            var detail = _eventService.Detail(id);
            ViewBag.EventDate = _eventService.EventYYYYDate(id);
            ViewBag.LoginFlag = userContext == null ? 0 : 1;
            ViewBag.MemberType = userContext == null ? 1 : userContext.MemberType;
            return View(detail);
        }
    }
}