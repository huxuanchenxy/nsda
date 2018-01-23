using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Web.wxpay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Controllers
{
    public class callbackController : Controller
    {
        IPayCallBackService _payCallBackService;
        IOrderService _orderService;
        public callbackController(IPayCallBackService payCallBackService,IOrderService orderService)
        {
            _payCallBackService = payCallBackService;
            _orderService = orderService;
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
                    if (trade_status == "TRADE_FINISHED"|| trade_status == "TRADE_SUCCESS")
                    {
                        _payCallBackService.Callback(out_trade_no, trade_no);
                    }
                    else
                    {
                        //支付失败
                        string[] str = DesEncoderAndDecoder.Decrypt(out_trade_no).Split('#');
                        int orderId = str[0].ToInt32();
                        _orderService.UpdateStatus(orderId, Model.enums.OrderStatusEm.支付失败);
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
            int id = 0;
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
                    }
                    //打印页面
                    else
                    {
                        returnmsg = $"trade_status{trade_status}";
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            return RedirectToAction("paysuccess","playerpay", new { area = "player",orderId = id });
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

        public ActionResult resultnotify()
        {
            //接收从微信后台POST过来的数据
            Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();
            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }
            ProcessNotify(data);

            return View();
        }

        private void ProcessNotify(WxPayData data)
        {
            WxPayData notifyData = data;

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Response.Write(res.ToXml());
                Response.End();
            }
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Response.Write(res.ToXml());
                Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                _payCallBackService.Callback(data.GetValue("out_trade_no").ToString(), data.GetValue("transaction_id").ToString());
                Response.Write(res.ToXml());
                Response.End();
            }
        }
        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}