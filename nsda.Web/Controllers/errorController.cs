using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class errorController : Controller
    {
        public ActionResult error()
        {
            return View();
        }
    }
}