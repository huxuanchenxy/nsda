﻿using nsda.Model.enums;
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
                    sysContext = MemberEncoderAndDecoder.decrypt(key).Deserialize<SysUserContext>();
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
                    string memberId = MemberEncoderAndDecoder.decrypt(value);
                    string sessionData = SessionCookieUtility.GetSession($"webusersession_{memberId}");
                    if (sessionData.IsNotEmpty())
                    {
                        webContext = MemberEncoderAndDecoder.decrypt(sessionData).Deserialize<WebUserContext>();
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
                using (var mysql = new MySqlDBContext())
                {
                    var detail = mysql.Get<t_member>(id);
                    if (detail != null)
                    {
                        //读取已经认证的
                        string role = ((int)detail.memberType).ToString();
                        var memberextend = mysql.Select<t_memberextend>(c => c.memberId == detail.id && c.memberExtendStatus == MemberExtendStatusEm.申请通过).ToList();
                        if (memberextend != null && memberextend.Count > 0)
                        {
                            foreach (var item in memberextend)
                            {
                                role += $",{((int)item.role).ToString()}";
                            }
                        }
                        //记录缓存
                        userContext = new WebUserContext
                        {
                            Id = detail.id,
                            Name = detail.name,
                            Account = detail.account,
                            Role = role,
                            MemberType = (int)detail.memberType,
                            Status = (int)detail.memberStatus
                        };
                        DateTime expireTime = DateTime.Now.AddHours(24);
                        SessionCookieUtility.WriteCookie(Constant.WebCookieKey, MemberEncoderAndDecoder.encrypt($"{userContext.Id}"), expireTime);
                        string data = MemberEncoderAndDecoder.encrypt(userContext.Serialize());
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
                string memberId = MemberEncoderAndDecoder.decrypt(value);
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
