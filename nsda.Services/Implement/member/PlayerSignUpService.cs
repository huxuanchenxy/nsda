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
    /// <summary>
    /// 选手报名管理
    /// </summary>
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

        //所有方法考虑 辩论和演讲

        //发起组队
        public bool Insert(PlayerSignUpRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //逻辑校验
                //1.0 报名者是否重复报名 是否有资格报名
                //2.0 被邀请者是否有资格报名 是否重复报名
                //3.0 报名队伍数是否达到上限
                t_event tevent = _dbContext.Get<t_event>(request.EventId);

                _dbContext.BeginTransaction();
                string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                if (request.EventType == EventTypeEm.辩论)
                {
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
                        isTemp = false
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
                }
                else //演讲逻辑可能不同
                {
                    //邀请者
                    _dbContext.Insert(new t_player_signup
                    {
                        eventId = request.EventId,
                        groupId = request.GroupId,
                        groupnum = groupnum,
                        memberId = request.FromMemberId,
                        signfee = tevent.signfee,
                        signUpStatus = SignUpStatusEm.报名成功,
                        signUpType = SignUpTypeEm.邀请人,
                        isTemp = false
                    });
                }
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
                _dbContext.BeginTransaction();
                var playsignup = _dbContext.Get<t_player_signup>(id);
                if (playsignup != null)
                {
                    var otherSignUp = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.memberId != memberId).FirstOrDefault();

                    if (isAgree)//确认组队
                    {
                        playsignup.signUpStatus = SignUpStatusEm.组队成功;
                        otherSignUp.signUpStatus = SignUpStatusEm.组队成功;
                    }
                    else//拒绝组队
                    {
                        playsignup.signUpStatus = SignUpStatusEm.组队失败;
                        otherSignUp.signUpStatus = SignUpStatusEm.组队失败;
                    }
                    playsignup.updatetime = DateTime.Now;
                    otherSignUp.updatetime = DateTime.Now;
                    _dbContext.Update(playsignup);
                    _dbContext.Update(otherSignUp);
                }
                else
                {
                    msg = "队伍信息不存在";
                }
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.IsAcceptTeam", ex);
            }
            return flag;
        }
        //替换队友
        public bool ReplaceTeammate(int id, int memberId, int newMemberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playsignup = _dbContext.Get<t_player_signup>(id);
                if (playsignup != null)
                {
                   var otherSignUp = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.memberId != memberId).FirstOrDefault();
                    otherSignUp.updatetime = DateTime.Now;
                    otherSignUp.memberId = newMemberId;
                   _dbContext.Update(otherSignUp);
                }
                else
                {
                    msg = "队伍信息不存在";
                }
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
                //判断是否有订单 无订单则创建订单并生成支付信息
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
        public bool ApplyRetire(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playsignup = _dbContext.Get<t_player_signup>(id);
                if (playsignup != null)
                {
                    //获取赛事信息
                    t_event t_event = _dbContext.Get<t_event>(playsignup.eventId);
                    if (playsignup.signUpStatus == SignUpStatusEm.组队成功)//未支付
                    {
                        playsignup.signUpStatus = SignUpStatusEm.等待队友确认退赛;
                        playsignup.updatetime = DateTime.Now;
                        _dbContext.Update(playsignup);
                    }
                    else if (playsignup.signUpStatus == SignUpStatusEm.报名成功)//已支付
                    {
                        var data = _dbContext.Select<t_player_signup>(c=>c.groupnum==playsignup.groupnum&&c.memberId!=memberId).ToList();
                        if (data == null || data.Count == 0)
                        {
                            playsignup.signUpStatus = SignUpStatusEm.退费申请中;
                            playsignup.updatetime = DateTime.Now;
                            _dbContext.Update(playsignup);
                        }
                        else {
                            playsignup.signUpStatus = SignUpStatusEm.等待队友确认退赛;
                            playsignup.updatetime = DateTime.Now;
                            _dbContext.Update(playsignup);
                        }
                    }
                    else {
                        msg = "状态已改变 请刷新页面后重试";
                    }
                }
                else
                {
                    msg = "报名信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.ApplyRetire", ex);
            }
            return flag;
        }
        //确认退赛
        public bool ConfirmRetire(int id, int memberId, out string msg)
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
                LogUtils.LogError("SignUpPlayerService.ConfirmRetire", ex);
            }
            return flag;
        }
        //选手报名列表
        public PagedList<PlayerSignUpResponse> List(PlayerSignUpQueryRequest request)
        {
            PagedList<PlayerSignUpResponse> list = new PagedList<PlayerSignUpResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();

                if (request.EventStartDate.HasValue)
                {
                    sb.Append(" and b.starteventdate>@EventStartDate ");
                }
                if (request.EventEndDate.HasValue)
                {
                    request.EventEndDate = request.EventEndDate.Value.AddDays(1).AddSeconds(-1);
                    sb.Append(" and b.endeventdate<@EventEndDate ");
                }
                if (request.CountryId.HasValue)
                {
                    sb.Append(" and b.countryId=@CountryId ");
                }
                if (request.ProvinceId.HasValue)
                {
                    sb.Append(" and b.provinceId=@ProvinceId ");
                }
                if (request.CityId.HasValue)
                {
                    sb.Append(" and b.cityId=@CityId ");
                }
                list = _dbContext.Page<PlayerSignUpResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
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
                var signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null)
                {

                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Callback", ex);
            }
        }
    }
}
