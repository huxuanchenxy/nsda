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
    public class ProvinceService:IProvinceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public ProvinceService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        public bool Insert(ProvinceRequest request, out string msg)
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

                _dbContext.Insert(new t_province
                {
                    name = request.Name,
                });
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("ProvinceService.Insert", ex);
            }
            return flag;
        }

        public PagedList<ProvinceResponse> List(ProvinceQueryRequest request)
        {
            PagedList<ProvinceResponse> list = new PagedList<ProvinceResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from t_province where isdelete=0");
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    sb.Append(" and name like @Name");
                }
                list = _dbContext.Page<ProvinceResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("ProvinceService.List", ex);
            }
            return list;
        }

        public bool Delete(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_province>(id);
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
                LogUtils.LogError("ProvinceService.Delete", ex);
            }
            return flag;
        }

        public ProvinceResponse Detail(int id)
        {
            ProvinceResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_province>(id);
                if (detail != null)
                {
                    response = new ProvinceResponse
                    {
                        Id = detail.id,
                        Name = detail.name
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("ProvinceService.Detail", ex);
            }
            return response;
        }
    }
}
