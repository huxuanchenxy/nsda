using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace nsda.Utilities
{
    public static class Utility
    {
        public static string Splice<T>(this IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            if (list == null || list.Count() == 0) { return string.Empty; }
            var result = new StringBuilder();
            foreach (var each in list)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            return result.ToString().TrimEnd(separator.ToCharArray());
        }

        public static bool IsInt(this int? id)
        {
            if (id != null && id > 0)
                return true;
            return false;
        }

        public static bool IsDateTime(this DateTime? datetime)
        {
            if (datetime != null && datetime != DateTime.MaxValue && datetime != DateTime.MinValue)
                return true;
            return false;
        }

        public static bool IsNotEmpty(this string data)
        {
            if (!string.IsNullOrEmpty(data))
                return true;
            return false;
        }

        public static bool IsEmpty(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return true;
            return false;
        }

        public static bool IsEmptyExtend(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return true;
            return false;
        }

        public static bool IsNotEmptyExtend(this string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
                return true;
            return false;
        }

        public static string FileSize(long Size)
        {
            string m_strSize = "";
            long factSize = 0;
            factSize = Size;
            if (factSize < 1024.00)
                m_strSize = factSize.ToString("F2") + " Byte";
            else if (factSize >= 1024.00 && factSize < 1048576)
                m_strSize = (factSize / 1024.00).ToString("F2") + " K";
            else if (factSize >= 1048576 && factSize < 1073741824)
                m_strSize = (factSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (factSize >= 1073741824)
                m_strSize = (factSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        public static string EndTime(DateTime? dt)
        {
            return Convert.ToDateTime(dt).AddDays(1).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }
        /**//// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        public static string GetSendID()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + GetRandomCode(8);
        }

        public static string GetRandomCode(int length)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random rnd = new Random(GetRandomSeed());
                sb.Append(chars[rnd.Next(0, 10)].ToString());
            }
            return sb.ToString();
        }

        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static bool IsMatch(string oldcontent, string template, out string content)
        {
            content = string.Empty;
            string keyWord = "@";
            int index = 0;
            int count = Regex.Matches(template, keyWord).Count;
            string[] ci = oldcontent.Replace("@", "##$$##&&").Split(new string[] { "||" }, StringSplitOptions.None);
            if (count != ci.Length)
            {
                return false;
            }
            StringBuilder sb = new StringBuilder(template);
            int i = 0;
            while ((index = sb.ToString().IndexOf(keyWord, index)) != -1)
            {
                sb.Replace("@", ci[i], index, 1);
                i++;
            }
            content = sb.ToString().Replace("##$$##&&", "@");
            return true;
        }

        public static string Displaymobile(this string mobile)
        {
            if (mobile.Length == 11)
            {
                return mobile.Replace(mobile.Substring(3, 4), "****");
            }
            return mobile;
        }

        public static string FileMd5(this string code)
        {
            const string key1 = "舙大羴毳舙麤鱻赑话麤";
            const string key2 = "赑上麤鱻赑羴毳舙官羴";
            const string key3 = "!@#)*(qaz";
            const string key4 = "wsx$%^(*)";
            var md5 = Md5En($"{key1}{code}{key2}");
            string md5s = Md5En($"{key3}{md5}{key4}");
            return $"{md5s.Substring(0, 6)}{md5s.Substring(10, 6)}{md5s.Substring(26)}";
        }

        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result.IsEmpty())
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (result.IsEmpty())
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        public static string GetFileExt(HttpFileCollection file)
        {
            string names = file[0].FileName;
            if (names.Contains(".csv") || names.Contains(".xls") || names.Contains(".xlsx") || names.Contains(".txt"))
            {
                string ext = file[0].ContentType;
                if (file[0].FileName.Substring(file[0].FileName.Length - 4, 4).ToLower() == "csv" && IsExcel(file))
                    return "txt";
                else if (ext == "text/plain")
                    return "txt";
                else if (IsExcel(file))
                    return "xls";
                else return "";
            }
            else return "";
        }

        public static List<string> GetMobileFromTxt(string str)
        {
            return str.TrimStart().TrimEnd().StrToMobile().Where(IsMobile).Distinct().ToList();
        }

        /// <summary>
        /// 判断是否为有效的Email地址
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSuccessEmail(this string email)
        {
            if (email.IsNotEmpty())
            {
                return Regex.IsMatch(email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的传真
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSuccessFax(this string fax)
        {
            if (fax.IsNotEmpty())
            {
                return Regex.IsMatch(fax, @"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)");
            }
            return true;
        }

        /// <summary>
        /// 验证是否是合法的电话
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSuccessTel(this string telphone)
        {
            if (telphone.IsNotEmpty())
            {
                return Regex.IsMatch(telphone, @"^\+?((\d{2,4}(-)?)|(\(\d{2,4}\)))*(\d{0,16})*$");
            }
            return true;
        }

        /// <summary>
        /// 验证QQ格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSuccessQQ(this string qq)
        {
            if (qq.IsNotEmpty())
            {
                return Regex.IsMatch(qq, @"^[1-9]\d{4,15}$");
            }
            return false;
        }

        /// <summary>
        /// 验证是否是合法的邮编
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSuccessZipCode(this string zipCode)
        {
            if (zipCode.IsNotEmpty())
            {
                return Regex.IsMatch(zipCode, @"[1-9]\d{5}(?!\d)");
            }
            return true;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Md5(this string code)
        {
            const string key1 = "舙麤羴毳舙麤鱻赑毳舙";
            const string key2 = "羴毳麤鱻赑羴毳舙鱻赑";
            const string key3 = "!@#)*(";
            const string key4 = "$%^(*)";
            var md5 = Md5En($"{key1}{code}{key2}");
            return Md5En($"{key3}{md5}{key4}");
        }

        public static string Md5En(string text)
        {
            string hexCiphertext;
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                hexCiphertext = BitConverter.ToString(data);
                md5.Clear();
            }
            return hexCiphertext.Replace("-", "").ToLower();
        }

        public static string Password(int length)
        {
            var sb = new StringBuilder();

            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            var rnd = new Random();

            for (var i = 0; i < length; i++)
            {
                sb.Append(character[rnd.Next(character.Length)]);
            }
            return sb.ToString();
        }

        public static List<string> StrToMobile(this string phone)
        {
            return phone.Split(new[] { "\r\n", " ", ",", ";", "，", "；" },
                StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static bool ToBoolean(this string content)
        {
            bool number = false;
            bool.TryParse(content, out number);
            return number;
        }

        /// <summary>
        /// Unix时间戳格式转换为DateTime时间格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp.ToString() + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 字符串转换成时间
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string content)
        {
            try
            {
                if (content.IsEmpty())
                    return DateTime.Now;
                return Convert.ToDateTime(content);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 符串转换成Decimal
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string content)
        {
            decimal number = 0;
            decimal.TryParse(content, out number);
            return number;
        }

        /// <summary>
        /// 符串转换成双精度
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static double ToDouble(this string content)
        {
            double number = 0;
            double.TryParse(content, out number);
            return number;
        }

        /// <summary>
        /// 字符串转换成浮点型
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static float ToFloat(this string content)
        {
            float number = 0;
            float.TryParse(content, out number);
            return number;
        }

        /// <summary>
        /// 字符串转换成整形
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int ToInt32(this string content)
        {
            int number = 0;
            int.TryParse(content, out number);
            return number;
        }

        public static int ToInt32(this bool content)
        {
            if (content)
                return 1;
            return 0;
        }

        /// <summary>
        /// 字符串转换成整形
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int ToInt32(this Enum value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 字符串转换成长整形
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static long ToInt64(this string content)
        {
            long number = 0;
            long.TryParse(content, out number);
            return number;
        }

        public static bool ToObjBoolean(this object obj, bool defaultvalue = false)
        {
            if (obj == null)
                return defaultvalue;
            try
            {
                return ToBoolean(obj.ToString());
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static decimal ToObjDecimal(this object obj, decimal defaultvalue = 0)
        {
            if (obj == null)
                return 0;
            return ToDecimal(obj.ToString());
        }

        public static double ToObjDouble(this object obj, double defaultvalue = 0)
        {
            if (obj == null)
                return 0;
            return ToDouble(obj.ToString());
        }

        public static int ToObjInt(this object obj, int defaultvalue = 0)
        {
            if (obj == null)
                return defaultvalue;
            try
            {
                return ToInt32(obj.ToString());
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static long ToObjLong(this object obj, long defaultvalue = 0)
        {
            if (obj == null)
                return defaultvalue;

            try
            {
                return ToInt64(obj.ToString());
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static string ToObjStr(this object obj, string msg = "")
        {
            if (obj == null)
                return msg;
            return obj.ToString();
        }

        //短信计费
        public static int ToSmsUnitPrice(this string content, int first = 70, int other = 67)
        {
            try
            {
                if (content.IsEmpty())
                    return 0;
                return content.Length <= first ? 1 : (content.Length + other - 1) / other;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static int ToUinxTime(this DateTime time)
        {
            int intResult = 0;
            System.DateTime begintime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = int.Parse((time - begintime).TotalSeconds.ToString().Split('.')[0]);
            return intResult;
        }

        private static bool IsExcel(HttpFileCollection file)
        {
            string ext = file[0].ContentType;
            if (ext == "application/vnd.ms-excel" || ext == "application/kset" || ext == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || ext == "application/ms-excel" || ext == "application/octet-stream")
                return true;
            return false;
        }

        #region 判断手机

        /// <summary>
        /// 是否是移动号码（webconfig需要配置号码段）
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsCmccMobile(this string mobile)
        {
            try
            {
                if (mobile.IsEmpty())
                    return false;

                if (!IsDigit(mobile))
                    return false;

                bool istrue = mobile.Length == 11 && Constant.cmcc.Contains(mobile.Substring(0, 3));

                if (!istrue)
                    istrue = mobile.Length == 11 && Constant.cmcc1.Contains(mobile.Substring(0, 4));

                return istrue;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是电信号码（webconfig需要配置号码段）
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsCtcMobile(this string mobile)
        {
            try
            {
                if (mobile.IsEmpty())
                    return false;

                if (!IsDigit(mobile))
                    return false;

                bool istrue = mobile.Length == 11 && Constant.ctc.Contains(mobile.Substring(0, 3));

                if (!istrue)
                    istrue = mobile.Length == 11 && Constant.ctc1.Contains(mobile.Substring(0, 4));

                return istrue;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是联通号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsCuqMobile(this string mobile)
        {
            try
            {
                if (mobile.IsEmpty())
                    return false;

                if (!IsDigit(mobile))
                    return false;

                bool istrue = mobile.Length == 11 && Constant.cuq.Contains(mobile.Substring(0, 3));

                if (!istrue)
                    istrue = mobile.Length == 11 && Constant.cuq1.Contains(mobile.Substring(0, 4));

                return istrue;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDigit(this string value)
        {
            Regex reg = new Regex("^[0-9]+$"); //判断是不是数据，要不是就表示没有选择，则从隐藏域里读出来
            Match ma = reg.Match(value);
            if (ma.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsMobile(this string str)
        {
            const string pattern = @"^(13|14|15|18|17|19)\d{9}$";
            if (str.IsEmpty())
                return false;
            return Regex.IsMatch(str, pattern);
        }

        public static bool Mobile(this string mobile)
        {
            var flag = false;
            try
            {
                if (mobile.IsCmccMobile())
                {
                    flag = true;
                }
                else if (mobile.IsCtcMobile())
                {
                    flag = true;
                }
                else if (mobile.IsCuqMobile())
                {
                    flag = true;
                }
            }
            catch
            {
            }
            return flag;
        }

        #endregion 
    }
}
