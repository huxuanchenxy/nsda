using Dapper;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Models;
using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.member;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.member
{
    /// <summary>
    /// 会员管理
    /// </summary>
    public class MemberService : IMemberService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        ISysOperLogService _sysOperLogService;
        public MemberService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _sysOperLogService = sysOperLogService;
        }

        //注册
        public bool Register(MemberRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Account.IsEmpty())
                {
                    msg = "电子邮箱不能为空";
                    return flag;
                }
                if (!request.Account.IsSuccessEmail())
                {
                    msg = "电子邮箱格式有误";
                    return flag;
                }
                if (request.Pwd.IsEmpty())
                {
                    msg = "密码不能为空";
                    return flag;
                }
                if (request.Pwd.Length < 6)
                {
                    msg = "密码长度不能低于6";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }

                if (request.MemberType == MemberTypeEm.选手)
                {
                    if (request.PlayerEdu == null || request.PlayerEdu.SchoolId <= 0)
                    {
                        msg = "请新增教育经历";
                        return flag;
                    }
                }
                t_member member = InsertValidate(request, out msg);
                if (msg.IsNotEmpty())
                {
                    return flag;
                }
                //判断账号是否存在
                if (_dataRepository.MemberRepo.IsExist(request.Account))
                {
                    msg = "账号已存在";
                    return flag;
                }

                try
                {
                    _dbContext.BeginTransaction();
                    member.code = _dataRepository.MemberRepo.RenderCode();
                    int memberId = _dbContext.Insert(member).ToObjInt();
                    _dbContext.Insert(new t_memberpoints
                    {
                        eventPoints = 0,
                        memberId = memberId,
                        points = 0,
                        servicePoints = 0,
                    });

                    //如果是选手  填了教育经验
                    if (request.MemberType == MemberTypeEm.选手)
                    {
                        _dbContext.Insert(new t_playereduexper
                        {
                            enddate = request.PlayerEdu.EndDate,
                            memberId = memberId,
                            schoolId = request.PlayerEdu.SchoolId,
                            startdate = request.PlayerEdu.StartDate
                        });
                    }

                    if (request.MemberType == MemberTypeEm.裁判)
                    {
                        if (request.EventId != null && request.EventId > 0)
                        {
                            _dbContext.Insert(new t_referee_signup
                            {
                                eventId = (int)request.EventId,
                                isTemp = false,
                                memberId = memberId,
                                refereeSignUpStatus = RefereeSignUpStatusEm.待审核
                            });
                        }
                    }

                    _dbContext.CommitChanges();
                    flag = true;
                    var userContext = new WebUserContext
                    {
                        Name = request.Name,
                        Account = request.Account,
                        Role = ((int)request.MemberType).ToString(),
                        MemberType = (int)request.MemberType,
                        Id = memberId,
                        Status = request.MemberType == MemberTypeEm.选手 ? (int)MemberStatusEm.待认证 : request.MemberType == MemberTypeEm.赛事管理员 ? (int)MemberStatusEm.待审核 : (int)MemberStatusEm.通过
                    };
                    SaveCurrentUser(userContext);
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("MemberService.RegisterTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Register", ex);
            }
            return flag;
        }
        //登录
        public WebUserContext Login(string account, string pwd, out string msg)
        {
            WebUserContext userContext = null;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.QueryFirstOrDefault<t_member>(@"select * from t_member where account=@account and pwd=@pwd and IsDelete=0",
                                                                        new
                                                                        {
                                                                            account = account,
                                                                            pwd = pwd
                                                                        });
                if (detail == null)
                {
                    msg = "账号或密码错误";
                }
                else
                {
                    detail.lastlogintime = DateTime.Now;
                    _dbContext.Update(detail);
                    //读取已经认证的
                    string role = ((int)detail.memberType).ToString();
                    var memberextend = _dbContext.Select<t_memberextend>(c => c.memberId == detail.id && c.memberExtendStatus == MemberExtendStatusEm.申请通过).ToList();
                    if (memberextend != null && memberextend.Count > 0)
                    {
                        foreach (var item in memberextend)
                        {
                            role += $",{((int)item.role).ToString()}";
                        }
                    }
                    //记录缓存
                    userContext = new WebUserContext
                    {
                        Id = detail.id,
                        Name = detail.name,
                        Account = detail.account,
                        Role = role,
                        MemberType = (int)detail.memberType,
                        Status = (int)detail.memberStatus
                    };
                    SaveCurrentUser(userContext);
                }
            }
            catch (Exception ex)
            {
                msg = "服务异常";
                LogUtils.LogError("MemberService.Login", ex);
            }
            return userContext;
        }
        //修改
        public bool Edit(MemberRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_member member = _dbContext.Get<t_member>(request.Id);
                if (member != null)
                {
                    member = EditValidate(member, request, out msg);
                    if (msg.IsNotEmpty())
                    {
                        return flag;
                    }
                    member.updatetime = DateTime.Now;
                    _dbContext.Update(member);
                }
                else
                {
                    msg = "找不到会员信息";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Edit", ex);
            }
            return flag;
        }

        private t_member InsertValidate(MemberRequest request, out string msg)
        {
            msg = string.Empty;
            t_member member = new t_member
            {
                account = request.Account,
                card = request.Card,
                cardType = request.CardType,
                completename = request.Name,
                completepinyin = request.CompletePinYin,
                contactaddress = request.ContactAddress,
                contactmobile = request.ContactMobile,
                emergencycontact = request.EmergencyContact,
                gender = request.Gender,
                grade = request.Grade,
                lastlogintime = DateTime.Now,
                name = request.Name,
                memberType = request.MemberType,
                pinyinname = request.PinYinName,
                pinyinsurname = request.PinYinSurName,
                pwd = request.Pwd,
                surname = request.SurName
            };
            switch (request.MemberType)
            {
                case MemberTypeEm.选手:
                case MemberTypeEm.赛事管理员:
                    if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                    {
                        msg = "姓/名不能为空";
                        return member;
                    }
                    if (request.CardType == null)
                    {
                        msg = "请选择证件类型";
                        return member;
                    }
                    if (request.Card.IsEmpty())
                    {
                        msg = "证件号不能为空";
                        return member;
                    }
                    member.completename = request.Name + request.SurName;
                    member.completepinyin = request.PinYinName + request.PinYinSurName;
                    member.memberStatus = request.MemberType == MemberTypeEm.赛事管理员 ? MemberStatusEm.待审核 : MemberStatusEm.待认证;
                    break;
                case MemberTypeEm.教练:
                case MemberTypeEm.裁判:
                    if (request.CompleteName.IsEmpty())
                    {
                        msg = "中文名不能为空";
                        return member;
                    }
                    if (request.PinYinName.IsEmpty())
                    {
                        msg = "First name不能为空";
                        return member;
                    }
                    if (request.PinYinSurName.IsEmpty())
                    {
                        msg = "Last name不能为空";
                        return member;
                    }
                    member.completepinyin = request.PinYinName + request.PinYinSurName;
                    member.memberStatus = MemberStatusEm.通过;
                    break;
            }
            return member;
        }

        private t_member EditValidate(t_member member, MemberRequest request, out string msg)
        {
            member.card = request.Card;
            member.cardType = request.CardType;
            member.completename = request.Name;
            member.completepinyin = request.CompletePinYin;
            member.contactaddress = request.ContactAddress;
            member.contactmobile = request.ContactMobile;
            member.emergencycontact = request.EmergencyContact;
            member.gender = request.Gender;
            member.grade = request.Grade;
            member.name = request.Name;
            member.memberType = request.MemberType;
            member.pinyinname = request.PinYinName;
            member.pinyinsurname = request.PinYinSurName;
            member.surname = request.SurName;
            msg = string.Empty;
            switch (member.memberType)
            {
                case MemberTypeEm.选手:
                case MemberTypeEm.赛事管理员:
                    if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                    {
                        msg = "姓/名不能为空";
                        return member;
                    }
                    if (!request.CardType.HasValue)
                    {
                        msg = "请选择证件类型";
                        return member;
                    }
                    if (request.Card.IsEmpty())
                    {
                        msg = "证件号不能为空";
                        return member;
                    }
                    member.completename = request.Name + request.SurName;
                    member.completepinyin = request.PinYinName + request.PinYinSurName;
                    break;
                case MemberTypeEm.教练:
                case MemberTypeEm.裁判:
                    if (request.CompleteName.IsEmpty())
                    {
                        msg = "中文名不能为空";
                        return member;
                    }
                    if (request.PinYinName.IsEmpty())
                    {
                        msg = "First name不能为空";
                        return member;
                    }
                    if (request.PinYinSurName.IsEmpty())
                    {
                        msg = "Last  name不能为空";
                        return member;
                    }
                    member.completepinyin = request.PinYinName + request.PinYinSurName;
                    break;
            }
            return member;
        }

        //修改密码
        public bool EditPwd(int memberId, string oldPwd, string newPwd, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (oldPwd.IsEmpty())
                {
                    msg = "原密码不能为空";
                    return flag;
                }

                if (newPwd.IsEmpty())
                {
                    msg = "新密码不能为空";
                    return flag;
                }

                if (newPwd.Length < 6)
                {
                    msg = "密码长度不能低于6";
                    return flag;
                }

                if (string.Equals(oldPwd, newPwd))
                {
                    msg = "新密码和原密码相同";
                    return flag;
                }

                var member = _dbContext.Get<t_member>(memberId);
                if (member != null)
                {
                    if (!string.Equals(oldPwd, member.pwd, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "原密码有误";
                    }
                    else
                    {
                        member.pwd = newPwd;
                        member.updatetime = DateTime.Now;
                        _dbContext.Update(member);
                        flag = true;
                    }
                }
                else
                {
                    msg = "修改有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.UpdatePwd", ex);
            }
            return flag;
        }
        //实名认证回调 修改用户状态
        public void CallBack(int id)
        {
            try
            {
                var member = _dbContext.Get<t_member>(id);
                if (member != null)
                {
                    member.updatetime = DateTime.Now;
                    member.memberStatus = MemberStatusEm.已认证;

                    string data = SessionCookieUtility.GetSession($"webusersession_{id}");
                    if (data.IsNotEmpty())
                    {
                        //读取已经认证的
                        string role = ((int)member.memberType).ToString();
                        var memberextend = _dbContext.Select<t_memberextend>(c => c.memberId == member.id && c.memberExtendStatus == MemberExtendStatusEm.申请通过).ToList();
                        if (memberextend != null && memberextend.Count > 0)
                        {
                            foreach (var item in memberextend)
                            {
                                role += $",{((int)item.role).ToString()}";
                            }
                        }
                        //记录缓存
                        var userContext = new WebUserContext
                        {
                            Id = member.id,
                            Name = member.name,
                            Account = member.account,
                            Role = role,
                            MemberType = (int)member.memberType,
                            Status = (int)member.memberStatus
                        };
                        SessionCookieUtility.WriteSession($"webusersession_{id}", MemberEncoderAndDecoder.encrypt(userContext.Serialize()));
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.CallBack", ex);
            }
        }
        //会员列表 
        public List<MemberResponse> List(MemberQueryRequest request)
        {
            List<MemberResponse> list = new List<MemberResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Account.IsNotEmpty())
                {
                    request.Account = "%" + request.Account + "%";
                    join.Append(" and account like @Account");
                }
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    join.Append(" and completename like @Name");
                }

                if (request.Mobile.IsNotEmpty())
                {
                    request.Mobile = "%" + request.Mobile + "%";
                    join.Append(" and contactmobile like @Mobile");
                }

                if (request.MemberStatus.HasValue&&request.MemberStatus>0)
                {
                    join.Append(" and memberStatus=@MemberStatus");
                }

                if (request.MemberType.HasValue && request.MemberType > 0)
                {
                    join.Append(" and memberType=@MemberType");
                }
                var sql=$@"select * from t_member where isdelete=0 
                           and memberType!={(int)MemberTypeEm.临时裁判} and memberType!={(int)MemberTypeEm.临时选手} 
                           {join.ToString()} order by createtime desc
                         ";               
                int totalCount = 0;
                list = _dbContext.Page<MemberResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.List", ex);
            }
            return list;
        }
        //删除会员信息
        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var member = _dbContext.Get<t_member>(id);
                if (member != null)
                {
                    member.isdelete = true;
                    member.updatetime = DateTime.Now;
                    _dbContext.Update(member);
                    DeleteCurrentUser(id);//清除用户缓存
                    flag = true;
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Delete", ex);
            }
            return flag;
        }
        //重置密码
        public bool Reset(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var member = _dbContext.Get<t_member>(id);
                if (member != null)
                {
                    member.pwd = "159357";
                    member.updatetime = DateTime.Now;
                    _dbContext.Update(member);
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Reset", ex);
            }
            return flag;
        }
        //找回密码
        public bool FindPwd(int memberId, string pwd, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (pwd.IsEmpty())
                {
                    msg = "密码不能为空";
                    return flag;
                }
                var member = _dbContext.Get<t_member>(memberId);
                if (member != null)
                {
                    member.pwd = pwd;
                    member.updatetime = DateTime.Now;
                    _dbContext.Update(member);
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.FindPwd", ex);
            }
            return flag;
        }
        //验证邮箱是否有效 并返回用户id
        public int SendEmail(string email, out string msg)
        {
            int memberId = 0;
            msg = string.Empty;
            try
            {
                if (email.IsEmpty())
                {
                    msg = "邮箱不能为空";
                    return memberId;
                }

                if (!email.IsSuccessEmail())
                {
                    msg = "邮箱有误";
                    return memberId;
                }
                var member = _dbContext.Select<t_member>(c => c.account == email && c.memberType != MemberTypeEm.临时裁判 && c.memberType != MemberTypeEm.临时选手).FirstOrDefault();
                if (member != null)
                {
                    memberId = member.id;
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                msg = "服务异常";
                LogUtils.LogError("MemberService.SendEmail", ex);
            }
            return memberId;
        }
        //会员详情
        public MemberResponse Detail(int id)
        {
            MemberResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_member>(id);
                if (detail != null)
                {
                    response = new MemberResponse
                    {
                        Account = detail.account,
                        Name = detail.name,
                        Card = detail.card,
                        CardType = detail.cardType,
                        Code = detail.code,
                        CompleteName = detail.completename,
                        ContactAddress = detail.contactaddress,
                        ContactMobile = detail.contactmobile,
                        CompletePinYin = detail.completepinyin,
                        EmergencyContact = detail.emergencycontact,
                        Gender = detail.gender,
                        CreateTime = detail.createtime,
                        Grade = detail.grade,
                        Id = detail.id,
                        MemberStatus = detail.memberStatus,
                        MemberType = detail.memberType,
                        PinYinName = detail.pinyinname,
                        PinYinSurName = detail.pinyinsurname,
                        SurName = detail.surname,
                        UpdateTime = detail.updatetime
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.Detail", ex);
            }
            return response;
        }

        //审核赛事管理员账号
        public bool Check(int id, string remark, bool isAppro, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_member>(id);
                if (detail != null && detail.memberType == MemberTypeEm.赛事管理员)
                {
                    detail.updatetime = DateTime.Now;
                    detail.memberStatus = isAppro ? MemberStatusEm.通过 : MemberStatusEm.拒绝;
                    _dbContext.Update(detail);
                    DeleteCurrentUser(id);//清除用户缓存 使其重新登录
                    flag = true;
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.Force", ex);
            }
            return flag;
        }
        //强制认证选手账号
        public bool Force(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_member>(id);
                if (detail != null && detail.memberStatus == MemberStatusEm.待认证)
                {
                    detail.updatetime = DateTime.Now;
                    detail.memberStatus = MemberStatusEm.已认证;
                    _dbContext.Update(detail);
                    DeleteCurrentUser(id);//清除用户缓存 使其重新登录
                    flag = true;
                }
                else
                {
                    msg = "会员信息不存在";
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.Force", ex);
            }
            return flag;
        }
        /// <summary>
        /// 保存用户缓存
        /// </summary>
        private void SaveCurrentUser(WebUserContext context)
        {
            try
            {
                DateTime expireTime = DateTime.Now.AddHours(12);
                SessionCookieUtility.WriteCookie(Constant.WebCookieKey, MemberEncoderAndDecoder.encrypt($"webusersession_{context.Id}"), expireTime);
                string data = MemberEncoderAndDecoder.encrypt(context.Serialize());
                SessionCookieUtility.WriteSession($"webusersession_{context.Id}", data);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SaveCurrentUser", ex);
            }
        }

        private void DeleteCurrentUser(int id)
        {
            string data = SessionCookieUtility.GetSession($"webusersession_{id}");
            if (data.IsNotEmpty())
            {
                SessionCookieUtility.RemoveSession($"webusersession_{id}");
            }
        }
        // 选手下拉框
        public List<MemberSelectResponse> SelectPlayer(string keyvalue, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                if (keyvalue.IsEmpty())
                {
                    return list;
                }
                //查询注册号 为选手号 或者扩展有选手的会员
                var sql = $@"
                            select Id,Code,completename as Name from t_member where (isdelete=0 
                            and memberType={MemberTypeEm.选手} and id!={memberId} and (code like @key or completename like @key)) or id in
                            (
	                            select a.memberId from t_memberextend a
	                            inner join t_member b on a.memberId=b.id
	                            where a.memberId!={memberId} and a.memberExtendStatus={MemberExtendStatusEm.申请通过} and a.role={RoleEm.选手} 
                                and (b.code like @key or b.completename like @key)
                            ) limit 30
                         ";
                var dy = new DynamicParameters();
                dy.Add("key", "%" + keyvalue + "%");
                list = _dbContext.Query<MemberSelectResponse>(sql, dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SelectPlayer", ex);
            }
            return list;
        }
        // 教练下拉框
        public List<MemberSelectResponse> SelectTrainer(string keyvalue, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                if (keyvalue.IsEmpty())
                {
                    return list;
                }
                //查询注册号 为教练号 或者扩展有教练的会员
                var sql = $@"
                            select Id,Code,completename as Name from t_member where (isdelete=0 
                            and memberType={MemberTypeEm.教练} and id!={memberId} and (code like @key or completename like @key)) or id in
                            (
	                            select a.memberId from t_memberextend a
	                            inner join t_member b on a.memberId=b.id
	                            where and a.memberId!={memberId} a.memberExtendStatus={MemberExtendStatusEm.申请通过} and a.role={RoleEm.教练} 
                                and (b.code like @key or b.completename like @key)
                            ) limit 30
                         ";
                var dy = new DynamicParameters();
                dy.Add("key", "%" + keyvalue + "%");
                list = _dbContext.Query<MemberSelectResponse>(sql, dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SelectTrainer", ex);
            }
            return list;
        }
        // 去支付
        public bool GoPay(int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                t_member member = _dbContext.Get<t_member>(memberId);
                if (member != null && member.memberStatus == MemberStatusEm.待认证)
                {
                    t_order order = _dbContext.Select<t_order>(c => c.memberId == memberId && c.orderType == OrderTypeEm.实名认证).FirstOrDefault();
                    if (order != null)
                    {
                        if (order.orderStatus != OrderStatusEm.等待支付 && order.orderStatus != OrderStatusEm.支付失败)
                        {

                        }
                        else
                        {
                            msg = "状态已改变";
                        }
                    }
                    else
                    {
                        var orderid = _dbContext.Insert(new t_order
                        {
                            isNeedInvoice = false,
                            mainOrderId = null,
                            memberId = memberId,
                            money = Constant.AuthMoney,
                            orderStatus = OrderStatusEm.等待支付,
                            orderType = OrderTypeEm.实名认证,
                            payExpiryDate = DateTime.Now.AddYears(3),
                            remark = "实名认证",
                            sourceId = memberId,
                            totalcoupon = 0,
                            totaldiscount = 0
                        }).ToObjInt();
                        _dbContext.Insert(new t_orderdetail
                        {
                            memberId = memberId,
                            orderId = orderid,
                            coupon = 0,
                            discountprice = 0,
                            money = Constant.AuthMoney,
                            productId = 0,
                            name = "会员认证",
                            number = 1,
                            unitprice = Constant.AuthMoney
                        });
                        //生成支付链接
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.Force", ex);
            }
            return flag;
        }
        //检测账号是否存在
        public bool IsExist(string account)
        {
            bool flag = false;
            try
            {
                if (account.IsEmpty())
                {
                    return flag;
                }
                if (_dataRepository.MemberRepo.IsExist(account))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.IsExist", ex);
            }
            return flag;
        }
    }
}
