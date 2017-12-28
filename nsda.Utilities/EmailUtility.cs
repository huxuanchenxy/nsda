using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class EmailUtility
    {
        private string fpwd = "";
        private string from = "";
        private string host = "";
        private string prot = "";

        public EmailUtility(string from, string fpwd, string host, string port)
        {
            this.fpwd = fpwd;
            this.from = from;
            this.host = host;
            this.prot = port;
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="name">显示的发送者姓名</param>
        /// <param name="to">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="arges">可选参数 1抄送，2密送，3附件</param>
        public void Send(string name, string to, string subject, string body, params string[] arges)
        {
            try
            {
                SendMail(name, subject, body, to, arges);
            }
            catch
            {
                throw new Exception("系统异常，请稍后重试");
            }
        }

        private MailMessage InitMailMessage(string name, string subject, string body, string to, params string[] arges)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, name, Encoding.UTF8);  //发件人
            mail.To.Add(to);                  //收件人
            mail.Subject = subject;         //主题
            mail.Body = body;          //内容

            //邮件主题和正文的编码格式
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;                //邮件正文允许html编码
            mail.Priority = MailPriority.Normal;     //优先级
            //密送——就是将信密秘抄送给收件人以外的人，所有收件人看不到密件抄送的地址
            if (arges.Length > 0 && arges[0].IsNotEmpty())
                mail.Bcc.Add(arges[0]);
            //抄送——就是将信抄送给收件人以外的人,所有的收件人可以在抄送地址处看到此信还抄送给谁
            if (arges.Length > 1 && arges[1].IsNotEmpty())
                mail.CC.Add(arges[1]);
            if (arges.Length > 2 && arges[2].IsNotEmpty())
                mail.Attachments.Add(new Attachment(arges[2]));     //新增附件
            return mail;
        }

        private void SendMail(string name, string subject, string body, string to, params string[] arges)
        {
            SmtpClient client = new SmtpClient();

            //获取或设置用于验证发件人身份的凭据。
            client.Credentials = new System.Net.NetworkCredential(from, fpwd);

            //经过ssl(安全套接层)加密,163邮箱SSL协议端口号为465/994,关闭SSL时端口为25,
            //qq邮箱SSL协议端口号为465或587,关闭SSL时端口同样为25，不过用SSL加密后发送邮件都失败，具体原因不知
            if (host.Contains("gmail.com"))
                client.EnableSsl = true;
            client.Port = prot.ToInt32();                //端口号
            client.Host = host;     //获取或设置用于 SMTP 事务的主机的名称或 IP 地址
            try
            {
                client.Send(InitMailMessage(name, subject, body, to, arges));
            }
            catch
            {
                throw new Exception("系统异常，请稍后重试");
            }
        }
    }

}
