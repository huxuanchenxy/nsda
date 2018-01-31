using Dapper;
using nsda.Repository.Contract.eventmanage;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository.Implement.eventmanage
{
    public class EventRoomRepo: IEventRoomRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public EventRoomRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string RenderCode(int eventId, string code = "nsda")
        {
            lock (lockObject)
            {
                var dy = new DynamicParameters();
                string sql = $@"select  code from t_event_room where eventId={eventId} and  code like '{code}%' order by Id desc limit 1";
                object obj = _dbContext.ExecuteScalar(sql);

                if (obj == null || obj.ToString().IsEmpty())
                {
                    return $"{code}101";
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
