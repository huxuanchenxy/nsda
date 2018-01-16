using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 国家信息管理
    /// </summary>
    public class CountryService: ICountryService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public CountryService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        public bool Insert(CountryRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                _dbContext.Insert(new t_country
                {
                    name = request.Name
                });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("CountryService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(CountryRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                t_country country = _dbContext.Get<t_country>(request.Id);
                if (country != null)
                {
                    country.name = request.Name;
                    _dbContext.Update(country);
                    flag = true;
                }
                else
                {
                    msg = "数据信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("CountryService.Edit", ex);
            }
            return flag;
        }

        public List<CountryResponse> List(CountryQueryRequest request)
        {
            List<CountryResponse> list = new List<CountryResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    join.Append(" and name like @Name");
                }
                var sql=$@"select *  from t_country  where isdelete=0 {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<CountryResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("CountryService.List", ex);
            }
            return list;
        }

        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_country>(id);
                if (detail != null)
                {
                    detail.isdelete = true;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                }
                else
                {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("CountryService.Delete", ex);
            }
            return flag;
        }

        public CountryResponse Detail(int id)
        {
            CountryResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_country>(id);
                if (detail != null)
                {
                    response = new CountryResponse
                    {
                        Id = detail.id,
                        Name = detail.name
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("CountryService.Detail", ex);
            }
            return response;
        }

        public List<BaseDataResponse> Country()
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                var data = _dbContext.Select<t_country>(c=>true).ToList();
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
                LogUtils.LogError("CountryService.Country", ex);
            }
            return list;
        }
    }
}
