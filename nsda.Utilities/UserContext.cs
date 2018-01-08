using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public  class UserContext
    {
        public static SysUserContext SysUserContext
        {
            get
            {
                return GetSysUser();
            }
        }

        private  static SysUserContext GetSysUser()
        {
            SysUserContext sysContext = null;
            try
            {
                string key = SessionCookieUtility.GetCookie(Constant.SysCookieKey).ToString();
                if (key.IsNotEmpty())
                {
                    sysContext = MemberEncoderAndDecoder.decrypt(key).Deserialize<SysUserContext>();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("UserContext.GetSysUser", ex);
            }
            return sysContext;
        }

        public  static void RemoveSysCookie()
        {
            SessionCookieUtility.RemoveCookie(Constant.SysCookieKey);
        }


        public static WebUserContext WebUserContext
        {
            get
            {
                return GetWebUser();
            }
        }


        private static WebUserContext GetWebUser()
        {
            WebUserContext webContext = null;
            try
            {
                string value = SessionCookieUtility.GetCookie(Constant.WebCookieKey).ToString();
                if (value.IsNotEmpty())
                {
                    string sessionKey= MemberEncoderAndDecoder.decrypt(value);
                    string sessionData = SessionCookieUtility.GetSession(sessionKey);
                    if (sessionData.IsNotEmpty())
                    {
                        webContext = MemberEncoderAndDecoder.decrypt(sessionData).Deserialize<WebUserContext>();
                    }
                    else
                    {
                        SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("UserContext.GetWebUser", ex);
            }
            return webContext;
        }

        public static void RemoveWebUserInfo()
        {
            var data = SessionCookieUtility.GetCookie(Constant.WebCookieKey);
            if (data.IsNotEmpty())
            {
                string sessionKey = MemberEncoderAndDecoder.decrypt(data);
                string sessionData = SessionCookieUtility.GetSession(sessionKey);
                if (sessionData.IsNotEmpty())
                {
                    SessionCookieUtility.RemoveSession(sessionKey);
                }
                SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
            }
        }
    }
}
