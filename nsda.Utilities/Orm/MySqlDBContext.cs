using Dapper;
using MySql.Data.MySqlClient;
using nsda.Utilities.Orm;
using System;
using System.Data;

namespace nsda.Utilities.Orm
{
    public class MySqlDBContext : DBContext, IDBContext
    {
        /// <summary>
        ///数据库context
        /// </summary>
        /// <param name="conn">connectionStrings 配置项</param>
        public MySqlDBContext(string conn = "defconn")
        {
            //mysql数据库链接
            connection = new MySqlConnection(GetConfig.GetConnectionString(conn));
            connection.Open();
            
            //配置
            DapperExtensions.Config.IsEnableFalseDelele = true;
            DapperExtensions.Config.IsEnableGlobalFilter = true;
        }

        #region 资源释放

        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //请求系统不要调用指定对象的终结器。
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 子类重写
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open) { connection.Close(); }
                    connection.Dispose();
                    connection = null;
                }
            }
        }


        /// <summary>
        /// 析构函数
        /// 当客户端没有显示调用Dispose()时由GC完成资源回收功能
        /// </summary>
        ~MySqlDBContext()
        {
            Dispose(false);
        }

        #endregion 资源释放
    }
}