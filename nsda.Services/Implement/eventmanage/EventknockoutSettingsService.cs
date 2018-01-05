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
    /// 淘汰赛设置
    /// </summary>
    public class EventknockoutSettingsService:IEventknockoutSettingsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EventknockoutSettingsService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
