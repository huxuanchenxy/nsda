using nsda.Repository;
using nsda.Services.Contract.eventmanage;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.eventmanage
{
    /// <summary>
    /// 循环赛管理
    /// </summary>
    public class EventCyclingRaceService: IEventCyclingRaceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EventCyclingRaceService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
