using nsda.Services.Contract.eventmanage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    //赛事管理
    public class eventmgController : Controller
    {
        IEventService _eventService;
        public eventmgController(IEventService eventService)
        {
            _eventService = eventService;
        }
    }
}