using nsda.Model.dto.request;
using nsda.Services.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class homeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}