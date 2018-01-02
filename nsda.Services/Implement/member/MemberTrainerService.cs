using nsda.Repository;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.member
{
    /// <summary>
    /// 会员教练管理
    /// </summary>
    public class MemberTrainerService: IMemberTrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MemberTrainerService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
