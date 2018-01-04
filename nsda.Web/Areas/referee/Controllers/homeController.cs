using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.referee.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}