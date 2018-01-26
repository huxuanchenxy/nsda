using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services
{
    public class UserContext
    {
        public static SysUserContext SysUserContext
        {
            get
            {
                return GetSysUser();
            }
        }

        private static SysUserContext GetSysUser()
        {
            SysUserContext sysContext = null;
            try
            {
                string key = SessionCookieUtility.GetCookie(Constant.SysCookieKey).ToString();
                if (key.IsNotEmpty())
                {
                    sysContext = DesEncoderAndDecoder.Decrypt(key).Deserialize<SysUserContext>();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("UserContext.GetSysUser", ex);
            }
            return sysContext;
        }

        public static void RemoveSysCookie()
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
                string key = SessionCookieUtility.GetCookie(Constant.SysCookieKey).ToString();
                if (key.IsNotEmpty())
                {
                    webContext = DesEncoderAndDecoder.Decrypt(key).Deserialize<WebUserContext>();
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
            var value = SessionCookieUtility.GetCookie(Constant.WebCookieKey);
            if (value.IsNotEmpty())
            {
                SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
            }
        }
    }
}
