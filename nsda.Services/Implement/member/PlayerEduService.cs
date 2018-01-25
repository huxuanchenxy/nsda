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
    public class PlayerEduService: IPlayerEduService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IMailService _mailService;
        public PlayerEduService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }

        //1.0 添加教育经历
        public  bool Insert(PlayerEduRequest request,out string msg)
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
                if (request.StartDate.IsEmpty())
                {
                    msg = "请选择开始时间";
                    return flag;
                }
                _dbContext.Insert(new t_player_edu {
                      enddate=request.EndDate,
                      memberId=request.MemberId,
                      schoolId=request.SchoolId,
                      startdate=request.StartDate
                });
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerEduService.Insert", ex);
            }
            return flag;
        }
        //1.1 修改教育经历
        public  bool Edit(PlayerEduRequest request, out string msg)
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
                if (request.StartDate.IsEmpty())
                {
                    msg = "请选择开始时间";
                    return flag;
                }
                var memberedu = _dbContext.Get<t_player_edu>(request.Id);
                if (memberedu != null)
                {
                    memberedu.schoolId = request.SchoolId;
                    memberedu.startdate = request.StartDate;
                    memberedu.enddate = request.EndDate;
                    memberedu.updatetime = DateTime.Now;
                    _dbContext.Update(memberedu);
                    flag = true;
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
                LogUtils.LogError("PlayerEduService.Edit", ex);
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
                var memberedu = _dbContext.Get<t_player_edu>(id);
                if (memberedu != null&& memberedu.memberId== memberId)
                {
                    memberedu.updatetime = DateTime.Now;
                    memberedu.isdelete = true;
                    _dbContext.Update(memberedu);
                    flag = true;
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
                LogUtils.LogError("PlayerEduService.Delete", ex);
            }
            return flag;
        }
        //1.3 教育经历列表
        public List<PlayerEduResponse> List(PlayerEduExperQueryRequest request)
        {
            List<PlayerEduResponse> list = new List<PlayerEduResponse>();
            try
            {
                var sql= @"select a.*,b.chinessname as SchoolName from t_player_edu a
                            inner  join t_sys_school b on a.schoolId=b.id
                            where a.isdelete=0 and a.memberId=@MemberId order by a.startdate desc ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerEduResponse>(sql,out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerEduService.List", ex);
            }
            return list;
        }
        //教育经历详情
        public PlayerEduResponse Detail(int id, int memberId)
        {
            PlayerEduResponse response = null;
            try
            {
                var memberedu = _dbContext.Get<t_player_edu>(id);
                if (memberedu != null&& memberId == memberedu.memberId)
                {
                    response = new PlayerEduResponse
                    {
                        EndDate= memberedu.enddate,
                        StartDate= memberedu.startdate,
                        SchoolId= memberedu.schoolId,
                        Id= memberedu.id
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerEduService.Detail", ex);
            }
            return response;
        }
    }
}
