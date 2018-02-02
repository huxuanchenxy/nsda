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
                var playerEdu = _dbContext.Get<t_player_edu>(request.Id);
                if (playerEdu != null)
                {
                    playerEdu.schoolId = request.SchoolId;
                    playerEdu.startdate = request.StartDate;
                    playerEdu.enddate = request.EndDate;
                    playerEdu.updatetime = DateTime.Now;
                    _dbContext.Update(playerEdu);
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
                var playerEdu = _dbContext.Get<t_player_edu>(id);
                if (playerEdu != null&& playerEdu.memberId== memberId)
                {
                    playerEdu.updatetime = DateTime.Now;
                    playerEdu.isdelete = true;
                    _dbContext.Update(playerEdu);
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
                var sql= @" select a.*,b.chinessname  SchoolName,c.name CityName,d.name ProvinceName  from t_player_edu a
                            inner  join t_sys_school b on a.schoolId=b.id
                            inner  join t_sys_city c on b.cityId=c.id
                            inner  join t_sys_province d on b.provinceId=d.id
                            where a.isdelete=0 and a.memberId=@MemberId order by a.startdate desc ";
                list = _dbContext.Query<PlayerEduResponse>(sql, request).ToList();
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
                var sql = $@"select a.*,b.cityId,b.provinceId  from t_player_edu a
                            inner join t_sys_school b on a.schoolId = b.id
                            where a.isdelete = 0 and a.id = {id} and a.memberId={memberId}";
                var playerEdu = _dbContext.QueryFirstOrDefault<dynamic>(sql);
                if (playerEdu != null)
                {
                    response = new PlayerEduResponse
                    {
                        EndDate= playerEdu.enddate,
                        StartDate= playerEdu.startdate,
                        SchoolId= playerEdu.schoolId,
                        ProvinceId= playerEdu.provinceId,
                        CityId= playerEdu.cityId,
                        Id = playerEdu.id
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
