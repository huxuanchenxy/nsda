using Dapper;
using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.eventmanage
{
    public class EventService: IEventService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public EventService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        private static object lockObject = new object();

        //生成会员Code
        private string RenderCode()
        {
            lock (lockObject)
            {
                var dy = new DynamicParameters();
                string sql = @"select  code from t_event where code like 'nsda%' order by Id desc limit 1";
                object obj = _dbContext.Query<object>(sql).FirstOrDefault();

                if (obj == null || obj.ToString().IsEmpty())
                {
                    return "nsda1000001";
                }
                else
                {
                    string number = obj.ToString();
                    number = number.Substring(4);
                    int sequence = Convert.ToInt32(number);
                    sequence += 1;
                    return $"nsda{sequence}";
                }
            }
        }
    }
}
