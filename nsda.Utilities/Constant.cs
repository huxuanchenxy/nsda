using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class Constant
    {
        public static readonly string RedisPwd = ConfigurationManager.AppSettings["redis_pwd"];//redis密码
        public static readonly bool redis_auto_start = ConfigurationManager.AppSettings["redis_auto_start"].ToBoolean();
        public static readonly int redis_local_cachetime = ConfigurationManager.AppSettings["redis_local_cachetime"].ToInt32();
        public static readonly int redis_max_read = ConfigurationManager.AppSettings["redis_max_read"].ToInt32();
        public static readonly int redis_max_write = ConfigurationManager.AppSettings["redis_max_write"].ToInt32();
        public static readonly string redis_read_server = ConfigurationManager.AppSettings["redis_read_server"];
        public static readonly string redis_write_server = ConfigurationManager.AppSettings["redis_write_server"];
        public static readonly bool redis_record_log = ConfigurationManager.AppSettings["redis_record_log"].ToBoolean();

        public static readonly string cmcc = ConfigurationManager.AppSettings["cmcc"];//移动号码
        public static readonly string cmcc1 = ConfigurationManager.AppSettings["cmcc1"];//移动虚拟号
        public static readonly string cuq = ConfigurationManager.AppSettings["cuq"];//联通号码
        public static readonly string cuq1 = ConfigurationManager.AppSettings["cuq1"];//联通虚拟号
        public static readonly string ctc = ConfigurationManager.AppSettings["ctc"];//电信号码
        public static readonly string ctc1 = ConfigurationManager.AppSettings["ctc1"];//电信虚拟号

        public static readonly string WebCookieKey = "webuserkey";
        public static readonly string SysCookieKey = "sysuserkey";
        public static readonly string FindPwd = "findpwdid";
        public static readonly string FindPwdEmail = "findpwdemail";

        public static readonly string EmailAccount = ConfigurationManager.AppSettings["fromaccount"];//邮件发送账号
        public static readonly string EmailPwd = ConfigurationManager.AppSettings["frompwd"];//邮件发送账号密码
        public static readonly string Email_smtp = ConfigurationManager.AppSettings["email_smtp"];
        public static readonly string Email_port = ConfigurationManager.AppSettings["email_port"];

        public static readonly decimal AuthMoney = ConfigurationManager.AppSettings["authmoney"].ToObjDecimal();//实名认证金额

        public static readonly string PayAccount = ConfigurationManager.AppSettings["PayAccount"];//支付宝账号

        public static readonly string Version = ConfigurationManager.AppSettings["version"];//css js版本号
        
    }
}
