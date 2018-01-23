using System;
using System.Collections.Generic;
using System.Web;

namespace nsda.Web.wxpay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}