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
    /// <summary>
    /// 城市信息管理
    /// </summary>
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
                    msg = "省份不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                _dbContext.Insert(new t_sys_city
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

        public List<CityResponse> List(CityQueryRequest request)
        {
            List<CityResponse> list = new List<CityResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    join.Append(" and a.name like @Name");
                }
                if (request.ProvinceId != null && request.ProvinceId > 0)
                {
                    join.Append(" and a.provinceId=@ProvinceId ");
                }
                var sql=$@"select a.*,b.name  ProvinceName from t_sys_city a left join t_sys_province b on a.provinceId=b.Id where isdelete=0 {join.ToString()}";
                int totalCount = 0;
                list = _dbContext.Page<CityResponse>(sql,out totalCount,request.PageIndex,request.PageSize, request);
                request.Records = totalCount;
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
                var detail = _dbContext.Get<t_sys_city>(id);
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
                var detail = _dbContext.Get<t_sys_city>(id);
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
                var data = _dbContext.Select<t_sys_city>(c => c.provinceId == provinceId).ToList();
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
                    msg = "省份不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "名称不能为空";
                    return flag;
                }

                t_sys_city city = _dbContext.Get<t_sys_city>(request.Id);
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
