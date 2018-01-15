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
using nsda.Services.Contract.admin;

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
        IMailService _mailService;
        public PlayerSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }

        //发起组队
        public bool Insert(PlayerSignUpRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(request.EventId);
                if (tevent.eventType == EventTypeEm.辩论)
                {
                    //逻辑校验
                    //1.0 报名者是否重复报名 是否有资格报名
                    //2.0 被邀请者是否有资格报名 是否重复报名
                    //3.0 报名队伍数是否达到上限
                    _dbContext.BeginTransaction();
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
                    string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                    //邀请者
                    _dbContext.Insert(new t_player_signup
                    {
                        eventId = request.EventId,
                        groupId = request.GroupId,
                        groupnum = groupnum,
                        memberId = request.FromMemberId,
                        signfee = tevent.signfee,
                        signUpStatus = tevent.signfee <= 0 ? SignUpStatusEm.报名成功 : SignUpStatusEm.组队成功,
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
        public bool IsAcceptTeam(int id, bool isAgree, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.BeginTransaction();
                var playsignup = _dbContext.Get<t_player_signup>(id);
                if (playsignup != null)
                {
                    var tevent = _dbContext.Get<t_event>(playsignup.eventId);
                    if (tevent.eventType == EventTypeEm.辩论)
                    {
                        var otherSignUp = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.memberId != memberId).FirstOrDefault();

                        //确认组队时 需要判断两位用户原有状态
                        if (isAgree)//确认组队
                        {
                            playsignup.signUpStatus = tevent.signfee > 0 ? SignUpStatusEm.组队成功 : SignUpStatusEm.报名成功;
                            otherSignUp.signUpStatus = tevent.signfee > 0 ? SignUpStatusEm.组队成功 : SignUpStatusEm.报名成功;
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
                        msg = "操作有误";
                    }
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
        public bool ReplaceTeammate(int id, int newMemberId, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playsignup = _dbContext.Get<t_player_signup>(id);
                if (playsignup != null)
                {
                    var tevent = _dbContext.Get<t_event>(playsignup.eventId);
                    if (tevent.eventType == EventTypeEm.辩论)
                    {
                        //新增队伍
                        //var otherSignUp = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.memberId != memberId).FirstOrDefault();
                        //otherSignUp.updatetime = DateTime.Now;
                        //otherSignUp.memberId = newMemberId;
                        //_dbContext.Update(otherSignUp);
                    }
                    else
                    {
                        msg = "操作有误";
                    }
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
                t_player_signup signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null)
                {
                    //判断是否有订单 无订单则创建订单并生成支付信息
                    t_order order = _dbContext.Select<t_order>(c => c.orderType == OrderTypeEm.赛事报名 && c.memberId == memberId && c.sourceId == id).FirstOrDefault();
                    if (order == null)
                    {
                        t_event t_event = _dbContext.Get<t_event>(signup.eventId);
                        try
                        {
                            _dbContext.BeginTransaction();
                            //创建订单
                            var orderid = _dbContext.Insert(new t_order
                            {
                                isNeedInvoice = false,
                                mainOrderId = null,
                                memberId = memberId,
                                money = t_event.signfee,
                                orderStatus = OrderStatusEm.等待支付,
                                orderType = OrderTypeEm.赛事报名,
                                payExpiryDate = t_event.starteventdate,
                                remark = "赛事报名",
                                sourceId = signup.id,
                                totalcoupon = 0,
                                totaldiscount = 0
                            }).ToObjInt();
                            _dbContext.Insert(new t_orderdetail
                            {
                                memberId = memberId,
                                orderId = orderid,
                                coupon = 0,
                                discountprice = 0,
                                money = t_event.signfee,
                                productId = 0,
                                name = $"{t_event.name}报名费",
                                number = 1,
                                unitprice = t_event.signfee
                            });
                            _dbContext.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            _dbContext.Rollback();
                            flag = false;
                            msg = "服务异常";
                            LogUtils.LogError("SignUpPlayerService.GoPayTran", ex);
                        }
                    }
                    else
                    {
                        //获取支付信息
                    }
                }
                else {
                    msg = "报名信息有误";
                }
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
                    t_event tevent = _dbContext.Get<t_event>(playsignup.eventId);
                    if (tevent.eventType == EventTypeEm.辩论)
                    {
                        if (playsignup.signUpStatus == SignUpStatusEm.组队成功)//未支付
                        {
                            playsignup.signUpStatus = SignUpStatusEm.等待队友确认退赛;
                            playsignup.updatetime = DateTime.Now;
                            _dbContext.Update(playsignup);
                        }
                        else if (playsignup.signUpStatus == SignUpStatusEm.报名成功)//已支付
                        {
                            var data = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.memberId != memberId).ToList();
                            if (data == null || data.Count == 0)
                            {
                                playsignup.signUpStatus = SignUpStatusEm.退费申请中;
                                playsignup.updatetime = DateTime.Now;
                                _dbContext.Update(playsignup);
                            }
                            else
                            {
                                playsignup.signUpStatus = SignUpStatusEm.等待队友确认退赛;
                                playsignup.updatetime = DateTime.Now;
                                _dbContext.Update(playsignup);
                            }
                        }
                        else
                        {
                            msg = "状态已改变 请刷新页面后重试";
                        }
                    }
                    else {
                        playsignup.signUpStatus = SignUpStatusEm.退费申请中;
                        playsignup.updatetime = DateTime.Now;
                        _dbContext.Update(playsignup);
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
                t_player_signup signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null)
                {

                }
                else
                {
                    msg = "赛事信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.ConfirmRetire", ex);
            }
            return flag;
        }
        //支付回调
        public void Callback(int memberId,int sourceId)
        {
            try
            {
                var signup = _dbContext.Get<t_player_signup>(sourceId);
                if (signup != null)
                {
                    signup.updatetime = DateTime.Now;
                    signup.signUpStatus = SignUpStatusEm.报名成功;
                    _dbContext.Update(signup);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Callback", ex);
            }
        }
        //审核退赛
        public bool CheckRetire(int id,bool isAppro,int memberId,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_player_signup signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null)
                {
                    signup.updatetime = DateTime.Now;
                    signup.signUpStatus = isAppro ? SignUpStatusEm.退费中 : SignUpStatusEm.拒绝退赛;
                    _dbContext.Update(signup);
                }
                else
                {
                    msg = "赛事信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.CheckRetire", ex);
            }
            return flag;
        }
        //当前赛事
        public List<CurrentEventResponse> CurrentPlayerEvent(int memberId)
        {
            List<CurrentEventResponse> list = new List<CurrentEventResponse>();
            try
            {
                var sql = $@"select a.* from t_player_signup a
                             inner join t_event b on a.eventId=b.id
                             where  a.isdelete=0 and (b.starteventdate={DateTime.Now.ToShortDateString()} or b.endeventdate={DateTime.Now.ToShortDateString()}) and a.memberId={memberId}";
                list = _dbContext.Query<CurrentEventResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.CurrentPlayerEvent", ex);
            }
            return list;
        }
        //选手报名列表
        public List<PlayerSignUpListResponse> EventPlayerList(PlayerSignUpQueryRequest request)
        {
            List<PlayerSignUpListResponse> list = new List<PlayerSignUpListResponse>();
            try
            {
                StringBuilder sb = new StringBuilder($@"select a.*,b.code MemberCode,b.completename MemberName,a.grade,a.gender,a.contactmobile from t_player_signup a 
                                                      inner join t_member b on a.memberId=b.id
                                                      inner join t_event c on a.eventId=c.id
                                                      where a.isdelete=0 and b.isdelete=0 and c.isdelete=0 
                                                      and c.memberId=@MemberId and a.eventId=@EventId 
                                                     ");

                if (request.KeyValue.IsEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    sb.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
                }
                int totalCount = 0;
                list = _dbContext.Page<PlayerSignUpListResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.EventPlayerList", ex);
            }
            return list;
        }

        public List<MemberSelectResponse> Invitation(string keyvalue, int eventId, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                var sql= $@"";
                list = _dbContext.Query<MemberSelectResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Invitation", ex);
            }
            return list;
        }
    }
}
