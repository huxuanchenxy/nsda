using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.admin
{
    public class BaseService : IBaseService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public BaseService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        public List<BaseDataResponse> City(int provinceId)
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                if (provinceId <= 0)
                    return list;
                var data = _dbContext.Select<t_city>(c => c.provinceId==provinceId).ToList();
                if (data != null && data.Count > 0)
                {
                    list = data.Select(c => new BaseDataResponse
                    {
                        Id = c.id,
                        Name = c.name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("BaseService.City", ex);
            } 
            return list;
        }

        public List<BaseDataResponse> Province()
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                var data = _dbContext.Select<t_province>(c=>true).ToList();
                if (data!=null&&data.Count>0)
                {
                    list = data.Select(c => new BaseDataResponse
                    {
                        Id=c.id,
                        Name=c.name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("BaseService.City", ex);
            }
            return list;
        }
    }
}
