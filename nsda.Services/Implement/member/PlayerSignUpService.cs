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
                    var from = _dbContext.ExecuteScalar($"select count(1) from t_player_signup where isdelete=0 and memberId={request.FromMemberId} and eventId={request.EventId} and signUpStatus not in ({ParamsConfig._signup_notin})").ToObjInt();
                    if (from > 0)
                    {
                        msg = "您已申请过此次赛事";
                        return flag;
                    }
                    var to = _dbContext.ExecuteScalar($"select count(1) from t_player_signup where isdelete=0 and memberId={request.ToMemberId} and eventId={request.EventId} and signUpStatus not in ({ParamsConfig._signup_notin})").ToObjInt();
                    if (to > 0)
                    {
                        msg = "您邀请的队友已申请过此次赛事";
                        return flag;
                    }
                    //2.0 报名队伍数是否达到上限
                    var teamcount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={request.EventId}").ToObjInt();
                    if (tevent.maxnumber < teamcount + 1)
                    {
                        msg = "达到报名人数上限无法继续报名";
                        return flag;
                    }

                    t_eventgroup eventGroup = _dbContext.Get<t_eventgroup>(request.EventGroupId);
                    //3.0 是否有资格报名
                    if (!IsValid(eventGroup, request.FromMemberId))
                    {
                        msg = "您不符合此赛事报名规则";
                        return flag;
                    }

                    if (!IsValid(eventGroup, request.ToMemberId))
                    {
                        msg = "您队友不符合此赛事报名规则";
                        return flag;
                    }

                    try
                    {
                        _dbContext.BeginTransaction();
                        string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                        //邀请者
                        _dbContext.Insert(new t_player_signup
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
                        _dbContext.Insert(new t_player_signup
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
                    var listfrom = _dbContext.Select<t_player_signup>(c => c.memberId == request.FromMemberId && c.eventId == request.EventId).ToList();
                    if (listfrom != null && listfrom.Count > 0)
                    {
                        msg = "您已申请过此次赛事";
                        return flag;
                    }
                    try
                    {
                        _dbContext.BeginTransaction();
                        string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                        //邀请者
                        _dbContext.Insert(new t_player_signup
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
                var playsignup = _dbContext.Get<t_player_signup>(id);
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
                        var sql = $"update t_player_signup set signUpStatus={signStatus},updatetime={DateTime.Now} where groupnum={playsignup.groupnum} and eventId={playsignup.eventId}";
                        _dbContext.Execute(sql);
                        flag = true;
                    }
                    else if(tevent.eventType==EventTypeEm.演讲)
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
        public bool GoPay(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_player_signup signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null && signup.memberId == memberId)
                {
                    t_event tevent = _dbContext.Get<t_event>(signup.eventId);
                    var teamcount = _dbContext.ExecuteScalar($"select count(distinct(groupnum)) from t_player_signup where isdelete=0 and signUpStatus in ({ParamsConfig._signup_in}) and eventId={signup.eventId}").ToObjInt();
                    if (tevent.maxnumber < teamcount + 1)
                    {
                        msg = "达到报名人数上限无法继续去支付";
                        return flag;
                    }
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
                                remark = $"{t_event.name}赛事报名",
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
                            flag = true;
                            //生成支付信息
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
                else
                {
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
                        #region 辩论
                        if (playsignup.signUpStatus == SignUpStatusEm.报名邀请中)//对方还未同意邀请
                        {
                            var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.组队失败},updatetime={DateTime.Now} where groupnum={playsignup.groupnum} and eventId={playsignup.eventId}";
                            _dbContext.Execute(sql);
                            flag = true;
                        }
                        else if (playsignup.signUpStatus == SignUpStatusEm.待付款)
                        {
                            var otherplaysignup = _dbContext.Select<t_player_signup>(c => c.groupnum == playsignup.groupnum && c.eventId == playsignup.eventId && c.memberId != memberId).FirstOrDefault();
                            if (otherplaysignup.signUpStatus == SignUpStatusEm.待付款)
                            {
                                var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.组队失败},updatetime={DateTime.Now} where groupnum={playsignup.groupnum} and eventId={playsignup.eventId}";
                                _dbContext.Execute(sql);
                                flag = true;
                            }
                            else
                            {
                                try
                                {
                                    _dbContext.BeginTransaction();
                                    var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.组队失败},updatetime={DateTime.Now} where groupnum={playsignup.groupnum} and eventId={playsignup.eventId}";
                                    t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == otherplaysignup.id && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
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
                                var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.组队失败},updatetime={DateTime.Now} where groupnum={playsignup.groupnum} and eventId={playsignup.eventId}";
                                t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == id && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
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
        public bool ConfirmRetire(int id, bool isAgree, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_player_signup signup = _dbContext.Get<t_player_signup>(id);
                if (signup != null&&signup.signUpStatus==SignUpStatusEm.退赛申请中)
                {
                    try
                    {
                        _dbContext.Rollback();
                        if (isAgree)
                        {
                            t_event tevent = _dbContext.Get<t_event>(signup.eventId);
                            if (tevent.endrefunddate > DateTime.Now)//可以退费
                            {
                                #region 队友退赛信息
                                t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == id && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
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
                                var otherplaysignup = _dbContext.Select<t_player_signup>(c => c.groupnum == signup.groupnum && c.eventId == signup.eventId && c.memberId == memberId).FirstOrDefault();
                                t_order t_orderother = _dbContext.Select<t_order>(c => c.sourceId == otherplaysignup.id && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                _dbContext.Insert(new t_order_operation
                                {
                                    content = "组队失败申请退费",
                                    title = "申请退费",
                                    operationStatus = OperationStatusEm.待处理,
                                    orderOperType = OrderOperTypeEm.取消订单,
                                    orderId = t_orderother.id,
                                    memberId = otherplaysignup.memberId
                                });
                                t_orderother.orderStatus = OrderStatusEm.退款中;
                                t_orderother.updatetime = DateTime.Now;
                                _dbContext.Update(t_orderother);
                                #endregion
                            }
                            var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.已退赛},updatetime={DateTime.Now} where groupnum={signup.groupnum} and eventId={signup.eventId}";
                            _dbContext.Execute(sql);
                        }
                        else
                        {
                            var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.报名成功},updatetime={DateTime.Now} where groupnum={signup.groupnum} and eventId={signup.eventId}";
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
                        LogUtils.LogError("SignUpPlayerService.ConfirmRetireTran", ex);
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
                LogUtils.LogError("SignUpPlayerService.ConfirmRetire", ex);
            }
            return flag;
        }
        //支付回调
        public void Callback(int memberId, int sourceId)
        {
            try
            {
                var signup = _dbContext.Get<t_player_signup>(sourceId);
                if (signup != null && memberId == signup.memberId)
                {
                    //查询队友状态
                    var other_signup = _dbContext.Select<t_player_signup>(c => c.groupnum == signup.groupnum && c.eventGroupId == signup.eventGroupId && c.memberId != memberId).FirstOrDefault();
                    if (other_signup.signUpStatus == SignUpStatusEm.待付款)
                    {
                        signup.updatetime = DateTime.Now;
                        signup.signUpStatus = SignUpStatusEm.已付款;
                        _dbContext.Update(signup);
                    }
                    else if (other_signup.signUpStatus == SignUpStatusEm.已付款)
                    {
                        var sql = $"update t_player_signup set signUpStatus={SignUpStatusEm.报名成功},updatetime={DateTime.Now} where groupnum={signup.groupnum} and eventId={signup.eventId}";
                        _dbContext.Execute(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SignUpPlayerService.Callback", ex);
            }
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
        public List<EventPlayerSignUpListResponse> EventPlayerList(EventPlayerSignUpQueryRequest request)
        {
            List<EventPlayerSignUpListResponse> list = new List<EventPlayerSignUpListResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.KeyValue.IsNotEmpty())
                {
                    request.KeyValue = "%" + request.KeyValue + "%";
                    join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
                }
                var sql = $@" select a.*,b.code MemberCode,b.completename MemberName,a.grade,a.gender,a.contactmobile from t_player_signup a 
                            inner join t_member b on a.memberId=b.id
                            inner join t_event c on a.eventId=c.id
                            where a.isdelete=0 and b.isdelete=0 and c.isdelete=0 
                            and c.memberId=@MemberId and a.eventId=@EventId {join.ToString()}
                            order by c.createtime desc 
                         ";
                int totalCount = 0;
                list = _dbContext.Page<EventPlayerSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                foreach (var item in list)
                {
                    var data = _dbContext.Query<dynamic>($"select b.chinessname,c.name from t_playereduexper  a inner join t_school b on a.schoolId=b.id inner join t_city c on c.id=b.cityId  where a.memberid={item.MemberId} and a.isdelete=0 order by startdate desc limit 1").FirstOrDefault();
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
        //邀请队友
        public List<MemberSelectResponse> Invitation(string keyvalue, int eventId, int groupId, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                //需要过滤已经报名的选手
                var sql = $@"
                            select * from t_member where (isdelete=0 
                            and memberType={MemberTypeEm.选手} and id!={memberId} and memberStatus={MemberStatusEm.已认证} and (code=@key or completename=@key)) or id in
                            (
	                            select a.memberId from t_memberextend a
	                            inner join t_member b on a.memberId=b.id
	                            where a.memberId!={memberId} and a.memberExtendStatus={MemberExtendStatusEm.申请通过} and a.role={RoleEm.选手}  and b.memberStatus={MemberStatusEm.已认证}
                                and (b.code=@key or b.completename=@key)
                            )) and id not in (select memberId from t_player_signup where isdelete=0 and signUpStatus not in ({ParamsConfig._signup_notin})) limit 30
                         ";
                var dy = new DynamicParameters();
                dy.Add("key", keyvalue);
                var data = _dbContext.Query<t_member>(sql).ToList();
                if (data != null && data.Count > 0)
                {
                    t_eventgroup group = _dbContext.Get<t_eventgroup>(groupId);
                    foreach (var item in data)
                    {
                        //需要判断选手是否满足条件
                        if (IsValid(group, item.id))
                        {
                            list.Add(new MemberSelectResponse
                            {
                                Id = item.id,
                                Code = item.code,
                                Name = item.completename
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
        //获取用户可报名的组别
        public List<EventGroupResponse> EventGroup(int eventId, int memberId)
        {
            List<EventGroupResponse> list = new List<EventGroupResponse>();
            try
            {
                List<t_eventgroup> listgroup = _dbContext.Select<t_eventgroup>(c => c.eventId == eventId).ToList();
                foreach (var item in listgroup)
                {
                    if (IsValid(item, memberId))
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
        //判断选手是否可以报名
        private bool IsValid(t_eventgroup group, int memberId)
        {
            bool flag = true;
            try
            {
                //第一步判断年级
                if (group.mingrade.HasValue || group.maxgrade.HasValue)
                {
                    t_member member = _dbContext.Get<t_member>(memberId);
                    if (group.mingrade != (int)GradeEm.unlimited)
                    {
                        if (member.grade.HasValue)
                        {
                            if ((int)member.grade < (int)group.mingrade)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (group.maxgrade != (int)GradeEm.unlimited)
                    {
                        if (member.grade.HasValue)
                        {
                            if ((int)member.grade > (int)group.maxgrade)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                //第二步判断参赛次数
                if (group.mintimes.HasValue || group.maxtimes.HasValue)
                {
                    //从对垒表中查参加过的赛事
                    var times = _dbContext.ExecuteScalar($"select count(1) from t_player_signup where isdelete=0 and  signUpStatus in ({ParamsConfig._signup_in})").ToObjInt();
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
                var sql = $@"select a.*,b.code MemberCode,b.completename MemberName,c.code EventCode,c.name EventName,d.name EventGroupName
                             from  t_player_signup a 
                             inner join t_member b on a.memberId=b.id
                             inner join t_event c on a.eventId=c.id
                             inner join t_eventgroup d on a.eventGroupId=d.id
                             where a.isdelete=0  and groupnum in (select groupnum  from t_player_signup where memberId=@MemberId)
                            {join.ToString()} order by  a.creatTime desc
                         ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
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
                    var data = _dbContext.Select<t_eventsign>(c => c.eventId == eventId).ToList();
                    if (data != null && data.Count > 0)
                    {
                        msg = "您已申请过签到表";
                        return flag;
                    }

                    var list_referee_signup = _dbContext.Select<t_referee_signup>(c =>c.eventId==eventId&&c.refereeSignUpStatus != RefereeSignUpStatusEm.申请失败 && c.refereeSignUpStatus != RefereeSignUpStatusEm.待审核).ToList();
                    if (list_referee_signup != null && list_referee_signup.Count > 0)
                    {
                        foreach (var item in list_referee_signup)
                        {
                            //生成签到表
                            _dbContext.Insert(new t_eventsign
                            {
                                eventId = t_event.id,
                                eventSignStatus = EventSignStatusEm.待签到,
                                eventSignType = EventSignTypeEm.裁判,
                                memberId = item.memberId,
                                signdate = t_event.starteventdate
                            });

                            if (t_event.starteventdate != t_event.endeventdate)
                            {
                                _dbContext.Insert(new t_eventsign
                                {
                                    eventId = t_event.id,
                                    eventSignStatus = EventSignStatusEm.待签到,
                                    eventSignType = EventSignTypeEm.裁判,
                                    memberId = item.memberId,
                                    signdate = t_event.endeventdate
                                });
                            }
                        }
                    }

                    var list_playsignup = _dbContext.Select<t_player_signup>(c => c.eventId == eventId && c.signUpStatus == SignUpStatusEm.报名成功).ToList();
                    if (list_playsignup != null && list_playsignup.Count > 0)
                    {
                        foreach (var item in list_playsignup)
                        {

                            //生成签到表
                            _dbContext.Insert(new t_eventsign
                            {
                                eventId = t_event.id,
                                eventSignStatus = EventSignStatusEm.待签到,
                                eventSignType = EventSignTypeEm.选手,
                                memberId = item.memberId,
                                signdate = t_event.starteventdate,
                                eventGroupId = item.eventGroupId
                            });

                            if (t_event.starteventdate != t_event.endeventdate)
                            {
                                _dbContext.Insert(new t_eventsign
                                {
                                    eventId = t_event.id,
                                    eventSignStatus = EventSignStatusEm.待签到,
                                    eventSignType = EventSignTypeEm.选手,
                                    memberId = item.memberId,
                                    signdate = t_event.endeventdate,
                                    eventGroupId = item.eventGroupId
                                });
                            }
                        }
                    }
                }
                else
                {

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
                var sql = $@"select d.code EventCode,d.name EventName,d.eventType EventType,e.name EventGroupName,b.orderStatus OrderStatus 
                             from t_order_operation a
                             inner join t_order b on a.orderId=b.id
                             inner join t_player_signup c on b.sourceId=c.id
                             inner join t_event d on d.id=c.eventId
                             inner join t_eventgroup e on e.id=c.eventGroupId
                             where a.isdelete=0 and a.memberId=@MemberId  and b.orderType={OrderTypeEm.赛事报名}
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
                    var playersignup = _dbContext.Select<t_player_signup>(c => c.eventId == eventId && c.signUpStatus != SignUpStatusEm.已退赛 && c.signUpStatus == SignUpStatusEm.报名成功 && c.signUpStatus == SignUpStatusEm.组队失败 && c.signUpStatus == SignUpStatusEm.退赛申请中).ToList();
                    if (playersignup != null && playersignup.Count > 0)
                    {
                        try
                        {
                            _dbContext.BeginTransaction();
                            foreach (var item in playersignup)
                            {
                                if (item.signUpStatus == SignUpStatusEm.已付款)
                                {
                                    //创建一条
                                    t_order t_order = _dbContext.Select<t_order>(c => c.sourceId == item.id && c.orderType == OrderTypeEm.赛事报名 && c.orderStatus == OrderStatusEm.支付成功).FirstOrDefault();
                                    if (t_order != null)
                                    {
                                        _dbContext.Insert(new t_order_operation
                                        {
                                            content = "组队失败申请退费",
                                            title = "申请退费",
                                            operationStatus = OperationStatusEm.待处理,
                                            orderOperType = OrderOperTypeEm.取消订单,
                                            orderId = t_order.id,
                                            memberId=item.memberId
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
    }
}
