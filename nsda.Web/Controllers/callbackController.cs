using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Utilities.Alipay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class callbackController : Controller
    {
        IPayCallBackService _payCallBackService;
        public callbackController(IPayCallBackService payCallBackService)
        {
            _payCallBackService = payCallBackService;
        }
        //支付宝支付异步回调
        public ContentResult alinotifyurl()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                string out_trade_no = Request.Form["out_trade_no"];
                string notify_id = Request.Form["notify_id"];
                string sign = Request.Form["sign"];
                string trade_no = Request.Form["trade_no"];
                string trade_status = Request.Form["trade_status"];
                LogUtils.LogInfo($"ali异步回调参数out_trade_no={out_trade_no}&notify_id={notify_id}&sign={sign}&trade_no={trade_no}&trade_status={trade_status}");
                bool verifyResult = new Notify().Verify(sPara, notify_id, sign);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表
                    //商户订单号
                    //支付宝交易号
                    //交易状态
                    if (trade_status == "TRADE_FINISHED")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                        //更改订单状态
                        _payCallBackService.Callback(out_trade_no, trade_no);
                    }
                    else if (trade_status == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                        //更改订单状态
                        _payCallBackService.Callback(out_trade_no, trade_no);
                    }
                    else
                    {
                        //支付失败
                        return Content(trade_status);
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    return Content("success"); //请不要修改或删除
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                   return Content("fail"); //请不要修改或删除
                }
            }
            else
            {
               return Content("无通知参数");
            }
        }

        //支付宝支付异步回调
        public ActionResult alireturnurl()
        {
            string returnmsg = string.Empty;
            SortedDictionary<string, string> sPara = GetRequestGet();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                string out_trade_no = Request.Form["out_trade_no"];
                string notify_id = Request.Form["notify_id"];
                string sign = Request.Form["sign"];
                string trade_no = Request.Form["trade_no"]; //支付宝交易号
                string trade_status = Request.Form["trade_status"]; //交易状态
                LogUtils.LogInfo($"ali同步回调参数out_trade_no={out_trade_no}&notify_id={notify_id}&sign={sign}&trade_no={trade_no}&trade_status={trade_status}");
                bool verifyResult = new Notify().Verify(sPara, notify_id, sign);
                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表
                    if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        //更新订单信息
                        _payCallBackService.Callback(out_trade_no, trade_no);
                        returnmsg=   "支付成功";
                    }
                    //打印页面
                    else
                    {
                        returnmsg = $"trade_status{trade_status}";
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    returnmsg = "验证失败";
                }
            }
            else
            {
                returnmsg = "无返回参数";
            }
            ViewBag.ReturnMsg = returnmsg;
            return View();
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        private  string RequestString(string key)
        {
            var data = Request.QueryString[key];
            if (data.IsEmpty())
                return "";
            return data;
        }
    }
}