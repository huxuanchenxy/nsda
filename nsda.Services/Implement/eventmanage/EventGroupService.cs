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
    /// 赛事组别管理
    /// </summary>
    public class EventGroupService: IEventGroupService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EventGroupService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
