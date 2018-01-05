using nsda.Services.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class findpwdController : Controller
    {
        IMemberService _memberService;
        public findpwdController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult index()
        {
            return View();
        }
    }
}