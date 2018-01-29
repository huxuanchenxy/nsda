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
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Services.admin;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 资料管理
    /// </summary>
    public class DataSourceService: IDataSourceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public DataSourceService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        //删除资料
        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_datasource>(id);
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
                LogUtils.LogError("DataSourceService.Delete", ex);
            }
            return flag;
        }

        public bool Insert(DataSourceRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.Insert(new t_sys_datasource
                {
                    title = request.Title,
                    filepath=request.FilePath 
                });
                flag = true; 
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("DataSourceService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(DataSourceRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var datasource = _dbContext.Get<t_sys_datasource>(request.Id);
                if (datasource != null)
                {
                    datasource.filepath = request.FilePath;
                    datasource.title = request.Title;
                    datasource.updatetime = DateTime.Now;
                    _dbContext.Update(datasource);
                    flag = true;
                }
                else
                {
                    msg = "资料信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("DataSourceService.Edit", ex);
            }
            return flag;
        }

        public DataSourceResponse Detail(int id)
        {
            DataSourceResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_sys_datasource>(id);
                if (detail != null)
                {
                    response = new DataSourceResponse {
                        CreateTime = detail.createtime,
                        FilePath = detail.filepath,
                        Id = detail.id,
                        Title = detail.title,
                        UpdateTime = detail.updatetime  
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("DataSourceService.Detail", ex);
            }
            return response;
        }

        public List<DataSourceResponse> List(DataSourceQueryRequest request)
        {
            List<DataSourceResponse> list = new List<DataSourceResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Title.IsNotEmpty())
                {
                    request.Title = $"%{request.Title}%";
                    join.Append(" and title like @Title");
                }
                var sql=$@"select * from t_sys_datasource where isdelete=0 {join.ToString()} order by createtime desc";            
                int totalCount = 0;
                list = _dbContext.Page<DataSourceResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("DataSourceService.List", ex);
            }
            return list;
        }
    }
}
