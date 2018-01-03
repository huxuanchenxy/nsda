using nsda.Repository;
using nsda.Services.trainer;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.referee
{
    public class RefereeService: IRefereeService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public RefereeService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
