using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Services;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
        public ContentResult alipay(int id)
        {
            OrderResponse response = _orderService.OrderDetail(id);
            var userContext = UserContext.WebUserContext;
            if (response == null|| response.MemberId!= userContext.Id)
            {
                Response.Redirect("/player/player/index",true);
            }
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

        //跳转至微信支付页面
        public ContentResult wechat(int id)
        {
            OrderResponse response = _orderService.OrderDetail(id);
            var userContext = UserContext.WebUserContext;
            if (response == null || response.MemberId != userContext.Id)
            {
                Response.Redirect("/player/player/index", true);
            }
            string msg = string.Empty;
            _orderService.PayLog(id, response.Money, PayTypeEm.微信, userContext.Id, out msg);//插入支付流水信息
            return Content("");
        }

        //支付中转页
        public ActionResult pay(int payType,int orderId)
        {
            payType = payType <= 0 ? (int)PayTypeEm.支付宝 : payType;
            if (payType == (int)PayTypeEm.微信)
            {
                Response.Redirect($"/player/playerpay/alipay/{orderId}");
            }
            else
            {
                Response.Redirect($"/player/playerpay/wechat/{orderId}");
            }
            return View();
        }
    }
}