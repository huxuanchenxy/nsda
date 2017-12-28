using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class LogUtils
    {
        #region 日志记录

        /// <summary>
        /// 订单日志
        /// </summary>
        private static readonly Logger NsdaLogger = LogManager.GetLogger("nsda");

        public static void LogError(string message, Exception ex = null)
        {
            Task.Factory.StartNew(() => NsdaLogger.Error(ex, message));
        }

        /// <summary>
        /// 订单日志记录
        /// </summary>
        /// <param name="message">错误描述</param>
        /// <param name="ex">Exception</param>
        public static void LogInfo(string message)
        {
            Task.Factory.StartNew(() => NsdaLogger.Info(message));
        }

        #endregion 日志记录
    }
}
