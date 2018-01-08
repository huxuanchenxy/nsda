using Dapper;
using nsda.Repository.Contract.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository.Implement.member
{
    public class MemberRepo : IMemberRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public MemberRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string RenderCode(string code = "nsda")
        {
            lock (lockObject)
            {
                var dy = new DynamicParameters();
                string sql = $@"select  code from t_member where code like '{code}%' order by Id desc limit 1";
                object obj = _dbContext.Query<object>(sql).FirstOrDefault();

                if (obj == null || obj.ToString().IsEmpty())
                {
                    return $"{code}1000001";
                }
                else
                {
                    string number = obj.ToString();
                    number = number.Substring(code.Length);
                    int sequence = Convert.ToInt32(number);
                    sequence += 1;
                    return $"{code}{sequence}";
                }
            }
        }
    }
}
