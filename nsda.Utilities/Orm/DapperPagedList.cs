using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dapper
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class DapperList
    {
        //默认页码
        const int def = 20;
        //查询字段
        private static readonly Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //排序字段
        private static readonly Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //去重字段
        private static readonly Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 构造基本Page
        /// </summary>
        /// <typeparam name="T">结构集</typeparam>
        /// <param name="sql">查询sql</param>
        /// <param name="param">查询参数</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">每页的条数</param>
        /// <param name="sqlPage">返回的sql</param>
        /// <param name="pageParam">返回的参数</param>
        /// <returns></returns>
        public static List<T> Page<T>(this IDbConnection connection, string sql, out int totalCount, int pageIndex = 1, int pageSize = 20, object param = null)
        {
            totalCount = 0;
            //替换 select filed  => select count(*)
            var m = rxColumns.Match(sql);
            // 获取 count(*)
            var g = m.Groups[1];

            //查询field
            var sqlSelectRemoved = sql.Substring(g.Index);

            var count = rxDistinct.IsMatch(sqlSelectRemoved) ? m.Groups[1].ToString().Trim() : "*";
            var sqlCount = string.Format("{0} count({1}) from ({0} 1 {2}) t", sql.Substring(0, g.Index), count, sql.Substring(g.Index + g.Length));
            //查找 order by filed
            m = rxOrderBy.Match(sqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }
            //查询总条数
            totalCount = connection.ExecuteScalar(sqlCount, param).ToObjInt();
            if (totalCount > 0)
            {
                //分页查询语句
                var connectionName = connection.GetType().Name.ToLower();
                string pagelimit; //分页关键字
                _sqlpage.TryGetValue(connectionName, out pagelimit);
                string queryPage = sql + pagelimit;

                var pageParam = new DynamicParameters(param);
                pageParam.Add("@offset", (pageIndex - 1) * pageSize);
                pageParam.Add("@limit", pageSize);
                return connection.Query<T>(queryPage, pageParam).ToList();
            }
            else {
                return new List<T>();
            } 
        }

        /// <summary>
        /// 分页的关键字，目前写了mysql 和 mssql
        /// </summary>
        private static readonly IDictionary<string, string> _sqlpage = new Dictionary<string, string>
                                                                                {
                                                                                    { "mysqlconnection", "\n limit @limit offset @offset" },
                                                                                    { "sqlconnection", "\n offset @offset row fetch next @limit rows only" }
                                                                                };

    }
}
