using nsda.Repository;
using nsda.Services.Contract.admin;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 赛事评分表管理
    /// </summary>
    public class EventScoreService: IEventScoreService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EventScoreService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
