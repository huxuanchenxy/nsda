using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace nsda.Utilities
{
    public class nsdaRequest
    {
        public static string Get(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse webreponse = null;
            StreamReader sr = null;
            var restString = string.Empty;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 5000;
                request.KeepAlive = false;
                webreponse = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(webreponse.GetResponseStream(), System.Text.Encoding.UTF8);
                restString = sr.ReadToEnd();
                return restString;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("Get:" + url, ex);
                restString = ex.Message;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (webreponse != null)
                {
                    webreponse.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return restString;
        }

        public static string GetIpAddress(string ip)
        {
            string address = string.Empty;
            string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip=" + ip;
            string response = Get(url);
            if (response.IsNotEmpty())
            {
                try
                {
                    IpReturn ipreturn = JsonUtils.Deserialize<IpReturn>(response);
                    if (ipreturn != null && ipreturn.ret == "1")
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append($"{HttpUtility.UrlDecode(ipreturn.country)} ");
                        sb.Append($"{HttpUtility.UrlDecode(ipreturn.province)} ");
                        sb.Append($"{HttpUtility.UrlDecode(ipreturn.city)} ");
                        address = sb.ToString();
                    }
                    else
                    {
                        address = "";
                    }
                }
                catch
                {
                    address = "";
                }
            }
            else
            {
                address = "";
            }
            return address;
        }

        public static string Post(string weburl, string data, Encoding encode)
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse response = null;
            StreamReader aspx = null;
            Stream newStream = null;
            string result = string.Empty;
            try
            {
                byte[] byteArray = encode.GetBytes(data);
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
                webRequest.Method = "POST";
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                //接收返回信息：
                response = (HttpWebResponse)webRequest.GetResponse();
                aspx = new StreamReader(response.GetResponseStream(), encode);
                result = aspx.ReadToEnd();
            }
            catch
            {
                result = "";
            }
            finally
            {
                if (aspx != null)
                {
                    aspx.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (webRequest != null)
                {
                    webRequest.Abort();
                }
            }
            return result;
        }

        public class IpReturn
        {
            public string city { get; set; }
            public string country { get; set; }
            public string province { get; set; }
            public string ret { get; set; }
        }

        #region HtmlEncode(对html字符串进行编码)

        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }

        #endregion HtmlEncode(对html字符串进行编码)

        #region UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion UrlEncode(对Url进行编码)

        #region UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion UrlDecode(对Url进行解码)
    }
}
