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
using nsda.Services.Contract.admin;

namespace nsda.Services.member
{
    /// <summary>
    /// 选手教练绑定管理
    /// </summary>
    public class PlayerTrainerService: IPlayerTrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        IMailService _mailService;
        public PlayerTrainerService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }
        //绑定教练/学生
        public bool Insert(PlayerTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ObjMemberId <= 0)
                {
                    if (request.IsTrainer)
                    {
                        msg = "教练不能为空";
                        return flag;
                    }
                    else
                    {
                        msg = "学生不能为空";
                        return flag;
                    }
                }
                if (request.StartDate==DateTime.MinValue||request.StartDate==DateTime.MaxValue||request.StartDate>DateTime.Now)
                {
                    msg = "开始时间有误";
                    return flag;
                }

                var playerTrainer = new t_player_trainer
                {
                    memberId = request.MemberId,
                    toMemberId = request.ObjMemberId,
                    startdate = request.StartDate,
                    enddate = request.EndDate,
                    isPositive=request.IsPositive,
                    isTrainer=request.IsTrainer,
                    playerTrainerStatus = PlayerTrainerStatusEm.待确认,
                };
                _dbContext.Insert(playerTrainer);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("PlayerTrainerService.Insert", ex);
            }
            return flag;
        }
        //编辑绑定教练/学生
        public bool Edit(PlayerTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ObjMemberId <= 0)
                {
                    if (request.IsTrainer)
                    {
                        msg = "教练不能为空";
                        return flag;
                    }
                    else
                    {
                        msg = "学生不能为空";
                        return flag;
                    }
                }
                if (request.StartDate == DateTime.MinValue || request.StartDate == DateTime.MaxValue || request.StartDate > DateTime.Now)
                {
                    msg = "开始时间有误";
                    return flag;
                }
                var playerTrainer = _dbContext.Get<t_player_trainer>(request.Id);
                if (playerTrainer != null&& playerTrainer.memberId==request.MemberId)
                {
                    playerTrainer.startdate = request.StartDate;
                    playerTrainer.enddate = request.EndDate;
                    playerTrainer.playerTrainerStatus =  PlayerTrainerStatusEm.待确认;
                    playerTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(playerTrainer);
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
        //删除
        public bool Delete(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playerTrainer = _dbContext.Get<t_player_trainer>(id);
                if (playerTrainer != null&& playerTrainer.memberId==memberId)
                {
                    playerTrainer.isdelete = true;
                    playerTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(playerTrainer);
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
        //是否同意 教练或者学生的申请
        public bool IsAppro(int id, int memberId,bool isAppro, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {   
                var playerTrainer = _dbContext.Get<t_player_trainer>(id);
                if (playerTrainer != null&& playerTrainer.toMemberId==memberId)
                {
                    playerTrainer.playerTrainerStatus = isAppro ? PlayerTrainerStatusEm.已确认 : PlayerTrainerStatusEm.拒绝;
                    playerTrainer.updatetime = DateTime.Now;
                    _dbContext.Update(playerTrainer);
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
        //教练下的学生列表
        public List<PlayerTrainerResponse> TrainerList(PlayerTrainerQueryRequest request)
        {
            List<PlayerTrainerResponse> list = new List<PlayerTrainerResponse>();
            try
            {
                var sql = @"select a.*,b.name MemberName,c.name ToMemberName  from t_player_trainer a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_member c on a.toMemberId=c.id
                            where (isTrainer=1 and isPositive=0 and memberId=@MemberId) or (isTrainer=0 and isPositive=1 and toMemberId=@MemberId)";
                int totalCount = 0;
                list = _dbContext.Page<PlayerTrainerResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
                //要查询执教期间所获的积分数
                if (list != null && list.Count>0)
                {
                    foreach (var item in list)
                    {
                        if (item.MemberId == request.MemberId)
                        {
                            item.Flag = true;
                        }
                        if (item.PlayerTrainerStatus == PlayerTrainerStatusEm.已确认)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerTrainerService.TrainerList", ex);
            }
            return list;
        }
        //学生下的教练列表
        public List<PlayerTrainerResponse> MemberList(PlayerTrainerQueryRequest request)
        {
            List< PlayerTrainerResponse > list = new List<PlayerTrainerResponse>();
            try
            {
                var sql = @"select a.*,b.name MemberName,c.name ToMemberName from t_player_trainer a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_member c on a.toMemberId=c.id
                            where  (isTrainer=0 and isPositive=1 and memberId=@MemberId) or (isTrainer=1 and isPositive=0 and toMemberId=@MemberId)";
                int totalCount = 0;
                list = _dbContext.Page<PlayerTrainerResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
                if (list != null && list.Count>0)
                {
                    foreach (var item in list)
                    {
                        if (item.MemberId == request.MemberId)
                        {
                            item.Flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("PlayerTrainerService.TrainerList", ex);
            }
            return list;
        }
    }
}
