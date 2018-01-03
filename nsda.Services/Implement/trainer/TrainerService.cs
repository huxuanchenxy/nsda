using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.trainer;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.trainer
{
    /// <summary>
    /// 教练管理
    /// </summary>
    public class TrainerService: ITrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public TrainerService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
    }
}
