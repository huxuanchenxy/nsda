using nsda.Services.Contract.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Utilities.Orm;
using nsda.Repository;
using nsda.Models;

namespace nsda.Services.Implement.admin
{
    public class SchoolService : ISchoolService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public SchoolService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
        //删除学校
        public bool Delete(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var school = _dbContext.Get<t_school>(id);
                if (school != null)
                {
                    school.isdelete = true;
                    school.updatetime = DateTime.Now;
                    _dbContext.Update(school);
                    flag = true;
                }
                else
                {
                    msg = "学校信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SchoolService.Delete", ex);
            }
            return flag;
        }
        //学校详情
        public SchoolResponse Detail(int id)
        {
            SchoolResponse response = null;
            try
            {
                var school = _dbContext.Get<t_school>(id);
                if (school != null)
                {
                    response = new SchoolResponse {
                        Id=school.id,
                        CityId=school.cityId,
                        CreateTime=school.createtime,
                        UpdateTime=school.updatetime,
                        IsDelete=school.isdelete,
                        Name=school.name,
                        ProvinceId=school.provinceId  
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SchoolService.Detail", ex);
            }
            return response;
        }
        // 编辑学校
        public bool Edit(SchoolRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var school = _dbContext.Get<t_school>(request.Id);
                if (school != null)
                {
                    school.provinceId = request.ProvinceId;
                    school.name = request.Name;
                    school.cityId = request.CityId;
                    school.updatetime = DateTime.Now;
                    _dbContext.Update(school);
                    flag = true;
                }
                else
                {
                    msg = "学校信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SchoolService.Edit", ex);
            }
            return flag;
        }
        //新增学校
        public bool Insert(SchoolRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var model = new t_school {
                     cityId=request.CityId,
                     name=request.Name,
                     provinceId=request.ProvinceId   
                };
                _dbContext.Insert(model);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SchoolService.Insert", ex);
            }
            return flag;
        }
        //学校列表
        public PagedList<SchoolResponse> List(SchoolQueryRequest request)
        {
            PagedList<SchoolResponse> list = new PagedList<SchoolResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SchoolService.List", ex);
            }
            return list;
        }
    }
}
