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
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }
                t_event_group t_group = _dbContext.Get<t_event_group>(tempplayer.EventGroupId);
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
                    if (item.PlayerEdu == null || item.PlayerEdu.SchoolId == 0)
                    {
                        msg = "教育经历不能为空";
                        break;
                    }
                    if (item.PlayerEdu.StartDate.IsEmpty())
                    {
                        msg = "教育经历开始时间不能为空";
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
                            account = item.Name,
                            pwd = item.ContactMobile,
                            memberStatus = MemberStatusEm.待认证,
                            memberType = MemberTypeEm.临时选手,
                            isExtendCoach = false,
                            isExtendPlayer = false,
                            isExtendReferee = false
                        };
                        int memberInsertId = _dbContext.Insert(member).ToObjInt();
                        _dbContext.Insert(new t_member_player
                        {
                            memberId = memberInsertId,
                            code = code,
                            card = item.Name,
                            cardType = CardTypeEm.身份证,
                            completename = item.Name,
                            completepinyin = item.Name,
                            name = item.Name,
                            contactaddress = item.Name,
                            contactmobile = item.ContactMobile,
                            emergencycontact = item.Name,
                            emergencycontactmobile = item.ContactMobile,
                            gender = GenderEm.未知,
                            grade = item.Grade,
                            pinyinname = item.Name,
                            pinyinsurname = item.Name,
                            surname = item.Name
                        });
                        _dbContext.Insert(new t_member_points
                        {
                            memberId = memberInsertId,
                            playerPoints = 0,
                            coachPoints = 0,
                            refereePoints = 0
                        });
                        _dbContext.Insert(new t_member_temp
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
                        _dbContext.Insert(new t_event_player_signup
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
                        _dbContext.Insert(new t_player_edu
                        {
                            startdate = item.PlayerEdu.StartDate,
                            enddate = item.PlayerEdu.EndDate,
                            memberId = memberInsertId,
                            schoolId = item.PlayerEdu.SchoolId
                        });
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
                        account = request.Name,
                        pwd = request.ContactMobile,
                        memberStatus = MemberStatusEm.已认证,
                        memberType = MemberTypeEm.临时裁判,
                        isExtendCoach = false,
                        isExtendPlayer = false,
                        isExtendReferee = false
                    };
                    int memberInsertId = _dbContext.Insert(member).ToObjInt();
                    _dbContext.Insert(new t_member_referee
                    {
                        pinyinsurname = request.Name,
                        completename = request.Name,
                        code = code,
                        completepinyin = request.Name,
                        contactaddress = request.Name,
                        contactmobile = request.ContactMobile,
                        emergencycontact = request.Name,
                        emergencycontactmobile = request.ContactMobile,
                        gender = GenderEm.未知,
                        memberId = memberInsertId,
                        pinyinname = request.Name
                    });
                    //积分表
                    _dbContext.Insert(new t_member_points
                    {
                        memberId = memberInsertId,
                        playerPoints = 0,
                        coachPoints = 0,
                        refereePoints = 0
                    });
                    //临时会员表
                    _dbContext.Insert(new t_member_temp
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
                    _dbContext.Insert(new t_event_referee_signup
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
        public int BindTempPlayer(BindTempPlayerRequest request, out string msg)
        {
            int orderId = 0;
            msg = string.Empty;
            try
            {
                //校验数据的真实性
                if (request.GroupNum.IsEmpty())
                {
                    msg = "队伍编码不能为空";
                    return orderId;
                }
                if (request.Email.IsEmpty())
                {
                    msg = "邮箱不能为空";
                    return orderId;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return orderId;
                }

                var data = _dbContext.Select<t_member_temp>(c => c.email == request.Email && c.contactmobile == request.ContactMobile && c.code == request.GroupNum && c.tempType == TempTypeEm.临时选手 && c.tempStatus == TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在，请核对后再操作";
                    return orderId;
                }

                t_event t_event = _dbContext.Get<t_event>(data.eventId);
                if (t_event.eventStatus != EventStatusEm.比赛完成)
                {
                    msg = "赛事未完成不能进行绑定";
                    return orderId;
                }

                //if (data.tomemberId != null && data.tomemberId > 0)
                //{
                //    if (data.tomemberId != request.MemberId)
                //    {
                //        msg = "此信息已绑定过";
                //        return orderId;
                //    }
                //}

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
                        _dbContext.Insert(new t_order_detail
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
                        orderId = orderid;
                    }
                    catch (Exception ex)
                    {
                        msg = "服务异常";
                        _dbContext.Rollback();
                        LogUtils.LogError("MemberTempService.BindTempPlayerTran", ex);
                    }
                }
                else//创建过订单
                {
                    if (order.orderStatus != OrderStatusEm.等待支付 && order.orderStatus != OrderStatusEm.支付失败)
                    {
                        orderId = order.id;
                    }
                    else
                    {
                        msg = "状态已改变";
                    }
                }

            }
            catch (Exception ex)
            {
                msg = "服务异常";
                LogUtils.LogError("MemberTempService.BindTempPlayer", ex);
            }
            return orderId;
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

                var data = _dbContext.Select<t_member_temp>(c => c.contactmobile == request.ContactMobile && c.code == request.TempRefereeNum && c.tempType == Model.enums.TempTypeEm.临时裁判 && c.tempStatus == TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在或已绑定过，请核对后再操作";
                    return flag;
                }

                t_event t_event = _dbContext.Get<t_event>(data.eventId);
                if (t_event.eventStatus != EventStatusEm.比赛完成)
                {
                    msg = "赛事未完成不能进行绑定";
                    return flag;
                }

                try
                {
                    _dbContext.BeginTransaction();
                    t_member_points points = _dbContext.Select<t_member_points>(c => c.memberId == data.memberId).FirstOrDefault();
                    _dbContext.Execute($"update t_member_points set playerPoints=playerPoints+{points.playerPoints},coachPoints=coachPoints+{points.coachPoints},refereePoints=refereePoints+{points.refereePoints} where memberId={request.MemberId}");
                    _dbContext.Execute($"update t_member_temp set tomemberId={request.MemberId},updateTime='{DateTime.Now}',tempStatus={(int)TempStatusEm.已绑定}  where id={data.id}");
                    _dbContext.Execute($"update t_member_points_record set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                    _dbContext.Execute($"update t_event_referee_signup set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                    _dbContext.Execute($"update t_event_cycling_match set refereeId={request.MemberId} where  refereeId={data.memberId} and isdelete=0");
                    _dbContext.Execute($"update t_event_cycling_match_playerresult set refereeId={request.MemberId} where refereeId={data.memberId} and isdelete=0");
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
        public List<MemberTempResponse> ListPlayer(TempMemberQueryRequest request)
        {
            List<MemberTempResponse> list = new List<MemberTempResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.TempStatus.HasValue && request.TempStatus > 0)
                {
                    join.Append(" and a.tempStatus=@TempStatus");
                }
                if (request.TempType.HasValue && request.TempType > 0)
                {
                    join.Append(" and a.tempType=@TempType");
                }
                var sql = $@"select a.*,b.completename  MemberName,c.name EventName 
                            from t_member_temp a 
                            inner join t_member_player b on a.memberId=b.memberId 
                            inner join t_event  c on a.eventId=c.Id
                            where a.isdelete=0 {join.ToString()} order by a.createtime
                        ";
                int totalCount = 0;
                list = _dbContext.Page<MemberTempResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.ListPlayer", ex);
            }
            return list;
        }
        //临时裁判数据列表
        public List<MemberTempResponse> ListReferee(TempMemberQueryRequest request)
        {
            List<MemberTempResponse> list = new List<MemberTempResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.TempStatus.HasValue && request.TempStatus > 0)
                {
                    join.Append(" and a.tempStatus=@TempStatus");
                }
                if (request.TempType.HasValue && request.TempType > 0)
                {
                    join.Append(" and a.tempType=@TempType");
                }
                var sql = $@"select a.*,b.completename  MemberName,c.name EventName  from t_member_temp a 
                            inner join t_member_referee b on a.memberId=b.memberId 
                            inner join t_event  c on a.eventId=c.Id
                            where a.isdelete=0 {join.ToString()} order by a.createtime
                        ";
                int totalCount = 0;
                list = _dbContext.Page<MemberTempResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.ListReferee", ex);
            }
            return list;
        }
        // 支付回调 进行数据迁移
        public void Callback(int memberId, int sourceId)
        {
            try
            {
                using (IDBContext dbcontext = new MySqlDBContext())
                {
                    t_member_temp temp = dbcontext.Get<t_member_temp>(sourceId);
                    if (temp != null && temp.memberId == memberId)
                    {
                        try
                        {
                            dbcontext.BeginTransaction();
                            t_member_points points = dbcontext.Select<t_member_points>(c => c.memberId == temp.memberId).FirstOrDefault();
                            dbcontext.Execute($"update t_member_points set playerPoints=playerPoints+{points.playerPoints},coachPoints=coachPoints+{points.coachPoints},refereePoints=refereePoints+{points.refereePoints} where memberId={temp.tomemberId}");
                            dbcontext.Execute($"update t_member_temp set updateTime='{DateTime.Now}',tempStatus={(int)TempStatusEm.已绑定}  where id={temp.id}");
                            dbcontext.Execute($"update t_member_points_record set memberId={temp.tomemberId} where memberId={temp.memberId} and isdelete=0");
                            dbcontext.Execute($"update t_event_player_signup set memberId={temp.tomemberId} where memberId={temp.memberId} and isdelete=0");
                            //对垒表也要修改
                            //1 循环赛
                            dbcontext.Execute($"update t_event_cycling_match_playerresult set playerId={temp.tomemberId} where playerId={temp.memberId} and isdelete=0");
                            //2 淘汰赛

                            dbcontext.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            dbcontext.Rollback();
                            LogUtils.LogError("MemberTempService.CallbackTran", ex);
                        }
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
