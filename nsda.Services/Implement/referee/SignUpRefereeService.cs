using nsda.Repository;
using nsda.Services.Contract.referee;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.referee
{
    public class SignUpRefereeService: ISignUpRefereeService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public SignUpRefereeService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
