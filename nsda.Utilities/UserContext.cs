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

        public  static SysUserContext GetSysUser()
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
                LogUtils.LogError("UserContext.SysUserContext", ex);
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


        public static WebUserContext GetWebUser()
        {
            WebUserContext webContext = null;
            try
            {
                string key = SessionCookieUtility.GetCookie(Constant.WebCookieKey).ToString();
                if (key.IsNotEmpty())
                {
                    webContext = MemberEncoderAndDecoder.decrypt(key).Deserialize<WebUserContext>();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("UserContext.webContext", ex);
            }
            return webContext;
        }

        public static void RemoveWebCookie()
        {
            SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
        }
    }
}
