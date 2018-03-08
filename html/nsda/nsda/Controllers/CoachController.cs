using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Controllers
{
    public class CoachController : Controller
    {
        // GET: Coach

        public ActionResult coachregform()
        {
            ViewBag.Message = "教练注册";
            return View();
        }
        public ActionResult coachinformation()
        {
            ViewBag.Message = "Coach Personal Information";
            return View();
        }
        public ActionResult coachboundplayer()
        {
            ViewBag.Message = "绑定学员";
            return View();
        }
        public ActionResult boundplayer()
        {
            ViewBag.Message = "绑定学员";
            return View();
        }
    }
}