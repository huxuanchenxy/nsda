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
    /// 临时会员管理
    /// </summary>
    public class MemberTempService : IMemberTempService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberTempService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //新增临时选手
        public bool InsertTempPlayer(List<TempPlayerRequest> request, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request == null || request.Count == 0)
                {
                    msg = "请先录入数据";
                    return flag;
                }
                TempPlayerRequest tempplayer = request.FirstOrDefault();
                t_event t_event = _dbContext.Get<t_event>(tempplayer.EventId);
                //数据校验
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                t_eventgroup t_group = _dbContext.Get<t_eventgroup>(tempplayer.EventGroupId);
                if (t_group == null)
                {
                    msg = "赛事组别信息有误";
                    return flag;
                }

                if (t_group.teamnumber != request.Count)
                {
                    msg = "队伍人数有误";
                    return flag;
                }

                foreach (var item in request)
                {
                    if (item.Name.IsEmpty())
                    {
                        msg = "姓名不能为空";
                        break;
                    }
                    if (item.Email.IsEmpty())
                    {
                        msg = "邮箱不能为空";
                        break;
                    }
                    if (item.ContactMobile.IsEmpty())
                    {
                        msg = "联系方式不能为空";
                        break;
                    }
                }

                if (msg.IsNotEmpty())
                {
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                    foreach (var item in request)
                    {
                        //创建账号
                        string code = _dataRepository.MemberRepo.RenderCode("tnsda");
                        t_member member = new t_member
                        {
                            code = code,
                            account = code,
                            name = item.Email,
                            pwd = item.ContactMobile,
                            memberStatus = MemberStatusEm.待认证,
                            completename = item.Name,
                            memberType = MemberTypeEm.临时选手
                        };
                        int memberInsertId = _dbContext.Insert(member).ToObjInt();
                        _dbContext.Insert(new t_memberpoints
                        {
                            memberId = memberId,
                            points = 0,
                            eventPoints = 0,
                            servicePoints = 0
                        });
                        _dbContext.Insert(new t_membertemp
                        {
                            code = groupnum,
                            contactmobile = item.ContactMobile,
                            email = item.Email,
                            eventId = item.EventId,
                            memberId = memberInsertId,
                            tempStatus = TempStatusEm.待绑定,
                            tempType = TempTypeEm.临时选手
                        });
                        //报名表
                        _dbContext.Insert(new t_player_signup
                        {
                            groupnum = groupnum,
                            eventId = item.EventId,
                            eventGroupId = item.EventGroupId,
                            isTemp = true,
                            memberId = memberInsertId,
                            signfee = t_event.signfee,
                            signUpStatus = SignUpStatusEm.报名成功,
                            signUpType = SignUpTypeEm.临时添加
                        });

                        if (item.PlayerEdu != null && item.PlayerEdu.SchoolId > 0)
                        {
                            _dbContext.Insert(new t_playereduexper
                            {
                                startdate = item.PlayerEdu.StartDate,
                                enddate = item.PlayerEdu.EndDate,
                                memberId = memberInsertId,
                                schoolId = item.PlayerEdu.SchoolId
                            });
                        }
                    }
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    msg = "服务异常";
                    _dbContext.Rollback();
                    LogUtils.LogError("MemberTempService.InsertTempPlayerTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberTempService.InsertTempPlayer", ex);
            }
            return flag;
        }
        //新增临时裁判
        public bool InsertTempReferee(TempRefereeRequest request, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系号码不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }
                t_event t_event = _dbContext.Get<t_event>(request.EventId);
                //数据校验
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    string code = _dataRepository.MemberRepo.RenderCode("tnsda");
                    t_member member = new t_member
                    {
                        code = code,
                        account = code,
                        name = request.Name,
                        pwd = request.ContactMobile,
                        memberStatus = MemberStatusEm.通过,
                        completename = request.Name,
                        memberType = MemberTypeEm.临时裁判
                    };
                    int memberInsertId = _dbContext.Insert(member).ToObjInt();
                    //积分表
                    _dbContext.Insert(new t_memberpoints
                    {
                        memberId = memberInsertId,
                        points = 0,
                        eventPoints = 0,
                        servicePoints = 0
                    });
                    //临时会员表
                    _dbContext.Insert(new t_membertemp
                    {
                        code = member.code,
                        contactmobile = request.ContactMobile,
                        email = string.Empty,
                        eventId = request.EventId,
                        memberId = memberInsertId,
                        tempStatus = TempStatusEm.待绑定,
                        tempType = TempTypeEm.临时裁判
                    });
                    //报名表
                    _dbContext.Insert(new t_referee_signup
                    {
                        isTemp = true,
                        eventId = request.EventId,
                        memberId = memberInsertId,
                        refereeSignUpStatus = RefereeSignUpStatusEm.申请成功
                    });
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    msg = "服务异常";
                    _dbContext.Rollback();
                    LogUtils.LogError("MemberTempService.InsertTempRefereeTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberTempService.InsertTempReferee", ex);
            }
            return flag;
        }
        //临时选手绑定 生成支付订单
        public bool BindTempPlayer(BindTempPlayerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //校验数据的真实性
                if (request.GroupNum.IsEmpty())
                {
                    msg = "队伍编码不能为空";
                    return flag;
                }
                if (request.Email.IsEmpty())
                {
                    msg = "邮箱不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }

                var data = _dbContext.Select<t_membertemp>(c => c.email == request.Email && c.contactmobile == request.ContactMobile && c.code == request.GroupNum && c.tempType == TempTypeEm.临时选手 && c.tempStatus == TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在，请核对后再操作";
                    return flag;
                }

                t_event t_event = _dbContext.Get<t_event>(data.eventId);
                if (t_event.eventStatus != EventStatusEm.比赛完成)
                {
                    msg = "赛事未完成不能进行绑定";
                    return flag;
                }

                if (data.tomemberId != null && data.tomemberId > 0)
                {
                    if (data.tomemberId != request.MemberId)
                    {
                        msg = "此信息已绑定过";
                        return flag;
                    }
                }

                t_order order = _dbContext.Select<t_order>(c => c.memberId == data.memberId && c.orderType == OrderTypeEm.临时选手绑定 && c.sourceId == data.id).FirstOrDefault();
                if (order == null)//没创建过订单 
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        //创建订单
                        var orderid = _dbContext.Insert(new t_order
                        {
                            isNeedInvoice = false,
                            mainOrderId = null,
                            memberId = data.memberId,
                            money = t_event.signfee,
                            orderStatus = OrderStatusEm.等待支付,
                            orderType = OrderTypeEm.临时选手绑定,
                            payExpiryDate = DateTime.Now.AddYears(3),
                            remark = "临时选手绑定",
                            sourceId = data.id,
                            totalcoupon = 0,
                            totaldiscount = 0
                        }).ToObjInt();
                        _dbContext.Insert(new t_orderdetail
                        {
                            memberId = data.memberId,
                            orderId = orderid,
                            coupon = 0,
                            discountprice = 0,
                            money = t_event.signfee,
                            productId = 0,
                            name = $"{t_event.name}报名费",
                            number = 1,
                            unitprice = t_event.signfee
                        });
                        //生成支付链接
                        data.tomemberId = request.MemberId;
                        data.updatetime = DateTime.Now;
                        _dbContext.Update(data);
                        _dbContext.CommitChanges();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        msg = "服务异常";
                        _dbContext.Rollback();
                        LogUtils.LogError("MemberTempService.BindTempPlayerTran", ex);
                    }
                }
                else//创建过订单
                {
                    //通过订单生成支付链接
                    if (order.orderStatus != OrderStatusEm.等待支付 && order.orderStatus != OrderStatusEm.支付失败)
                    {

                    }
                    else
                    {
                        msg = "状态已改变";
                    }
                }

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberTempService.BindTempPlayer", ex);
            }
            return flag;
        }
        //临时教练绑定 
        public bool BindTempReferee(BindTempRefereeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //校验数据的真实性
                if (request.TempRefereeNum.IsEmpty())
                {
                    msg = "临时裁判编码不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "手机号码不能为空";
                    return flag;
                }

                var data = _dbContext.Select<t_membertemp>(c => c.contactmobile == request.ContactMobile && c.code == request.TempRefereeNum && c.tempType == Model.enums.TempTypeEm.临时裁判 && c.tempStatus == TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在，请核对后再操作";
                    return flag;
                }
                try
                {
                    _dbContext.BeginTransaction();
                    t_memberpoints points = _dbContext.Select<t_memberpoints>(c => c.memberId == data.memberId).FirstOrDefault();
                    _dbContext.Execute($"update t_memberpoints set points=points+{points.points},eventPoints=eventPoints+{points.eventPoints},servicePoints=servicePoints+{points.servicePoints} where memberId={request.MemberId}");
                    _dbContext.Execute($"update t_membertemp set tomemberId={request.MemberId},updateTime={DateTime.Now},tempStatus={TempStatusEm.已绑定}  where id={data.id}");
                    _dbContext.Execute($"update t_memberpointsrecord set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                    _dbContext.Execute($"update t_memberpointsdetail set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                    _dbContext.Execute($"update t_referee_signup set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    LogUtils.LogError("MemberTempService.BindTempRefereeTran", ex);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.BindTempReferee", ex);
            }
            return flag;
        }
        //临时会员数据列表
        public List<MemberTempResponse> List(TempMemberQueryRequest request)
        {
            List<MemberTempResponse> list = new List<MemberTempResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.TempStatus.HasValue)
                {
                    join.Append(" and a.tempStatus=@TempStatus");
                }
                if (request.TempType.HasValue)
                {
                    join.Append(" and a.tempType=@TempType");
                }
                var sql=$@"select a.*,b.completename  MemberName,c.completename  ToMemberName,d.name EventName  from t_membertemp a 
                            inner join t_member b on a.memberId=b.Id 
                            left  join t_member c on a.tomemberId=c.Id 
                            inner join t_event  d on a.eventId=d.Id
                            where a.isdelete=0 {join.ToString()} order by a.createtime
                        ";
                int totalCount = 0;
                list = _dbContext.Page<MemberTempResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.List", ex);
            }
            return list;
        }
        // 支付回调 进行数据迁移
        public void Callback(int memberId, int sourceId)
        {
            try
            {
                t_membertemp temp = _dbContext.Get<t_membertemp>(sourceId);
                if (temp != null && temp.memberId == memberId)
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        t_memberpoints points = _dbContext.Select<t_memberpoints>(c => c.memberId == temp.memberId).FirstOrDefault();
                        _dbContext.Execute($"update t_memberpoints set points=points+{points.points},eventPoints=eventPoints+{points.eventPoints},servicePoints=servicePoints+{points.servicePoints} where memberId={temp.tomemberId}");
                        _dbContext.Execute($"update t_membertemp set updateTime={DateTime.Now},tempStatus={TempStatusEm.已绑定}  where id={temp.id}");
                        _dbContext.Execute($"update t_memberpointsrecord set memberId={temp.tomemberId} where memberId={temp.memberId} and isdelete=0");
                        _dbContext.Execute($"update t_memberpointsdetail set memberId={temp.tomemberId} where memberId={temp.memberId} and isdelete=0");
                        _dbContext.Execute($"update t_player_signup set memberId={temp.tomemberId} where memberId={temp.tomemberId} and isdelete=0");
                        //对垒表也要修改
                        _dbContext.CommitChanges();
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        LogUtils.LogError("MemberTempService.CallbackTran", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.Callback", ex);
            }
        }
    }
}
