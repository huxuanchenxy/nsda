using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class callbackController : Controller
    {
        //支付宝支付回调
        public ContentResult alipay()
        {
            return Content("");
        }

        //微信支付回调
        public ContentResult wechat()
        {
            return Content("");
        }
    }
}