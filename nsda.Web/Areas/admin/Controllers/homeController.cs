using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class homeController : baseController
    {
        // GET: admin/home
        public ActionResult Index()
        {
            return View();
        }
    }
}