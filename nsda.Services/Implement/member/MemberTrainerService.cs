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

namespace nsda.Services.member
{
    /// <summary>
    /// 会员教练管理
    /// </summary>
    public class MemberTrainerService: IMemberTrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MemberTrainerService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        public bool Insert(MemberTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var memberTrainer = new t_member_trainer();
                memberTrainer.objMemberId = request.ObjMemberId;
                memberTrainer.startdate = request.StartDate;
                memberTrainer.enddate = request.EndDate;
                memberTrainer.memberTrainerStatus = MemberTrainerStatusEm.待确认;
                memberTrainer.updatetime = DateTime.Now;
                _dbContext.Insert(memberTrainer);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberTrainerService.Insert", ex);
            }
            return flag;
        }

        public bool Edit(MemberTrainerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var memberTrainer = _dbContext.Get<t_member_trainer>(request.Id);
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
                LogUtils.LogError("MemberTrainerService.Edit", ex);
            }
            return flag;
        }

        public bool Delete(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var memberTrainer = _dbContext.Get<t_member_trainer>(id);
                if (memberTrainer != null)
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
                LogUtils.LogError("MemberTrainerService.Delete", ex);
            }
            return flag;
        }

        public bool IsAppro(int id, int memberId,bool isAppro, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var memberTrainer = _dbContext.Get<t_member_trainer>(id);
                if (memberTrainer != null)
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
                LogUtils.LogError("MemberTrainerService.IsAppro", ex);
            }
            return flag;
        }

        public PagedList<MemberTrainerResponse> TrainerList(MemberTrainerQueryRequest request)
        {
            PagedList<MemberTrainerResponse> list = new PagedList<MemberTrainerResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTrainerService.TrainerList", ex);
            }
            return list;
        }

        public PagedList<MemberTrainerResponse> MemberList(MemberTrainerQueryRequest request)
        {
            PagedList< MemberTrainerResponse > list = new PagedList<MemberTrainerResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTrainerService.TrainerList", ex);
            }
            return list;
        }
    }
}
