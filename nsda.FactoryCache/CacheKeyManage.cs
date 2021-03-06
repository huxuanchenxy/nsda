﻿namespace nsda.Cache
{
    public class CacheKeyManage
    {
        public static string SysUserToken(int id)
        {
            return $"nsda_sys_user_token_{id}";
        }

        public static string WebUserToken(int id)
        {
            return $"nsda_web_user_token_{id}";
        }

        public static string WebUserDataAuth(int id)
        {
            return $"nsda_web_user_dataauth_{id}";
        }
    }
}