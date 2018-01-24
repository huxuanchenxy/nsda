using nsda.Model.enums;
using nsda.Models;
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
                string value = SessionCookieUtility.GetCookie(Constant.WebCookieKey).ToString();
                if (value.IsNotEmpty())
                {
                    string memberId = DesEncoderAndDecoder.Decrypt(value);
                    string sessionData = SessionCookieUtility.GetSession($"webusersession_{memberId}");
                    if (sessionData.IsNotEmpty())
                    {
                        webContext = DesEncoderAndDecoder.Decrypt(sessionData).Deserialize<WebUserContext>();
                    }
                    else
                    {
                        webContext = GetUserContext(memberId.ToInt32());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("UserContext.GetWebUser", ex);
            }
            return webContext;
        }

        private static WebUserContext GetUserContext(int id)
        {
            WebUserContext userContext = null;
            try
            {
                using (IDBContext dbcontext = new MySqlDBContext())
                {
                    var detail = dbcontext.Get<t_member>(id);
                    if (detail != null)
                    {
                        //读取已经认证的
                        string role = ((int)detail.memberType).ToString();
                        var memberextend = dbcontext.Select<t_memberextend>(c => c.memberId == detail.id && c.memberExtendStatus == MemberExtendStatusEm.申请通过).ToList();
                        if (memberextend != null && memberextend.Count > 0)
                        {
                            foreach (var item in memberextend)
                            {
                                role += $",{((int)item.role).ToString()}";
                            }
                        }
                        decimal points = dbcontext.ExecuteScalar($"select points from t_memberpoints where memberId={id}").ToObjDecimal();
                        //记录缓存
                        userContext = new WebUserContext
                        {
                            Id = detail.id,
                            Name = detail.name,
                            Account = detail.account,
                            Role = role,
                            MemberType = (int)detail.memberType,
                            Status = (int)detail.memberStatus,
                            Points = points
                        };
                        DateTime expireTime = DateTime.Now.AddHours(24);
                        SessionCookieUtility.WriteCookie(Constant.WebCookieKey, DesEncoderAndDecoder.Encrypt($"{userContext.Id}"), expireTime);
                        string data = DesEncoderAndDecoder.Encrypt(userContext.Serialize());
                        SessionCookieUtility.WriteSession($"webusersession_{userContext.Id}", data);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.GetUserContext", ex);
            }
            return userContext;
        }

        public static void RemoveWebUserInfo()
        {
            var value = SessionCookieUtility.GetCookie(Constant.WebCookieKey);
            if (value.IsNotEmpty())
            {
                string memberId = DesEncoderAndDecoder.Decrypt(value);
                string sessionData = SessionCookieUtility.GetSession($"webusersession_{memberId}");
                if (sessionData.IsNotEmpty())
                {
                    SessionCookieUtility.RemoveSession($"webusersession_{memberId}");
                }
                SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
            }
        }
    }
}
