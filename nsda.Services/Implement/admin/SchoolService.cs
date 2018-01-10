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
using Dapper;
using nsda.Services.admin;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 学校信息管理
    /// </summary>
    public class SchoolService : ISchoolService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public SchoolService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }
        //删除学校
        public bool Delete(int id, int sysUserId, out string msg)
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
                        Id = school.id,
                        CityId = school.cityId,
                        CreateTime = school.createtime,
                        UpdateTime = school.updatetime,
                        IsDelete = school.isdelete,
                        ChinessName = school.chinessname,
                        EnglishName = school.englishname,
                        isInter=school.isInter,
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
        public bool Edit(SchoolRequest request, int sysUserId, out string msg)
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
                if (request.CityId<=0)
                {
                    msg = "城市不能为空";
                    return flag;
                }

                if (request.ChinessName.IsEmpty())
                {
                    msg = "学校名称不能为空";
                    return flag;
                }

                var school = _dbContext.Get<t_school>(request.Id);
                if (school != null)
                {
                    school.provinceId = request.ProvinceId;
                    school.englishname = request.EnglishName;
                    school.chinessname = request.ChinessName;
                    school.isInter = request.IsInter;
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
        public bool Insert(SchoolRequest request, int sysUserId, out string msg)
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
                if (request.CityId <= 0)
                {
                    msg = "城市不能为空";
                    return flag;
                }

                if (request.ChinessName.IsEmpty())
                {
                    msg = "学校名称不能为空";
                    return flag;
                }

                var model = new t_school {
                     cityId=request.CityId,
                     englishname=request.EnglishName,
                     chinessname=request.ChinessName,
                     isInter=request.IsInter,
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
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.name as ProvinceName,c.name as CityName from t_school a 
                            left join t_province b on a.provinceId=b.id
                            left join t_city c on a.cityId=c.id
                            where isdelete=0");
                if (request.EnglishName.IsNotEmpty())
                {
                    request.EnglishName = "%" + request.EnglishName + "%";
                    sb.Append(" and englishname like @EnglishName");
                }
                if (request.ChinessName.IsNotEmpty())
                {
                    request.ChinessName = "%" + request.ChinessName + "%";
                    sb.Append(" and chinessname like @ChinessName");
                }
                if (request.ProvinceId.HasValue)
                {
                    sb.Append(" and provinceId = @ProvinceId");
                }
                if (request.CityId.HasValue)
                {
                    sb.Append(" and cityId = @CityId");
                }
                list = _dbContext.Page<SchoolResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SchoolService.List", ex);
            }
            return list;
        }
        //学校下拉框
        public List<BaseDataResponse> Select(int? provinceId, int? cityId, string name)
        {
            List<BaseDataResponse> list = new List<BaseDataResponse>();
            try
            {
                StringBuilder sb = new StringBuilder("select id,chinessname as Name from t_school where 1=1 ");
                var dy = new DynamicParameters();
                if (provinceId != null && provinceId > 0)
                {
                    sb.Append(" and provinceId=@provinceId ");
                    dy.Add("provinceId", provinceId);
                }
                if (cityId != null && cityId > 0)
                {
                    sb.Append(" and cityId=@cityId ");
                    dy.Add("cityId", cityId);
                }
                if (name.IsNotEmpty())
                {
                    sb.Append(" and chinessname like @ChinessName");
                    dy.Add("ChinessName", "%" + name + "%");
                }
                list = _dbContext.Query<BaseDataResponse>(sb.ToString(),dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SchoolService.Select", ex);
            }
            return list;
        }
    }
}
