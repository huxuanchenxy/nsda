using nsda.Repository;
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
using nsda.Services.admin;

namespace nsda.Services.Implement.admin
{
    public class CityService:ICityService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public CityService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        public bool Insert(CityRequest request,int sysUserId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ProvinceId<=0)
                {
                    msg = "父级不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                _dbContext.Insert(new t_city
                {
                    provinceId=request.ProvinceId,
                    name = request.Name
                });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("CityService.Insert", ex);
            }
            return flag;
        }

        public PagedList<CityResponse> List(CityQueryRequest request)
        {
            PagedList<CityResponse> list = new PagedList<CityResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.name  ProvinceName from t_city a left join t_province b on a.provinceId=b.Id where isdelete=0");
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    sb.Append(" and a.name like @Name");
                }
                if (request.ProvinceId != null && request.ProvinceId > 0)
                {
                    sb.Append(" and a.provinceId=@ProvinceId ");
                }
                list = _dbContext.Page<CityResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("CityService.List", ex);
            }
            return list;
        }

        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_city>(id);
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
                LogUtils.LogError("CityService.Delete", ex);
            }
            return flag;
        }

        public CityResponse Detail(int id)
        {
            CityResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_city>(id);
                if (detail != null)
                {
                    response = new CityResponse
                    {
                        Id = detail.id,
                        Name = detail.name,
                        ProvinceId=detail.provinceId
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("CityService.Detail", ex);
            }
            return response;
        }

        public List<BaseDataResponse> City(int provinceId)
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                if (provinceId <= 0)
                    return list;
                var data = _dbContext.Select<t_city>(c => c.provinceId == provinceId).ToList();
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
                LogUtils.LogError("CityService.City", ex);
            }
            return list;
        }

        public bool Edit(CityRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ProvinceId <= 0)
                {
                    msg = "父级不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                t_city city = _dbContext.Get<t_city>(request.Id);
                if (city != null)
                {
                    city.name = request.Name;
                    city.provinceId = request.ProvinceId;
                    _dbContext.Update(city);
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
                LogUtils.LogError("CityService.Edit", ex);
            }
            return flag;
        }
    }
}
