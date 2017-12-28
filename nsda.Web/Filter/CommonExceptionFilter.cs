using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Filter
{
    public class CommonExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            StringBuilder error = new StringBuilder();
            var enter = Environment.NewLine;
            error.Append(enter);
            error.Append($"发生时间:{DateTime.Now}");
            error.Append(enter);

            error.Append($"用户IP:{Utility.GetClientIP()}");
            error.Append(enter);

            error.Append($"发生异常页:{filterContext.HttpContext.Request.RawUrl}");
            error.Append(enter);

            error.Append($"控制器:{filterContext.RouteData.Values["controller"]}");
            error.Append(enter);

            error.Append($"Action:{filterContext.RouteData.Values["action"]}");
            error.Append(enter);

            LogUtils.LogError(error.ToString(), filterContext.Exception);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var data = new
                {
                    flag = false,
                    data = string.Empty,
                    msg = filterContext.Exception.Message
                };
                filterContext.Result = new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var view = new ViewResult
                {
                    ViewName = "~/Views/error/error.cshtml"
                };
                filterContext.Result = view;
            }

            filterContext.ExceptionHandled = false;
        }
    }
}