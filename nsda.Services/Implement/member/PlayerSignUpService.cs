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
using Dapper;
using nsda.Model;

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
        public PlayerSignUpService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService, IMailService mailService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _mailService = mailService;
        }
        //获取用户可报名的组别
        public List<EventGroupResponse> EventGroup(int eventId, int memberId)
        {
            List<EventGroupResponse> list = new List<EventGroupResponse>();
            try
            {
                List<t_event_group> listgroup = _dbContext.Select<t_event_group>(c => c.eventId == eventId).ToList();
                t_member_player member = _dbContext.Get<t_member_player>(memberId);
                foreach (var item in listgroup)
                {
                    if (IsValid(item, member))
                    {
                        list.Add(new EventGroupResponse
                        {
                            EventId = eventId,
                            Id = item.id,
                            MaxGrade = item.maxgrade,
                            MaxTimes = item.maxtimes,
                            MinGrade = item.mingrade,
                            MinTimes = item.mintimes,
                            Name = item.name,
                            TeamNumber = item.teamnumber
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Invitation", ex);
            }
            return list;
        }
        //邀请队友
        public List<MemberSelectResponse> Invitation(string keyvalue, int eventId, int groupId, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                //需要过滤已经报名的选手
                var sql = $@"select a.* from t_member_player a inner join t_member b on a.memberId=b.id                            
                            where a.isdelete=0  and (b.memberType={(int)MemberTypeEm.选手} or b.isExtendPlayer=1) and a.memberId!={memberId}
                            and b.memberStatus={(int)MemberStatusEm.已认证} and (a.code=@key or a.completename=@key)
                            and a.memberId not in (select memberId from t_event_player_signup 
                                                   where isdelete=0 and signUpStatus not in ({ParamsConfig._signup_notin})
                                                  ) 
                            limit 30
                         ";
                var dy = new DynamicParameters();
                dy.Add("key", keyvalue);
                var data = _dbContext.Query<t_member_player>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    t_event_group group = _dbContext.Get<t_event_group>(groupId);
                    foreach (var item in data)
                    {
                        //需要判断选手是否满足条件
                        if (IsValid(group, item))
                        {
                            list.Add(new MemberSelectResponse
                            {
                                MemberId = item.memberId,
                                MemberCode = item.code,
                                MemberName = item.completename
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Invitation", ex);
            }
            return list;
        }
        //发起组队
        public bool Insert(PlayerSignUpRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event tevent = _dbContext.Get<t_event>(request.EventId);
                if (tevent == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }

                if (tevent.eventStatus != EventStatusEm.报名中)
                {
                    msg = "当前不允许选手报名";
                    return flag;
                }

                if (tevent.eventType == EventTypeEm.辩论)
                {
                    #region 辩论
                    //1.0 报名者是否重复报名 
                    var from = _dbContext.ExecuteScalar($"select count(1) from t_event_player_signup where isdelete=0 and memberId={request.FromMemberId} and eventId={request.EventId} and signUpStatus not in ({ParamsConfig._signup_notin})").ToObjInt();
                    if (from > 0)
                    {
                        msg = "您已申请过此次赛事";
                        return flag;
                    }

                    var to = _dbContext.ExecuteScalar($"select count(1) from t_event_player_signup where isdelete=0 and memberId={request.ToMemberId} and eventId={request.EventId} and signUpStatus not in ({ParamsConfig._signup_notin})").ToObjInt();
                    if (to > 0)
                    {
                        msg = "您邀请的队友已申请过此次赛事";
                        return flag;
                    }

                    //2.0 报名队伍数是否达到上限
                    var teamcount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={request.EventId}").ToObjInt();
                    if (tevent.maxnumber < teamcount + 1)
                    {
                        msg = "达到报名人数上限无法继续报名";
                        return flag;
                    }

                    t_event_group eventGroup = _dbContext.Get<t_event_group>(request.EventGroupId);
                    if (eventGroup == null)
                    {
                        msg = "赛事组别信息有误";
                        return flag;
                    }

                    if (eventGroup.teamnumber > 1)
                    {
                        if (request.ToMemberId <= 0)
                        {
                            msg = "请选择您的队友";
                            return flag;
                        }
                    }
                    t_member_player frommember = _dbContext.Select<t_member_player>(c=>c.memberId==request.FromMemberId).FirstOrDefault();
                    //3.0 是否有资格报名
                    if (!IsValid(eventGroup, frommember))
                    {
                        msg = "您不符合此赛事报名规则";
                        return flag;
                    }
                    t_member_player tomember = _dbContext.Select<t_member_player>(c=>c.memberId==request.ToMemberId).FirstOrDefault();
                    if (!IsValid(eventGroup, tomember))
                    {
                        msg = "您队友不符合此赛事报名规则";
                        return flag;
                    }

                    try
                    {
                        _dbContext.BeginTransaction();
                        string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode(tevent.id);
                        //邀请者
                        _dbContext.Insert(new t_event_player_signup
                        {
                            eventId = request.EventId,
                            eventGroupId = request.EventGroupId,
                            groupnum = groupnum,
                            memberId = request.FromMemberId,
                            signfee = tevent.signfee,
                            signUpStatus = SignUpStatusEm.报名邀请中,
                            signUpType = SignUpTypeEm.邀请人,
                            isTemp = false
                        });
                        //被邀请者
                        _dbContext.Insert(new t_event_player_signup
                        {
                            eventId = request.EventId,
                            eventGroupId = request.EventGroupId,
                            groupnum = groupnum,
                            memberId = request.ToMemberId,
                            signfee = tevent.signfee,
                            signUpStatus = SignUpStatusEm.报名邀请中,
                            signUpType = SignUpTypeEm.被邀请人,
                            isTemp = false
                        });

                        _dbContext.Insert(new t_sys_mail
                        {
                            title = $"{frommember.completename} 向您发出组队申请",
                            content = $"{frommember.completename} 向您发出组队申请。邀请您一同参加<a href=\"/player/playersignup/eventdetail\"> \"{tevent.name}</a>\" {eventGroup.name}组别比赛。请尽快前往处理",
                            isRead = false,
                            mailType = MailTypeEm.赛事报名邀请,
                            memberId = request.ToMemberId,
                            sendMemberId=request.FromMemberId
                        });
                        _dbContext.CommitChanges();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("SignUpPlayerService.InsertTran", ex);
                    }
                    #endregion
                }
                else //演讲逻辑可能不同
                {
                    #region 演讲
                    var listfrom = _dbContext.Select<t_event_player_signup>(c => c.memberId == request.FromMemberId && c.eventId == request.EventId).ToList();
                    if (listfrom != null && listfrom.Count > 0)
                    {
                        msg = "您已申请过此次赛事";
                        return flag;
                    }
                    try
                    {
                        _dbContext.BeginTransaction();
                        string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode(tevent.id);
                        //邀请者
                        _dbContext.Insert(new t_event_player_signup
                        {
                            eventId = request.EventId,
                            eventGroupId = request.EventGroupId,
                            groupnum = groupnum,
                            memberId = request.FromMemberId,
                            signfee = tevent.signfee,
                            signUpStatus = SignUpStatusEm.待付款,
                            signUpType = SignUpTypeEm.邀请人,
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
                        LogUtils.LogError("SignUpPlayerService.InsertTran", ex);
                    }
                    #endregion 
                }
            }
            catch (Exception ex)
            {
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
                var playsignup = _dbContext.Get<t_event_player_signup>(id);
                if (playsignup != null && playsignup.signUpStatus == SignUpStatusEm.报名邀请中)
                {
                    var tevent = _dbContext.Get<t_event>(playsignup.eventId);
                    if (tevent.eventType == EventTypeEm.辩论)
                    {
                        SignUpStatusEm signStatus = SignUpStatusEm.待付款;
                        if (!isAgree)
                        {
                            signStatus = SignUpStatusEm.组队失败;
                        }
                        var sql = $"update t_event_player_signup set signUpStatus={(int)signStatus},updatetime='{DateTime.Now}' where groupnum='{playsignup.groupnum}' and eventId={playsignup.eventId}";
                        _dbContext.Execute(sql);
                        flag = true;
                    }
                    else if (tevent.eventType == EventTypeEm.演讲)
                    {
                        //演讲流程
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
                LogUtils.LogError("SignUpPlayerService.IsAcceptTeam", ex);
            }
            return flag;
        }
        //去支付
        public int GoPay(int id, int memberId, out string msg)
        {
            int orderId = 0;
            msg = string.Empty;
            try
            {
                t_event_player_signup signup = _dbContext.Get<t_event_player_signup>(id);
                if (signup != null && signup.memberId == memberId && signup.signUpStatus == SignUpStatusEm.待付款)
                {
                    t_event tevent = _dbContext.Get<t_event>(signup.eventId);
                    var teamcount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_event_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={signup.eventId}").ToObjInt();
                    if (tevent.maxnumber < teamcount + 1)
                    {
                        msg = "达到报名人数上限无法继续去支付";
                        return orderId;
                    }
                    //判断是否有订单 无订单则创建订单并生成支付信息
                    t_order order = _dbContext.Select<t_order>(c => c.orderType == OrderTypeEm.赛事报名 && c.memberId == memberId && c.sourceId == id).FirstOrDefault();
                    if (order == null)
                    {
                        try
                        {
                            _dbContext.BeginTransaction();
                            //创建订单
                            var orderid = _dbContext.Insert(new t_order
                            {
                                isNeedInvoice = false,
                                mainOrderId = null,
                                memberId = memberId,
                                money = tevent.signfee,
                                orderStatus = OrderStatusEm.等待支付,
                                orderType = OrderTypeEm.赛事报名,
                                payExpiryDate = tevent.starteventdate,
                                remark = $"{tevent.name}赛事报名",
                                sourceId = signup.id,
                                totalcoupon = 0,
                                totaldiscount = 0
                            }).ToObjInt();
                            _dbContext.Insert(new t_order_detail
                            {
                                memberId = memberId,
                                orderId = orderid,
                                coupon = 0,
                                discountprice = 0,
                                money = tevent.signfee,
                                productId = 0,
                                name = $"{tevent.name}报名费",
                                number = 1,
                                unitprice = tevent.signfee
                            });
                            _dbContext.CommitChanges();
                            orderId = orderid;
                        }
                        catch (Exception ex)
                        {
                            _dbContext.Rollback();
                            msg = "服务异常";
                            LogUtils.LogError("SignUpPlayerService.GoPayTran", ex);
                        }
                    }
                    else
                    {
                        orderId = order.id;
                    }
                }
                else
                {
                    msg = "报名信息有误";
                }
            }
            catch (Exception ex)
            {
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.GoPay", ex);
            }
            return orderId;
        }
        //申请退赛
        public bool ApplyRetire(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var playsignup = _dbContext.Get<t_event_player_signup>(id);
                if (playsignup != null)
                {
                    //获取赛事信息
                    t_event tevent = _dbContext.Get<t_event>(playsignup.eventId);
                    if (tevent.eventType == EventTypeEm.辩论)
                    {
                        #region 辩论
                        if (playsignup.signUpStatus == SignUpStatusEm.报名邀请中)//对方还未同意邀请
                        {
                            var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.组队失败},updatetime='{DateTime.Now}' where groupnum='{playsignup.groupnum}' and eventId={playsignup.eventId}";
                            _dbContext.Execute(sql);
                            flag = true;
                        }
                        else if (playsignup.signUpStatus == SignUpStatusEm.待付款)
                        {
                            var otherplaysignup = _dbContext.Select<t_event_player_signup>(c => c.groupnum == playsignup.groupnum && c.eventId == playsignup.eventId && c.memberId != memberId).FirstOrDefault();
                            if (otherplaysignup.signUpStatus == SignUpStatusEm.待付款)
                            {
                                var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.组队失败},updatetime='{DateTime.Now}' where groupnum='{playsignup.groupnum}' and eventId={playsignup.eventId}";
                                _dbContext.Execute(sql);
                                flag = true;
                            }
                            else
                            {
                                try
                                {
                                    _dbContext.BeginTransaction();
                                    var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.组队失败},updatetime='{DateTime.Now}' where groupnum='{playsignup.groupnum}' and eventId={playsignup.eventId}";
                                    t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == otherplaysignup.id && c.memberId == otherplaysignup.memberId && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                    _dbContext.Insert(new t_order_operation
                                    {
                                        content = "组队失败申请退费",
                                        title = "申请退费",
                                        operationStatus = OperationStatusEm.待处理,
                                        orderOperType = OrderOperTypeEm.取消订单,
                                        orderId = t_order.id,
                                        memberId = otherplaysignup.memberId
                                    });
                                    t_order.orderStatus = OrderStatusEm.退款中;
                                    t_order.updatetime = DateTime.Now;
                                    _dbContext.Update(t_order);

                                    _dbContext.Execute(sql);
                                    _dbContext.CommitChanges();
                                    flag = true;
                                }
                                catch (Exception ex)
                                {
                                    _dbContext.Rollback();
                                    flag = false;
                                    msg = "服务异常";
                                    LogUtils.LogError("SignUpPlayerService.ApplyRetire2Tran", ex);
                                }

                            }
                        }
                        else if (playsignup.signUpStatus == SignUpStatusEm.已付款)
                        {
                            try
                            {
                                _dbContext.BeginTransaction();
                                var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.组队失败},updatetime='{DateTime.Now}' where groupnum='{playsignup.groupnum}' and eventId={playsignup.eventId}";
                                t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == id && c.memberId == playsignup.memberId && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                _dbContext.Insert(new t_order_operation
                                {
                                    content = "组队失败申请退费",
                                    title = "申请退费",
                                    operationStatus = OperationStatusEm.待处理,
                                    orderOperType = OrderOperTypeEm.取消订单,
                                    orderId = t_order.id,
                                    memberId = memberId
                                });
                                t_order.orderStatus = OrderStatusEm.退款中;
                                t_order.updatetime = DateTime.Now;
                                _dbContext.Update(t_order);

                                _dbContext.Execute(sql);
                                _dbContext.CommitChanges();
                                flag = true;
                            }
                            catch (Exception ex)
                            {
                                _dbContext.Rollback();
                                flag = false;
                                msg = "服务异常";
                                LogUtils.LogError("SignUpPlayerService.ApplyRetire2Tran", ex);
                            }
                        }
                        else if (playsignup.signUpStatus == SignUpStatusEm.报名成功)
                        {
                            playsignup.updatetime = DateTime.Now;
                            playsignup.signUpStatus = SignUpStatusEm.退赛申请中;
                            _dbContext.Update(playsignup);
                            flag = true;
                        }
                        else
                        {
                            msg = "状态已改变 请刷新页面后重试";
                        }
                        #endregion 
                    }
                    else
                    {
                        #region 演讲
                        if (playsignup.signUpStatus == SignUpStatusEm.报名成功)
                        {
                            if (tevent.endrefunddate > DateTime.Now)
                            {
                                //发起退款
                                flag = true;
                            }
                            else
                            {
                                playsignup.signUpStatus = SignUpStatusEm.已退赛;
                                playsignup.updatetime = DateTime.Now;
                                _dbContext.Update(playsignup);
                                flag = true;
                            }
                        }

                        #endregion 
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
        public bool IsConfirmRetire(int id, bool isConfirm, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_event_player_signup signup = _dbContext.Get<t_event_player_signup>(id);
                if (signup != null && signup.signUpStatus == SignUpStatusEm.退赛申请中)
                {
                    try
                    {
                        _dbContext.Rollback();
                        if (isConfirm)
                        {
                            t_event tevent = _dbContext.Get<t_event>(signup.eventId);
                            if (tevent.endrefunddate > DateTime.Now)//可以退费
                            {
                                #region 队友退赛信息
                                t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == id && c.memberId == signup.memberId && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                _dbContext.Insert(new t_order_operation
                                {
                                    content = "组队失败申请退费",
                                    title = "申请退费",
                                    operationStatus = OperationStatusEm.待处理,
                                    orderOperType = OrderOperTypeEm.取消订单,
                                    orderId = t_order.id,
                                    memberId = signup.memberId
                                });
                                t_order.orderStatus = OrderStatusEm.退款中;
                                t_order.updatetime = DateTime.Now;
                                _dbContext.Update(t_order);
                                #endregion

                                #region 自己退赛信息
                                var otherplaysignup = _dbContext.Select<t_event_player_signup>(c => c.groupnum == signup.groupnum && c.eventId == signup.eventId && c.memberId == memberId).FirstOrDefault();
                                t_order t_orderother = _dbContext.Select<t_order>(c => c.sourceId == otherplaysignup.id && c.memberId == memberId && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                _dbContext.Insert(new t_order_operation
                                {
                                    content = "组队失败申请退费",
                                    title = "申请退费",
                                    operationStatus = OperationStatusEm.待处理,
                                    orderOperType = OrderOperTypeEm.取消订单,
                                    orderId = t_orderother.id,
                                    memberId = memberId
                                });
                                t_orderother.orderStatus = OrderStatusEm.退款中;
                                t_orderother.updatetime = DateTime.Now;
                                _dbContext.Update(t_orderother);
                                #endregion
                            }
                            var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.已退赛},updatetime='{DateTime.Now}' where groupnum='{signup.groupnum}' and eventId={signup.eventId}";
                            _dbContext.Execute(sql);
                        }
                        else
                        {
                            var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.报名成功},updatetime='{DateTime.Now}' where groupnum='{signup.groupnum}' and eventId={signup.eventId}";
                            _dbContext.Execute(sql);
                        }
                        _dbContext.CommitChanges();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("SignUpPlayerService.IsConfirmRetireTran", ex);
                    }
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
                LogUtils.LogError("SignUpPlayerService.IsConfirmRetire", ex);
            }
            return flag;
        }
        //支付回调
        public void Callback(int memberId, int sourceId)
        {
            try
            {
                using (IDBContext dbcontext = new MySqlDBContext())
                {
                    var signup = dbcontext.Get<t_event_player_signup>(sourceId);
                    if (signup != null && memberId == signup.memberId)
                    {
                        //查询队友状态
                        var other_signup = dbcontext.Select<t_event_player_signup>(c => c.groupnum == signup.groupnum && c.eventGroupId == signup.eventGroupId && c.memberId != memberId).FirstOrDefault();
                        if (other_signup.signUpStatus == SignUpStatusEm.待付款)
                        {
                            signup.updatetime = DateTime.Now;
                            signup.signUpStatus = SignUpStatusEm.已付款;
                            dbcontext.Update(signup);
                        }
                        else if (other_signup.signUpStatus == SignUpStatusEm.已付款)
                        {
                            var sql = $"update t_event_player_signup set signUpStatus={(int)SignUpStatusEm.报名成功},updatetime='{DateTime.Now}' where groupnum='{signup.groupnum}' and eventId={signup.eventId}";
                            dbcontext.Execute(sql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Callback", ex);
            }
        }
        //当前赛事
        public List<PlayerCurrentEventResponse> CurrentPlayerEvent(int memberId)
        {
            List<PlayerCurrentEventResponse> list = new List<PlayerCurrentEventResponse>();
            try
            {
                var sql = $@"select a.*,b.code MemberCode,b.completename MemberName from 
                             (select b.id EventId,b.code EventCode,b.name EventName,b.eventType EventType,b.starteventdate StartEventDate,
                              a.memberId MemberId from t_event_player_signup a
                              inner join t_event b on a.eventId=b.id
                              left  join t_event_matchdate c on a.eventId=c.eventId
                              where  a.isdelete=0 and c.eventMatchDate='{DateTime.Now.ToShortDateString()}' 
                              and  a.groupnum in (select groupnum  from t_event_player_signup where memberId={memberId} and signUpStatus={(int)SignUpStatusEm.报名成功})
                              ) a inner join t_member_player b on a.MemberId=b.memberId
                          ";
                list = _dbContext.Query<PlayerCurrentEventResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.CurrentPlayerEvent", ex);
            }
            return list;
        }
        //选手报名列表
        public List<EventPlayerSignUpListResponse> EventPlayerList(EventPlayerSignUpQueryRequest request)
        {
            List<EventPlayerSignUpListResponse> list = new List<EventPlayerSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = $"%{request.KeyValue}%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
                }
                if (request.EventGroupId != null && request.EventGroupId > 0)
                {
                    join.Append(" and a.eventGroupId=@EventGroupId");
                }
                if (request.SignUpStatus != null && request.SignUpStatus > 0)
                {
                    join.Append(" and a.signUpStatus=@SignUpStatus");
                }
                var sql = $@"select a.*,b.code MemberCode,b.completename MemberName,
                            b.grade,b.gender,b.contactmobile,d.name EventGroupName from t_event_player_signup a 
                            inner join t_member_player b on a.memberId=b.memberId
                            inner join t_event c on a.eventId=c.id
                            inner join t_event_group d on a.eventGroupId=d.id
                            and c.memberId=@MemberId and a.eventId=@EventId {join.ToString()}
                            order by a.groupnum desc 
                         ";
                int totalCount = 0;
                list = _dbContext.Page<EventPlayerSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                foreach (var item in list)
                {
                    var data = _dbContext.Query<dynamic>($"select b.chinessname,c.name from t_player_edu  a inner join t_sys_school b on a.schoolId=b.id inner join t_sys_city c on c.id=b.cityId  where a.memberid={item.MemberId} and a.isdelete=0 order by a.startdate desc limit 1").FirstOrDefault();
                    if (data != null)
                    {
                        item.SchoolName = data.chinessname;
                        item.CityName = data.name;
                    }
                }
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.EventPlayerList", ex);
            }
            return list;
        }
        //判断选手是否可以报名
        private bool IsValid(t_event_group group, t_member_player member)
        {
            bool flag = true;
            try
            {
                //第一步判断年级
                if (group.mingrade.HasValue || group.maxgrade.HasValue)
                {
                    if (group.mingrade != (int)GradeEm.unlimited)
                    {
                        if ((int)member.grade < group.mingrade)
                        {
                            return false;
                        }
                    }

                    if (group.maxgrade != (int)GradeEm.unlimited)
                    {
                        if ((int)member.grade > group.maxgrade)
                        {
                            return false;
                        }
                    }
                }
                //第二步判断参赛次数
                if (group.mintimes.HasValue || group.maxtimes.HasValue)
                {
                    //从对垒表中查参加过的赛事
                    var times = _dbContext.ExecuteScalar($"select count(1) from t_event_player_signup where isdelete=0 and memberId={member.memberId} and  signUpStatus in ({ParamsConfig._signup_in})").ToObjInt();
                    if (group.mintimes > 0)
                    {
                        if (group.mintimes > times)
                            return false;
                    }
                    if (group.maxtimes > 0)
                    {
                        if (times > group.maxtimes)
                            return false;
                    }
                }

            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.IsValid", ex);
            }
            return flag;
        }
        //选手报名列表
        public List<PlayerSignUpListResponse> PlayerSignUpList(PlayerSignUpQueryRequest request)
        {
            List<PlayerSignUpListResponse> list = new List<PlayerSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                var sql = $@"select a.*,b.code MemberCode,b.completename MemberName,c.code EventCode,
                             c.name EventName,d.name EventGroupName,c.eventType EventType
                             from  t_event_player_signup a 
                             inner join t_member_player b on a.memberId=b.memberId
                             inner join t_event c on a.eventId=c.id
                             inner join t_event_group d on a.eventGroupId=d.id
                             where a.isdelete=0  and a.groupnum in (select groupnum  from t_event_player_signup where memberId=@MemberId)
                            {join.ToString()} order by  a.groupnum desc
                         ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.MemberId == request.MemberId)
                        {
                            item.Flag = true;
                        }
                    }
                }
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.PlayerSignUpList", ex);
            }
            return list;
        }
        // 生成签到信息
        public bool RenderSign(int eventId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var t_event = _dbContext.Get<t_event>(eventId);
                if (t_event != null)
                {
                    var data = _dbContext.Select<t_event_sign>(c => c.eventId == eventId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        msg = "您已申请过签到表";
                        return flag;
                    }

                    var listMatchDate = _dbContext.Select<t_event_matchdate>(c => c.eventId == eventId).ToList();
                    var list_playsignup = _dbContext.Select<t_event_player_signup>(c => c.eventId == eventId && c.signUpStatus == SignUpStatusEm.报名成功).ToList();
                    var list_event_referee_signup = _dbContext.Select<t_event_referee_signup>(c => c.eventId == eventId && c.refereeSignUpStatus == RefereeSignUpStatusEm.已录取).ToList();

                    if (listMatchDate != null && listMatchDate.Count > 0)
                    {
                        foreach (var item in listMatchDate)
                        {
                            #region 教练
                            if (list_event_referee_signup != null && list_event_referee_signup.Count > 0)
                            {
                                foreach (var itemreferee in list_event_referee_signup)
                                {
                                    //生成签到表
                                    _dbContext.Insert(new t_event_sign
                                    {
                                        eventId = t_event.id,
                                        eventSignStatus = EventSignStatusEm.待签到,
                                        eventSignType = EventSignTypeEm.裁判,
                                        memberId = itemreferee.memberId,
                                        signdate = item.eventMatchDate,
                                        eventGroupId= itemreferee.eventGroupId,
                                        isStop = false
                                    });
                                }
                            }
                            #endregion

                            #region 选手
                            if (list_playsignup != null && list_playsignup.Count > 0)
                            {
                                foreach (var itemplayer in list_playsignup)
                                {
                                    //生成签到表
                                    _dbContext.Insert(new t_event_sign
                                    {
                                        eventId = t_event.id,
                                        eventSignStatus = EventSignStatusEm.待签到,
                                        eventSignType = EventSignTypeEm.选手,
                                        memberId = itemplayer.memberId,
                                        signdate = item.eventMatchDate,
                                        eventGroupId = itemplayer.eventGroupId,
                                        isStop = false
                                    });
                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    msg = "赛事信息有误";
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.RenderSign", ex);
            }
            return flag;
        }
        //选手退费列表
        public List<PlayerRefundListResponse> PlayerRefundList(PlayerSignUpQueryRequest request)
        {
            List<PlayerRefundListResponse> list = new List<PlayerRefundListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                var sql = $@"select d.code EventCode,d.name EventName,d.eventType EventType,e.name EventGroupName,a.operationStatus OperationStatus 
                             from t_order_operation a
                             inner join t_order b on a.orderId=b.id
                             inner join t_event_player_signup c on b.sourceId=c.id
                             inner join t_event d on c.eventId=d.id
                             inner join t_event_group e on c.eventGroupId=e.id
                             where a.isdelete=0 and a.memberId=@MemberId  and b.orderType={(int)OrderTypeEm.赛事报名}
                             {join.ToString()} order by a.createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<PlayerRefundListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.PlayerRefundList", ex);
            }
            return list;
        }
        //未报名成功的队伍 系统申请退费
        public bool ApplyRefund(int eventId, int operUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var t_event = _dbContext.Get<t_event>(eventId);
                if (t_event != null)
                {
                    var playersignup = _dbContext.Select<t_event_player_signup>(c => c.eventId == eventId && c.signUpStatus != SignUpStatusEm.已退赛 && c.signUpStatus != SignUpStatusEm.报名成功 && c.signUpStatus != SignUpStatusEm.组队失败 && c.signUpStatus != SignUpStatusEm.退赛申请中).ToList();
                    if (playersignup != null && playersignup.Count > 0)
                    {
                        try
                        {
                            _dbContext.BeginTransaction();
                            foreach (var item in playersignup)
                            {
                                if (item.signUpStatus == SignUpStatusEm.已付款)
                                {
                                    t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == item.id && c.memberId == item.memberId && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                    if (t_order != null)
                                    {
                                        _dbContext.Insert(new t_order_operation
                                        {
                                            content = "组队失败申请退费",
                                            title = "申请退费",
                                            operationStatus = OperationStatusEm.待处理,
                                            orderOperType = OrderOperTypeEm.取消订单,
                                            orderId = t_order.id,
                                            memberId = item.memberId
                                        });

                                        t_order.orderStatus = OrderStatusEm.退款中;
                                        t_order.updatetime = DateTime.Now;
                                        _dbContext.Update(t_order);
                                    }
                                }
                                item.signUpStatus = SignUpStatusEm.组队失败;
                                item.updatetime = DateTime.Now;
                                _dbContext.Update(item);
                            }
                            _dbContext.CommitChanges();
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            _dbContext.Rollback();
                            flag = false;
                            msg = "服务异常";
                            LogUtils.LogError("SignUpPlayerService.ApplyRefundTran", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SignUpPlayerService.ApplyRefund", ex);
            }
            return flag;
        }
        //报名成功的学员
        public List<MemberSelectResponse> SelectPlayer(int eventId,int? eventGroupId,string keyvalue)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                if (keyvalue.IsEmpty())
                {
                    return list;
                }
                var dy = new DynamicParameters();
                dy.Add("KeyValue", $"%{keyvalue}%");
                StringBuilder sb = new StringBuilder();
                if (eventGroupId != null&& eventGroupId>0)
                {
                    sb.Append($" and a.eventGroupId={eventGroupId}");
                }
                var sql = $@"select a.memberId MemberId,b.code MemberCode,b.completename MemberName
                             from  t_event_player_signup a 
                             inner join t_member_player b on a.memberId=b.memberId
                             where a.isdelete=0 and a.eventId={eventId}  and a.signUpStatus={(int)SignUpStatusEm.报名成功}
                             and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue) {sb.ToString()}
                           ";
                list=_dbContext.Query<MemberSelectResponse>(sql, dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.SelectPlayer", ex);
            }
            return list;
        }
    }
}
