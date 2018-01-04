using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.trainer.Controllers
{
    public class homeController : baseController
    {
        // GET: eventmanage/home
        public ActionResult Index()
        {
            return View();
        }
    }
}