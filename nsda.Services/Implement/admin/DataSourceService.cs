﻿using nsda.Models;
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

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 资料管理
    /// </summary>
    public class DataSourceService: IDataSourceService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public DataSourceService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //删除资料
        public bool Delete(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_datasource>(id);
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

        public bool Insert(DataSourceRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.Insert(new t_datasource
                {
                    remark = request.Remark,
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

        public bool Edit(DataSourceRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var datasource = _dbContext.Get<t_datasource>(request.Id);
                if (datasource != null)
                {
                    datasource.filepath = request.FilePath;
                    datasource.title = request.Title;
                    datasource.remark = request.Remark;
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
                var detail = _dbContext.Get<t_datasource>(id);
                if (detail != null)
                {
                    response = new DataSourceResponse {
                        CreateTime = detail.createtime,
                        FilePath = detail.filepath,
                        Id = detail.id,
                        Remark = detail.remark,
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

        public PagedList<DataSourceResponse> List(DataSourceQueryRequest request)
        {
            PagedList<DataSourceResponse> list = new PagedList<DataSourceResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from t_datasource where isdelete=0");
                if (request.Title.IsNotEmpty())
                {
                    request.Title = "%" + request.Title + "%";
                    sb.Append(" and title like @Title");
                }
                if (request.Remark.IsNotEmpty())
                {
                    request.Remark = "%" + request.Remark + "%";
                    sb.Append(" and remark like @Remark");
                }
                list = _dbContext.Page<DataSourceResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("DataSourceService.List", ex);
            }
            return list;
        }
    }
}
