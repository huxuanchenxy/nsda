using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
using nsda.Services.Contract.admin;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.member
{
    /// <summary>
    /// 选手教育经历
    /// </summary>
    public class PlayerEduExperService: IPlayerEduExperService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IMailService _mailService;
        public PlayerEduExperService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }

        //1.0 添加教育经历
        public  bool Insert(PlayerEduExperRequest request,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.SchoolId <= 0)
                {
                    msg = "请选择学校";
                    return flag;
                }
                _dbContext.Insert(new t_playereduexper {
                      enddate=request.EndDate,
                      memberId=request.MemberId,
                      schoolId=request.SchoolId,
                      startdate=request.StartDate
                });
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerEduExperService.Insert", ex);
            }
            return flag;
        }
        //1.1 修改教育经历
        public  bool Edit(PlayerEduExperRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.SchoolId <= 0)
                {
                    msg = "请选择学校";
                    return flag;
                }
                var membereduexper = _dbContext.Get<t_playereduexper>(request.Id);
                if (membereduexper != null)
                {
                    membereduexper.schoolId = request.SchoolId;
                    membereduexper.startdate = request.StartDate;
                    membereduexper.enddate = request.EndDate;
                    membereduexper.updatetime = DateTime.Now;
                    _dbContext.Update(membereduexper);
                }
                else
                {
                    msg = "教育经历不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerEduExperService.Edit", ex);
            }
            return flag;
        }
        //1.2 删除教育经历
        public  bool Delete(int id,int memberId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var membereduexper = _dbContext.Get<t_playereduexper>(id);
                if (membereduexper != null&&membereduexper.memberId== memberId)
                {
                    membereduexper.updatetime = DateTime.Now;
                    membereduexper.isdelete = true;
                    _dbContext.Update(membereduexper);
                }
                else
                {
                    msg = "教育经历不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerEduExperService.Delete", ex);
            }
            return flag;
        }
        //1.3 教育经历列表
        public List<PlayerEduExperResponse> List(PlayerEduExperQueryRequest request)
        {
            List<PlayerEduExperResponse> list = new List<PlayerEduExperResponse>();
            try
            {
                var sql= @"select a.*,b.chinessname as SchoolName from t_playereduexper a
                            left join t_school b on a.schoolId=b.id
                            where isdelete=0 and memberId=@MemberId order by a.createtime desc ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerEduExperResponse>(sql,out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerEduExperService.List", ex);
            }
            return list;
        }
        //教育经历详情
        public PlayerEduExperResponse Detail(int id, int userId)
        {
            PlayerEduExperResponse response = null;
            try
            {
                var membereduexper = _dbContext.Get<t_playereduexper>(id);
                if (membereduexper != null&&userId== membereduexper.memberId)
                {
                    response = new PlayerEduExperResponse
                    {
                        EndDate=membereduexper.enddate,
                        StartDate=membereduexper.startdate,
                        SchoolId=membereduexper.schoolId,
                        Id=membereduexper.id
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerEduExperService.Detail", ex);
            }
            return response;
        }
    }
}
