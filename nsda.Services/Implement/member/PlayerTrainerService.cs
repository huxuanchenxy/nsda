using nsda.Repository;
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
using nsda.Model.enums;
using nsda.Services.Contract.member;

namespace nsda.Services.member
{
    /// <summary>
    /// 选手教练管理
    /// </summary>
    public class PlayerTrainerService: IPlayerTrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public PlayerTrainerService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public bool Insert(PlayerTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ObjMemberId <= 0)
                {
                    msg = "教练不能为空";
                    return flag;
                }
                if (request.StartDate==DateTime.MinValue||request.StartDate==DateTime.MaxValue||request.StartDate>DateTime.Now)
                {
                    msg = "开始时间有误";
                    return flag;
                }
                var memberTrainer = new t_player_trainer
                {
                    objMemberId = request.ObjMemberId,
                    startdate = request.StartDate,
                    enddate = request.EndDate,
                    IsPositive=request.IsPositive,
                    IsTrainer=request.IsTrainer,
                    memberTrainerStatus = MemberTrainerStatusEm.待确认,
                };
                _dbContext.Insert(memberTrainer);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerTrainerService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(PlayerTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ObjMemberId <= 0)
                {
                    msg = "教练不能为空";
                    return flag;
                }
                if (request.StartDate == DateTime.MinValue || request.StartDate == DateTime.MaxValue || request.StartDate > DateTime.Now)
                {
                    msg = "开始时间有误";
                    return flag;
                }
                var memberTrainer = _dbContext.Get<t_player_trainer>(request.Id);
                if (memberTrainer != null&&memberTrainer.memberId==request.MemberId)
                {
                    memberTrainer.objMemberId = request.ObjMemberId;
                    memberTrainer.startdate = request.StartDate;
                    memberTrainer.enddate = request.EndDate;
                    memberTrainer.memberTrainerStatus =  MemberTrainerStatusEm.待确认;
                    memberTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(memberTrainer);
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
                LogUtils.LogError("PlayerTrainerService.Edit", ex);
            }
            return flag;
        }

        public bool Delete(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var memberTrainer = _dbContext.Get<t_player_trainer>(id);
                if (memberTrainer != null&&memberTrainer.memberId==memberId)
                {     
                    memberTrainer.isdelete = true;
                    memberTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(memberTrainer);
                }
                else {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerTrainerService.Delete", ex);
            }
            return flag;
        }

        public bool IsAppro(int id, int memberId,bool isAppro, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {   
                var memberTrainer = _dbContext.Get<t_player_trainer>(id);
                if (memberTrainer != null&& memberTrainer.objMemberId==memberId)
                {           
                    memberTrainer.memberTrainerStatus = isAppro ? MemberTrainerStatusEm.已确认 : MemberTrainerStatusEm.拒绝;
                    memberTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(memberTrainer);
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
                LogUtils.LogError("PlayerTrainerService.IsAppro", ex);
            }
            return flag;
        }

        public PagedList<PlayerTrainerResponse> TrainerList(PlayerTrainerQueryRequest request)
        {
            PagedList<PlayerTrainerResponse> list = new PagedList<PlayerTrainerResponse>();
            try
            {
                var sql = "select * from t_player_trainer where IsTrainer=1 and ( (IsPositive=0 and memberId=@MemberId) or (IsPositive=1 and ObjMemberId=@MemberId))";
                list = _dbContext.Page<PlayerTrainerResponse>(sql, request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerTrainerService.TrainerList", ex);
            }
            return list;
        }

        public PagedList<PlayerTrainerResponse> MemberList(PlayerTrainerQueryRequest request)
        {
            PagedList< PlayerTrainerResponse > list = new PagedList<PlayerTrainerResponse>();
            try
            {
                var sql = "select * from t_player_trainer where IsTrainer=0 and ( (IsPositive=0 and memberId=@MemberId) or (IsPositive=1 and ObjMemberId=@MemberId))";
                list = _dbContext.Page<PlayerTrainerResponse>(sql, request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerTrainerService.TrainerList", ex);
            }
            return list;
        }
    }
}
