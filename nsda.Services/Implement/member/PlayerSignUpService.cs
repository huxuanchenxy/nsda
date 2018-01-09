using nsda.Repository;
using nsda.Services.Contract.member;
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

namespace nsda.Services.Implement.member
{
    public class PlayerSignUpService : IPlayerSignUpService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public PlayerSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //发起组队
        public bool Insert(PlayerSignUpRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //逻辑校验
                _dbContext.BeginTransaction();
                t_event tevent = _dbContext.Get<t_event>(request.EventId);

                string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                //邀请者
                _dbContext.Insert(new t_player_signup
                {
                    eventId = request.EventId,
                    groupId = request.GroupId,
                    groupnum = groupnum,
                    memberId = request.FromMemberId,
                    signfee = tevent.signfee,
                    signUpStatus = SignUpStatusEm.等待队员确认邀请,
                    signUpType = SignUpTypeEm.邀请人,
                    isTemp=false
                });
                //被邀请者
                _dbContext.Insert(new t_player_signup
                {
                    eventId = request.EventId,
                    groupId = request.GroupId,
                    groupnum = groupnum,
                    memberId = request.ToMemberId,
                    signfee = tevent.signfee,
                    signUpStatus = SignUpStatusEm.报名邀请中,
                    signUpType = SignUpTypeEm.被邀请人,
                    isTemp = false
                });
                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.Insert", ex);
            }
            return flag;
        }
        //是否接受组队
        public bool IsAcceptTeam(int id, int memberId, bool isAgree, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.IsAcceptTeam", ex);
            }
            return flag;
        }
        //替换队友
        public bool ReplaceTeammate(int id, int memberId,int newMemberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.ReplaceTeammate", ex);
            }
            return flag;
        }
        //去支付
        public bool GoPay(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.GoPay", ex);
            }
            return flag;
        }
        //申请退赛
        public bool Cancel(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.Cancel", ex);
            }
            return flag;
        }
        //确认退赛
        public bool ConfirmCancel(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.ConfirmCancel", ex);
            }
            return flag;
        }
        //比赛列表
        public PagedList<PlayerSignUpResponse> List(PlayerSignUpQueryRequest request)
        {
            PagedList<PlayerSignUpResponse> list = new PagedList<PlayerSignUpResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.List", ex);
            }
            return list;
        }
        //支付回调
        public void Callback(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Callback", ex);
            }
        }
    }
}
