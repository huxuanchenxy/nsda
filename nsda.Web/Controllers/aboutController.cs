using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class aboutController : Controller
    {
        // GET: about
        public ActionResult us()
        {
            return View();
        }

        public ActionResult nsda()
        {
            return View();
        }

        public ActionResult partner()
        {
            return View();
        }
    }
}