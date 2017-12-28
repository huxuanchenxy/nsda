namespace nsda.Cache
{
    public class CacheKeyManage
    {
        public static string SysUserToken(int id)
        {
            return $"crm_sys_user_token_{id}";
        }

        public static string WebUserToken(int id)
        {
            return $"crm_web_user_token_{id}";
        }

        public static string WebUserDataAuth(int id)
        {
            return $"crm_web_user_dataauth_{id}";
        }
    }
}