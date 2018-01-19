using nsda.Model.dto;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.player.Controllers
{
    [PlayerLogin(LoginModeEnum.Enforce)]
    public class playerbaseController : Controller
    {
        protected virtual ContentResult Result<T>(bool flag, string message = "", T data = default(T))
        {
            return Content((new Result<T> { flag = flag, msg = message, data = data }).Serialize());
        }
    }
}