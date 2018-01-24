using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Web.wxpay;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace nsda.Web.Areas.player.Controllers
{
    /// <summary>
    /// 选手支付模块
    /// </summary>
    public class playerpayController : playerbaseController
    {
        IOrderService _orderService;
        public playerpayController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 支付页
        /// </summary>
        public ActionResult index(int orderId)
        {
            var detail = _orderService.Detail(orderId);
            if (detail == null || detail.MemberId != UserContext.WebUserContext.Id)
                Response.Redirect("/error/error", true);
            return View(detail);
        }

        //跳转至支付宝支付页面
        public ActionResult alipay(int id)
        {
            OrderResponse response = _orderService.OrderDetail(id);
            var userContext = UserContext.WebUserContext;
            if (response == null|| response.MemberId!= userContext.Id)
            {
                return RedirectToAction("index", "player", new { area = "player" });
            }
            else if (response.OrderStatus == OrderStatusEm.支付成功)
            {
                return RedirectToAction("paysuccess", "playerpay", new { area = "player", orderId = id });
            }
            else if (response.OrderStatus != OrderStatusEm.等待支付 && response.OrderStatus != OrderStatusEm.支付失败)
            {
                return RedirectToAction("index", "player", new { area = "player" });
            }
            else
            {
                string msg = string.Empty;
                _orderService.PayLog(id, response.Money, PayTypeEm.支付宝, userContext.Id, out msg);//插入支付流水信息

                string str = "1";
                string str2 = "/callback/alinotifyurl";//异步回调地址
                string str3 = "/callback/alireturnurl";//同步回调地址
                string str4 = Constant.PayAccount;//支付宝账号
                string str5 = DesEncoderAndDecoder.Encrypt($"{id}#nsda");
                string pkgName = response.Remark;
                string str7 = response.Money.ToString();
                string info = response.Remark;
                string str9 = "";//网站地址
                string str10 = Submit.Query_timestamp();
                string str11 = "";
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "create_direct_pay_by_user");
                sParaTemp.Add("payment_type", str);
                sParaTemp.Add("notify_url", str2);
                sParaTemp.Add("return_url", str3);
                sParaTemp.Add("seller_email", str4);
                sParaTemp.Add("out_trade_no", str5);
                sParaTemp.Add("subject", pkgName);
                sParaTemp.Add("total_fee", str7);
                sParaTemp.Add("body", info);
                sParaTemp.Add("show_url", str9);
                sParaTemp.Add("anti_phishing_key", str10);
                sParaTemp.Add("exter_invoke_ip", str11);
                string content = Submit.BuildRequest(sParaTemp, "get", "确认");
                return Content(content);
            }
        }

        //跳转至微信支付页面
        public ActionResult wechat(int id)
        {
            OrderResponse response = _orderService.OrderDetail(id);
            var userContext = UserContext.WebUserContext;
            if (response == null || response.MemberId != userContext.Id)
            {
                return RedirectToAction("index", "player", new { area = "player" });
            }
            else if (response.OrderStatus == OrderStatusEm.支付成功)
            {
                return RedirectToAction("paysuccess", "playerpay", new { area = "player", orderId=id });
            }
            else if (response.OrderStatus != OrderStatusEm.等待支付 && response.OrderStatus != OrderStatusEm.支付失败)
            {
                return RedirectToAction("index", "player",new { area = "player" });
            }
            else
            {
                string msg = string.Empty;
                _orderService.PayLog(id, response.Money, PayTypeEm.微信, userContext.Id, out msg);//插入支付流水信息
                NativePay nativePay = new NativePay();
                string url = nativePay.GetPayUrl(response);
                ViewBag.QRCode = "/commondata/makeqrcode?data=" + HttpUtility.UrlEncode(url);
                ViewBag.Order = response;
                return View();
            }
        }

        //支付成功页
        public ActionResult paysuccess(int orderId)
        {
            var order = _orderService.OrderDetail(orderId);
            return View(order);
        }

        //轮询订单状态是否改变
        public ContentResult paymentresult(int orderId)
        {
            var response = _orderService.OrderDetail(orderId);
            if (response.OrderStatus != OrderStatusEm.等待支付)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
    }
}