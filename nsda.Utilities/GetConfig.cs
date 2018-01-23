using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace nsda.Utilities
{
    public static class GetConfig
    {
        /// <summary>
        /// 获取appSettings
        /// </summary>
        /// <param name="key">键名</param>
        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取appSettings
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static string GetAppSettings(string key, string def)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(setting))
            {
                return def;
            }
            return setting;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key">键名</param>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }

        /// <summary>
        /// 获取数据提供程序名称
        /// </summary>
        /// <param name="key">键名</param>
        public static string GetProviderName(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ProviderName;
        }
    }

}
