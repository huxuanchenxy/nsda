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
    public class SignUpPlayerRepo:ISignUpPlayerRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public SignUpPlayerRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string RenderCode(int eventId,string code = "t")
        {
            lock (lockObject)
            {
                var dy = new DynamicParameters();
                string sql = $@"select  groupnum from t_event_player_signup where eventId={eventId} and groupnum like '{code}%' order by Id desc limit 1";
                object obj = _dbContext.ExecuteScalar(sql);

                if (obj == null || obj.ToString().IsEmpty())
                {
                    return $"{code}1001";
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
