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
    /// 身份管理
    /// </summary>
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

        public bool Insert(ProvinceRequest request,int sysUserId,out string msg)
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

                _dbContext.Insert(new t_sys_province
                {
                    name = request.Name,
                    isInter=request.IsInter
                });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("ProvinceService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(ProvinceRequest request, int sysUserId, out string msg)
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

                t_sys_province province = _dbContext.Get<t_sys_province>(request.Id);
                if (province != null)
                {
                    province.name = request.Name;
                    province.isInter = request.IsInter;
                    province.updatetime = DateTime.Now;
                    _dbContext.Update(province);
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
                LogUtils.LogError("ProvinceService.Edit", ex);
            }
            return flag;
        }

        public List<ProvinceResponse> List(ProvinceQueryRequest request)
        {
            List<ProvinceResponse> list = new List<ProvinceResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Name.IsNotEmpty())
                {
                    request.Name = $"%{request.Name}%";
                    join.Append(" and name like @Name");
                }
                if (request.IsInter != null)
                {
                    join.Append(" and IsInter=@IsInter ");
                }
                var sql=$@"select * from t_sys_province  where isdelete=0 {join.ToString()} order by createtime desc";              
                int totalCount = 0;
                list = _dbContext.Page<ProvinceResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("ProvinceService.List", ex);
            }
            return list;
        }

        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_province>(id);
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
                var detail = _dbContext.Get<t_sys_province>(id);
                if (detail != null)
                {
                    response = new ProvinceResponse
                    {
                        Id = detail.id,
                        Name = detail.name,
                        IsInter=detail.isInter
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("ProvinceService.Detail", ex);
            }
            return response;
        }

        public List<BaseDataResponse> Province(bool?  isInter)
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                var data = new List<t_sys_province>();
                if (isInter == null)
                {
                    data = _dbContext.Select<t_sys_province>(c => true).ToList();
                }
                else if ((bool)isInter)
                {
                    data = _dbContext.Select<t_sys_province>(c => c.isInter).ToList();
                }
                else{
                    data= _dbContext.Select<t_sys_province>(c => !c.isInter).ToList();
                }
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
                LogUtils.LogError("ProvinceService.Province", ex);
            }
            return list;
        }
    }
}
