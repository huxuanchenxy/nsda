namespace nsda.Utilities.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    ///
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段

        private static string input_charset;
        private static string key;
        private static string partner;
        private static string sign_type;


        #endregion 字段

        static Config()
        {
            partner = "";
            key = "";
            input_charset = "utf-8";
            sign_type = "MD5";
        }


        public Config()
        {

        }

        public static string Input_charset { get; }
        public static string Key { get; set; }
        public static string Partner { get; set; }
        public static string Sign_type { get; }
    }
}