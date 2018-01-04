using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
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
    /// 会员教育经历
    /// </summary>
    public class MemberEduExperService: IMemberEduExperService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MemberEduExperService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 添加教育经历
        public  bool Insert(MemberEduExperRequest request,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.Insert(new t_membereduexper {
                      enddate=request.EndDate,
                      memberId=request.MemberId,
                      schoolId=request.SchoolId,
                      reserveName = request.ReserveName,
                      startdate=request.StartDate
                });
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberEduExperService.Insert", ex);
            }
            return flag;
        }
        //1.1 修改教育经历
        public  bool Edit(MemberEduExperRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var membereduexper = _dbContext.Get<t_membereduexper>(request.Id);
                if (membereduexper != null)
                {
                    membereduexper.schoolId = request.SchoolId;
                    membereduexper.startdate = request.StartDate;
                    membereduexper.enddate = request.EndDate;
                    membereduexper.reserveName = request.ReserveName;
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
                LogUtils.LogError("MemberEduExperService.Edit", ex);
            }
            return flag;
        }
        //1.2 删除教育经历
        public  bool Delete(int id,int userId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var membereduexper = _dbContext.Get<t_membereduexper>(id);
                if (membereduexper != null&&membereduexper.memberId== userId)
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
                LogUtils.LogError("MemberEduExperService.Delete", ex);
            }
            return flag;
        }
        //1.3 教育经历列表
        public PagedList<MemberEduExperResponse> List(MemberEduExperQueryRequest request)
        {
            PagedList<MemberEduExperResponse> list = new PagedList<MemberEduExperResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.chinessname as SchoolName from t_membereduexper a
                            left join t_school b on a.schoolId=b.id
                            where isdelete=0 and memberId=@MemberId");
                list = _dbContext.Page<MemberEduExperResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberEduExperService.List", ex);
            }
            return list;
        }
        //教育经历详情
        public MemberEduExperResponse Detail(int id, int userId)
        {
            MemberEduExperResponse response = null;
            try
            {
                var membereduexper = _dbContext.Get<t_membereduexper>(id);
                if (membereduexper != null&&userId== membereduexper.memberId)
                {
                    response = new MemberEduExperResponse
                    {
                        EndDate=membereduexper.enddate,
                        StartDate=membereduexper.startdate,
                        ReserveName=membereduexper.reserveName,
                        SchoolId=membereduexper.schoolId,
                        Id=membereduexper.id
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberEduExperService.Detail", ex);
            }
            return response;
        }
    }
}
