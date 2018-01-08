using nsda.Services.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class memberController : baseController
    {
        IMemberService _memberService;
        public memberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region view
        public ActionResult index()
        {
            return View();
        }
        #endregion

        #region ajax

        #endregion
    }
}