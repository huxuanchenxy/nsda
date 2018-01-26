using Dapper;
using nsda.Model;
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
        public WebUserContext Login(LoginRequest request, out string msg)
        {
            WebUserContext userContext = null;
            msg = string.Empty;
            try
            {
                var sql = string.Empty;
                if (request.MemberType == MemberTypeEm.赛事管理员)
                {
                   sql= @"select a.*,b.completeName Name from t_member a 
                          inner join t_member_event b on a.id=b.memberId
                          where a.account=@account and a.pwd=@pwd and a.isdelete=0 
                        ";
                }
                else if (request.MemberType == MemberTypeEm.选手)
                {
                    sql = @"select a.*,b.completename Name,c.points  from t_member_player a 
                          inner join t_member_event b on a.id=b.memberId
                          inner join t_member_points c on a.id=c.memberId
                          where a.account=@account and a.pwd=@pwd and a.isdelete=0 
                        ";
                }
                else if (request.MemberType == MemberTypeEm.教练)
                {
                    sql = @"select a.*,b.completepinyin Name,c.points  from t_member_coach a 
                          inner join t_member_event b on a.id=b.memberId
                          inner join t_member_points c on a.id=c.memberId
                          where a.account=@account and a.pwd=@pwd and a.isdelete=0 
                        ";
                }
                else if (request.MemberType == MemberTypeEm.裁判)
                {
                    sql = @"select a.*,b.completename Name,c.points  from t_member_referee a 
                          inner join t_member_event b on a.id=b.memberId
                          inner join t_member_points c on a.id=c.memberId
                          where a.account=@account and a.pwd=@pwd and a.isdelete=0 
                        ";
                }
                var dy = new DynamicParameters();
                dy.Add("account",request.Account);
                dy.Add("pwd",request.Pwd);
                var data = _dbContext.QueryFirstOrDefault<dynamic>(sql, dy);
                if (data == null)
                {
                    msg = "账号或密码错误";
                }
                else
                {
                    _dbContext.Update($"update t_member set lastlogintime={DateTime.Now} where id={data.id}");
                    //记录缓存
                    userContext = new WebUserContext
                    {
                        Id = data.id,
                        Name = data.Name,
                        Account = data.account,
                        Code = data.code,
                        IsExtendCoach = data.isExtendCoach,
                        IsExtendReferee = data.isExtendReferee,
                        IsExtendPlayer = data.isExtendPlayer,
                        MemberType = (int)data.memberType,
                        Points = request.MemberType != MemberTypeEm.赛事管理员?data.points:0,
                        MemberStatus=(int)data.memberStatus
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
        //修改密码
        public bool EditPwd(int memberId, string pwd, out string msg)
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

                if (pwd.IsEmpty())
                {
                    msg = "密码长度不能低于6位";
                    return flag;
                }

                var member = _dbContext.Get<t_member>(memberId);
                if (member != null)
                {
                    member.pwd = pwd;
                    member.updatetime = DateTime.Now;
                    _dbContext.Update(member);
                    flag = true;
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
                using (IDBContext dbcontext = new MySqlDBContext())
                {
                    var member = dbcontext.Get<t_member>(id);
                    if (member != null && (member.memberType == MemberTypeEm.选手 || member.isExtendPlayer))
                    {
                        member.updatetime = DateTime.Now;
                        member.memberStatus = MemberStatusEm.已认证;
                        dbcontext.Update(member);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.CallBack", ex);
            }
        }
        //账号轮询
        public bool MemberPlayerPolling(WebUserContext userContext)
        {
            bool flag = false;
            try
            {
                var member = _dbContext.Get<t_member>(userContext.Id);
                if (member.memberStatus == MemberStatusEm.已认证)
                {
                    flag = true;
                    userContext.MemberStatus=(int)MemberStatusEm.已认证;
                    SaveCurrentUser(userContext);
                }

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.MemberPlayerPolling", ex);
            }
            return flag;
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
                if (request.MemberStatus.HasValue && request.MemberStatus > 0)
                {
                    join.Append(" and memberStatus=@MemberStatus");
                }

                if (request.MemberType.HasValue && request.MemberType > 0)
                {
                    join.Append(" and memberType=@MemberType");
                }
                var sql = $@"select * from t_member where isdelete=0 
                           and memberType not in ({ParamsConfig._tempmembertype}) 
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
                LogUtils.LogError("MemberService.Reset", ex);
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
                LogUtils.LogError("MemberService.Force", ex);
            }
            return flag;
        }
        // 选手下拉框
        public List<MemberSelectResponse> SelectPlayer(string key, string value, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                if (value.IsEmpty() || key.IsEmpty())
                {
                    return list;
                }
                if (key != "code" && key != "completename")
                {
                    return list;
                }
                //查询注册号 为选手号 或者扩展有选手的会员
                var sql = $@"
                            select memberId MemberId,code MemberCode,completename MemberName from t_member_player 
                            where  isdelete=0 and memberId!={memberId} and {key} like @value limit 30
                           ";
                var dy = new DynamicParameters();
                dy.Add("value", "%" + value + "%");
                list = _dbContext.Query<MemberSelectResponse>(sql, dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SelectPlayer", ex);
            }
            return list;
        }
        // 教练下拉框
        public List<MemberSelectResponse> SelectCoach(string key, string value, int memberId)
        {
            List<MemberSelectResponse> list = new List<MemberSelectResponse>();
            try
            {
                if (value.IsEmpty() || key.IsEmpty())
                {
                    return list;
                }
                if (key != "code" && key != "completepinyin")
                {
                    return list;
                }
                //查询注册号 为教练号 或者扩展有教练的会员
                var sql = $@"
                            select memberId MemberId,code MemberCode,completepinyin MemberName from t_member_coach 
                            where  isdelete=0 and memberId!={memberId} and {key} like @value limit 30
                         ";
                var dy = new DynamicParameters();
                dy.Add("value", "%" + value + "%");
                list = _dbContext.Query<MemberSelectResponse>(sql, dy).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SelectCoach", ex);
            }
            return list;
        }
        // 去支付
        public int GoAuth(int memberId, out string msg)
        {
            int orderId = 0;
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
                            orderId = order.id;
                        }
                        else
                        {
                            msg = "状态已改变";
                        }
                    }
                    else
                    {
                        try
                        {
                            _dbContext.BeginTransaction();
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

                            _dbContext.Insert(new t_order_detail
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
                            _dbContext.CommitChanges();
                            orderId = orderid;
                        }
                        catch (Exception ex)
                        {
                            _dbContext.Rollback();
                            LogUtils.LogError("MemberService.GoAuthTran", ex);
                        }
                    }
                }
                else
                {
                    msg = "请刷新页面重试";
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.GoAuth", ex);
            }
            return orderId;
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
        //替换头像
        public bool ReplaceHead(string headUrl, int memberId)
        {
            bool flag = false;
            try
            {
                if (headUrl.IsEmpty())
                {
                    return flag;
                }
                t_member member = _dbContext.Get<t_member>(memberId);
                if (member != null)
                {
                    member.updatetime = DateTime.Now;
                    member.head = headUrl;
                    _dbContext.Update(member);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                LogUtils.LogError("MemberService.ReplaceHead", ex);
            }
            return flag;
        }

        #region 注册
        // 选手
        public bool RegisterMemberPlayer(RegisterPlayerRequest request, out string msg)
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
                if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                {
                    msg = "姓/名不能为空";
                    return flag;
                }
                if (request.PinYinName.IsEmpty() || request.PinYinSurName.IsEmpty())
                {
                    msg = "拼音姓/名不能为空";
                    return flag;
                }
                if (request.Card.IsEmpty())
                {
                    msg = "证件号不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }

                if (request.PlayerEdu == null || request.PlayerEdu.SchoolId == 0)
                {
                    msg = "教育经历不能为空";
                    return flag;
                }
                if (request.PlayerEdu.StartDate.IsEmpty())
                {
                    msg = "教育经历开始时间不能为空";
                    return flag;
                }

                t_member member = new t_member
                {
                    account = request.Account,
                    head = "",//默认头像地址
                    isExtendCoach = false,
                    isExtendPlayer = false,
                    isExtendReferee = false,
                    lastlogintime = DateTime.Now,
                    memberStatus = MemberStatusEm.待认证,
                    memberType = MemberTypeEm.选手,
                    pwd = request.Pwd
                };
                t_member_player member_player = new t_member_player
                {
                    card = request.Card,
                    cardType = request.CardType,
                    completename = $"{request.Name}{request.SurName}",
                    completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                    contactaddress = request.ContactAddress,
                    contactmobile = request.ContactMobile,
                    emergencycontact = request.EmergencyContact,
                    gender = request.Gender,
                    grade = request.Grade,
                    name = request.Name,
                    pinyinname = request.PinYinName,
                    pinyinsurname = request.PinYinSurName,
                    surname = request.SurName,
                    emergencycontactmobile = request.EmergencyContactMobile
                };
                try
                {
                    _dbContext.BeginTransaction();
                    member.code = _dataRepository.MemberRepo.RenderCode();
                    int memberId = _dbContext.Insert(member).ToObjInt();
                    member_player.code = member.code;
                    member_player.memberId = memberId;
                    _dbContext.Insert(member_player);
                    _dbContext.Insert(new t_member_points
                    {
                        eventPoints = 0,
                        memberId = memberId,
                        points = 0,
                        servicePoints = 0,
                    });
                    _dbContext.Insert(new t_player_edu
                    {
                        enddate = request.PlayerEdu.EndDate,
                        memberId = memberId,
                        schoolId = request.PlayerEdu.SchoolId,
                        startdate = request.PlayerEdu.StartDate
                    });

                    _dbContext.CommitChanges();
                    flag = true;

                    SaveCurrentUser(new WebUserContext
                    {
                        Code = member.code,
                        Name = member_player.completename,
                        Account = request.Account,
                        MemberType = (int)MemberTypeEm.选手,
                        Id = memberId,
                        IsExtendCoach = false,
                        IsExtendPlayer = false,
                        IsExtendReferee = false
                    });
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("MemberService.RegisterPlayerTran", ex);
                }

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.RegisterPlayer", ex);
            }
            return flag;
        }
        // 教练
        public bool RegisterMemberCoach(RegisterCoachRequest request, out string msg)
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

                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                t_member member = new t_member
                {
                    account = request.Account,
                    head = "",//默认头像地址
                    isExtendCoach = false,
                    isExtendPlayer = false,
                    isExtendReferee = false,
                    lastlogintime = DateTime.Now,
                    memberStatus = MemberStatusEm.已认证,
                    memberType = MemberTypeEm.教练,
                    pwd = request.Pwd
                };
                t_member_coach member_coach = new t_member_coach
                {
                    completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                    contactaddress = request.ContactAddress,
                    contactmobile = request.ContactMobile,
                    emergencycontact = request.EmergencyContact,
                    gender = request.Gender,
                    pinyinname = request.PinYinName,
                    pinyinsurname = request.PinYinSurName,
                    emergencycontactmobile = request.EmergencyContactMobile
                };
                try
                {
                    _dbContext.BeginTransaction();
                    member.code = _dataRepository.MemberRepo.RenderCode();
                    int memberId = _dbContext.Insert(member).ToObjInt();
                    member_coach.code = member.code;
                    member_coach.memberId = memberId;
                    _dbContext.Insert(member_coach);
                    _dbContext.Insert(new t_member_points
                    {
                        eventPoints = 0,
                        memberId = memberId,
                        points = 0,
                        servicePoints = 0,
                    });
                    _dbContext.CommitChanges();
                    flag = true;

                    SaveCurrentUser(new WebUserContext
                    {
                        Code = member.code,
                        Name = member_coach.completepinyin,
                        Account = request.Account,
                        MemberType = (int)MemberTypeEm.裁判,
                        Id = memberId,
                        IsExtendCoach = false,
                        IsExtendPlayer = false,
                        IsExtendReferee = false
                    });
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("MemberService.RegisterCoachTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.RegisterCoach", ex);
            }
            return flag;
        }
        // 裁判
        public bool RegisterMemberReferee(RegisterRefereeRequest request, out string msg)
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

                if (request.CompleteName.IsEmpty())
                {
                    msg = "中文名不能为空";
                    return flag;
                }

                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                t_member member = new t_member
                {
                    account = request.Account,
                    head = "",//默认头像地址
                    isExtendCoach = false,
                    isExtendPlayer = false,
                    isExtendReferee = false,
                    lastlogintime = DateTime.Now,
                    memberStatus = MemberStatusEm.已认证,
                    memberType = MemberTypeEm.裁判,
                    pwd = request.Pwd
                };
                t_member_referee member_referee = new t_member_referee
                {
                    completename = request.CompleteName,
                    completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                    contactaddress = request.ContactAddress,
                    contactmobile = request.ContactMobile,
                    emergencycontact = request.EmergencyContact,
                    gender = request.Gender,
                    pinyinname = request.PinYinName,
                    pinyinsurname = request.PinYinSurName,
                    emergencycontactmobile = request.EmergencyContactMobile
                };
                try
                {
                    _dbContext.BeginTransaction();
                    member.code = _dataRepository.MemberRepo.RenderCode();
                    int memberId = _dbContext.Insert(member).ToObjInt();
                    member_referee.code = member.code;
                    member_referee.memberId = memberId;
                    _dbContext.Insert(member_referee);
                    _dbContext.Insert(new t_member_points
                    {
                        eventPoints = 0,
                        memberId = memberId,
                        points = 0,
                        servicePoints = 0,
                    });

                    if (request.EventId != null && request.EventId > 0)
                    {
                        _dbContext.Insert(new t_event_referee_signup
                        {
                            eventId = (int)request.EventId,
                            isTemp = false,
                            memberId = memberId,
                            refereeSignUpStatus = RefereeSignUpStatusEm.待审核
                        });
                    }

                    _dbContext.CommitChanges();
                    flag = true;

                    SaveCurrentUser(new WebUserContext
                    {
                        Code = member.code,
                        Name = member_referee.completename,
                        Account = request.Account,
                        MemberType = (int)MemberTypeEm.裁判,
                        Id = memberId,
                        IsExtendCoach = false,
                        IsExtendPlayer = false,
                        IsExtendReferee = false
                    });
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("MemberService.RegisterRefereeTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.RegisterReferee", ex);
            }
            return flag;
        }
        // 赛事管理员
        public bool RegisterMemberEvent(RegisterEventRequest request, out string msg)
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
                if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                {
                    msg = "姓/名不能为空";
                    return flag;
                }
                if (request.PinYinName.IsEmpty() || request.PinYinSurName.IsEmpty())
                {
                    msg = "拼音姓/名不能为空";
                    return flag;
                }
                if (request.Card.IsEmpty())
                {
                    msg = "证件号不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }

                t_member member = new t_member
                {
                    account = request.Account,
                    head = "",//默认头像地址
                    isExtendCoach = false,
                    isExtendPlayer = false,
                    isExtendReferee = false,
                    lastlogintime = DateTime.Now,
                    memberStatus = MemberStatusEm.已认证,
                    memberType = MemberTypeEm.赛事管理员,
                    pwd = request.Pwd
                };
                t_member_event member_event = new t_member_event
                {
                    card = request.Card,
                    cardType = request.CardType,
                    completename = $"{request.Name}{request.SurName}",
                    completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                    contactaddress = request.ContactAddress,
                    contactmobile = request.ContactMobile,
                    emergencycontact = request.EmergencyContact,
                    name = request.Name,
                    pinyinname = request.PinYinName,
                    pinyinsurname = request.PinYinSurName,
                    surname = request.SurName,
                    emergencycontactmobile = request.EmergencyContactMobile,
                    gender = request.Gender
                };
                try
                {
                    _dbContext.BeginTransaction();
                    member.code = _dataRepository.MemberRepo.RenderCode();
                    int memberId = _dbContext.Insert(member).ToObjInt();
                    member_event.code = member.code;
                    member_event.memberId = memberId;
                    _dbContext.Insert(member_event);
                    _dbContext.CommitChanges();
                    flag = true;

                    SaveCurrentUser(new WebUserContext
                    {
                        Code = member.code,
                        Name = request.Name,
                        Account = request.Account,
                        MemberType = (int)MemberTypeEm.赛事管理员,
                        Id = memberId,
                        IsExtendCoach = false,
                        IsExtendPlayer = false,
                        IsExtendReferee = false
                    });
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("MemberService.RegisterPlayerTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.RegisterEvent", ex);
            }
            return flag;
        }
        #endregion

        #region 修改账号信息
        //修改选手
        public bool EditMemberPlayer(RegisterPlayerRequest request,WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                {
                    msg = "姓/名不能为空";
                    return flag;
                }
                if (request.PinYinName.IsEmpty() || request.PinYinSurName.IsEmpty())
                {
                    msg = "拼音姓/名不能为空";
                    return flag;
                }
                if (request.Card.IsEmpty())
                {
                    msg = "证件号不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member_player>(request.Id);
                if (detail != null)
                {
                    detail.card = request.Card;
                    detail.cardType = request.CardType;
                    detail.completename = $"{request.Name}{request.SurName}";
                    detail.surname = request.SurName;
                    detail.name = request.Name;
                    detail.pinyinname = request.PinYinName;
                    detail.pinyinsurname = request.PinYinSurName;
                    detail.completepinyin = $"{request.PinYinName}{request.PinYinSurName}";
                    detail.contactaddress = request.ContactAddress;
                    detail.contactmobile = request.ContactMobile;
                    detail.emergencycontact = request.EmergencyContact;
                    detail.emergencycontactmobile = request.EmergencyContactMobile;
                    detail.gender = request.Gender;
                    detail.grade = request.Grade;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;

                    userContext.Name = detail.completename;
                    SaveCurrentUser(userContext);
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
                LogUtils.LogError("MemberService.EditPlayer", ex);
            }
            return flag;
        }
        //修改教练
        public bool EditMemberCoach(RegisterCoachRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member_coach>(request.Id);
                if (detail != null)
                {
                    detail.completepinyin = $"{request.PinYinName}{request.PinYinSurName}";
                    detail.contactaddress = request.ContactAddress;
                    detail.gender = request.Gender;
                    detail.emergencycontact = request.EmergencyContact;
                    detail.emergencycontactmobile = request.EmergencyContactMobile;
                    detail.pinyinname = request.PinYinName;
                    detail.pinyinsurname = request.PinYinSurName;
                    detail.contactmobile = request.ContactMobile;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                    userContext.Name = detail.completepinyin;
                    SaveCurrentUser(userContext);
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
                LogUtils.LogError("MemberService.EditCoach", ex);
            }
            return flag;
        }
        //修改裁判
        public bool EditMemberReferee(RegisterRefereeRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.CompleteName.IsEmpty())
                {
                    msg = "中文名不能为空";
                    return flag;
                }

                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member_referee>(request.Id);
                if (detail != null)
                {
                    detail.completename = request.CompleteName;
                    detail.pinyinname = request.PinYinName;
                    detail.pinyinsurname = request.PinYinSurName;
                    detail.completepinyin = $"{request.PinYinName}{request.PinYinSurName}";
                    detail.contactmobile = request.ContactMobile;
                    detail.contactaddress = request.ContactAddress;
                    detail.emergencycontact = request.EmergencyContact;
                    detail.emergencycontactmobile = request.EmergencyContactMobile;
                    detail.gender = request.Gender;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                    userContext.Name = detail.completename;
                    SaveCurrentUser(userContext);
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
                LogUtils.LogError("MemberService.EditReferee", ex);
            }
            return flag;
        }
        //修改赛事管理员
        public bool EditMemberEvent(RegisterEventRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                {
                    msg = "姓/名不能为空";
                    return flag;
                }
                if (request.PinYinName.IsEmpty() || request.PinYinSurName.IsEmpty())
                {
                    msg = "拼音姓/名不能为空";
                    return flag;
                }
                if (request.Card.IsEmpty())
                {
                    msg = "证件号不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member_event>(request.Id);
                if (detail != null)
                {
                    detail.card = request.Card;
                    detail.cardType = request.CardType;
                    detail.completename = $"{request.Name}{request.SurName}";
                    detail.name = request.Name;
                    detail.surname = request.SurName;
                    detail.pinyinname = request.PinYinName;
                    detail.pinyinsurname = request.PinYinSurName;
                    detail.completepinyin = $"{request.PinYinName}{request.PinYinSurName}";
                    detail.emergencycontact = request.EmergencyContact;
                    detail.emergencycontactmobile = request.EmergencyContactMobile;
                    detail.gender = request.Gender;
                    detail.contactaddress = request.ContactAddress;
                    detail.contactmobile = request.ContactMobile;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                    userContext.Name = detail.completename;
                    SaveCurrentUser(userContext);
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
                LogUtils.LogError("MemberService.EditEvent", ex);
            }
            return flag;
        }
        #endregion

        #region 账号扩展
        // 选手
        public bool ExtendPlayer(RegisterPlayerRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty() || request.SurName.IsEmpty())
                {
                    msg = "姓/名不能为空";
                    return flag;
                }
                if (request.PinYinName.IsEmpty() || request.PinYinSurName.IsEmpty())
                {
                    msg = "拼音姓/名不能为空";
                    return flag;
                }
                if (request.Card.IsEmpty())
                {
                    msg = "证件号不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }

                if (request.PlayerEdu == null || request.PlayerEdu.SchoolId == 0)
                {
                    msg = "教育经历不能为空";
                    return flag;
                }
                if (request.PlayerEdu.StartDate.IsEmpty())
                {
                    msg = "教育经历开始时间不能为空";
                    return flag;
                }

                var detail = _dbContext.Get<t_member>(request.Id);
                if (detail != null)
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        detail.memberStatus = MemberStatusEm.待认证;
                        detail.updatetime = DateTime.Now;
                        detail.isExtendPlayer = true;
                        _dbContext.Update(detail);
                        _dbContext.Insert(new t_member_player
                        {
                            memberId = detail.id,
                            code = detail.code,
                            card = request.Card,
                            cardType = request.CardType,
                            completename = $"{request.Name}{request.SurName}",
                            completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                            contactaddress = request.ContactAddress,
                            contactmobile = request.ContactMobile,
                            emergencycontact = request.EmergencyContact,
                            gender = request.Gender,
                            grade = request.Grade,
                            name = request.Name,
                            pinyinname = request.PinYinName,
                            pinyinsurname = request.PinYinSurName,
                            surname = request.SurName,
                            emergencycontactmobile = request.EmergencyContactMobile
                        });
                        _dbContext.Insert(new t_player_edu
                        {
                            enddate = request.PlayerEdu.EndDate,
                            memberId = detail.id,
                            schoolId = request.PlayerEdu.SchoolId,
                            startdate = request.PlayerEdu.StartDate
                        });
                        _dbContext.CommitChanges();
                        flag = true;

                        userContext.MemberStatus = (int)MemberStatusEm.待认证;
                        userContext.IsExtendPlayer = detail.isExtendPlayer;
                        SaveCurrentUser(userContext);
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("MemberService.ExtendPlayerTran", ex);
                    }
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
                LogUtils.LogError("MemberService.ExtendPlayer", ex);
            }
            return flag;
        }
        // 教练
        public bool ExtendCoach(RegisterCoachRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member>(request.Id);
                if (detail != null)
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        detail.updatetime = DateTime.Now;
                        detail.isExtendCoach = true;
                        _dbContext.Update(detail);
                        _dbContext.Insert(new t_member_coach
                        {
                            completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                            contactaddress = request.ContactAddress,
                            contactmobile = request.ContactMobile,
                            emergencycontact = request.EmergencyContact,
                            gender = request.Gender,
                            pinyinname = request.PinYinName,
                            pinyinsurname = request.PinYinSurName,
                            emergencycontactmobile = request.EmergencyContactMobile,
                            memberId = detail.id,
                            code = detail.code
                        });
                        _dbContext.CommitChanges();
                        flag = true;

                        userContext.IsExtendPlayer = detail.isExtendCoach;
                        SaveCurrentUser(userContext);
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("MemberService.ExtendCoachTran", ex);
                    }
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
                LogUtils.LogError("MemberService.ExtendCoach", ex);
            }
            return flag;
        }
        // 裁判
        public bool ExtendReferee(RegisterRefereeRequest request, WebUserContext userContext, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.CompleteName.IsEmpty())
                {
                    msg = "中文名不能为空";
                    return flag;
                }

                if (request.PinYinName.IsEmpty())
                {
                    msg = "First Name不能为空";
                    return flag;
                }

                if (request.PinYinSurName.IsEmpty())
                {
                    msg = "Last Name不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }
                if (request.EmergencyContact.IsEmpty())
                {
                    msg = "紧急联系人不能为空";
                    return flag;
                }
                if (request.EmergencyContactMobile.IsEmpty())
                {
                    msg = "紧急联系人电话不能为空";
                    return flag;
                }
                if (request.ContactAddress.IsEmpty())
                {
                    msg = "联系地址不能为空";
                    return flag;
                }
                var detail = _dbContext.Get<t_member>(request.Id);
                if (detail != null)
                {
                    try
                    {
                        _dbContext.BeginTransaction();
                        detail.updatetime = DateTime.Now;
                        detail.isExtendReferee = true;
                        _dbContext.Update(detail);
                        t_member_referee member_referee = new t_member_referee
                        {
                            completename = request.CompleteName,
                            completepinyin = $"{request.PinYinName}{request.PinYinSurName}",
                            contactaddress = request.ContactAddress,
                            contactmobile = request.ContactMobile,
                            emergencycontact = request.EmergencyContact,
                            gender = request.Gender,
                            pinyinname = request.PinYinName,
                            pinyinsurname = request.PinYinSurName,
                            emergencycontactmobile = request.EmergencyContactMobile,
                            code = detail.code,
                            memberId = detail.id
                        };
                        _dbContext.Insert(member_referee);
                        _dbContext.Insert(new t_member_points
                        {
                            eventPoints = 0,
                            memberId = detail.id,
                            points = 0,
                            servicePoints = 0,
                        });
                        _dbContext.CommitChanges();
                        flag = true;

                        userContext.IsExtendPlayer = detail.isExtendReferee;
                        SaveCurrentUser(userContext);
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Rollback();
                        flag = false;
                        msg = "服务异常";
                        LogUtils.LogError("MemberService.ExtendRefereeTran", ex);
                    }
                }
                else
                {
                    msg = "会员信息有误";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.ExtendReferee", ex);
            }
            return flag;
        }
        #endregion

        #region 账号详情
        //选手会员详情
        public MemberPlayerResponse MemberPlayerDetail(int id)
        {
            MemberPlayerResponse detail = null;
            try
            {
                var response = _dbContext.Get<t_member_player>(id);
                if (response != null)
                {
                    detail = new MemberPlayerResponse
                    {
                        MemberId = response.memberId,
                        Card = response.card,
                        CardType = response.cardType,
                        Code = response.code,
                        CompleteName = response.completename,
                        PinYinName = response.pinyinname,
                        CompletePinYin = response.completepinyin,
                        ContactAddress = response.contactaddress,
                        ContactMobile = response.contactmobile,
                        EmergencyContact = response.emergencycontact,
                        EmergencyContactMobile = response.emergencycontactmobile,
                        Gender = response.gender,
                        Grade = response.grade,
                        Name = response.name,
                        PinYinSurName = response.pinyinsurname,
                        SurName = response.surname
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.MemberPlayerDetail", ex);
            }
            return detail;
        }
        //教练会员详情
        public MemberCoachResponse MemberCoachDetail(int id)
        {
            MemberCoachResponse detail = null;
            try
            {
                var response = _dbContext.Get<t_member_coach>(id);
                if (response != null)
                {
                    detail = new MemberCoachResponse
                    {
                        Code = response.code,
                        CompletePinYin = response.completepinyin,
                        ContactAddress = response.contactaddress,
                        ContactMobile = response.contactmobile,
                        EmergencyContact = response.emergencycontact,
                        EmergencyContactMobile = response.emergencycontactmobile,
                        MemberId = response.memberId,
                        PinYinName = response.pinyinname,
                        PinYinSurName = response.pinyinsurname
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.MemberCoachDetail", ex);
            }
            return detail;
        }
        //裁判会员详情
        public MemberRefereeResponse MemberRefereeDetail(int id)
        {
            MemberRefereeResponse detail = null;
            try
            {
                var response = _dbContext.Get<t_member_referee>(id);
                if (response != null)
                {
                    detail = new MemberRefereeResponse
                    {
                        CompleteName = response.completename,
                        PinYinSurName = response.pinyinsurname,
                        PinYinName = response.pinyinname,
                        MemberId = response.memberId,
                        Code = response.code,
                        CompletePinYin = response.completepinyin,
                        ContactAddress = response.contactaddress,
                        ContactMobile = response.contactmobile,
                        EmergencyContact = response.emergencycontact,
                        EmergencyContactMobile = response.emergencycontactmobile
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.MemberRefereeDetail", ex);
            }
            return detail;
        }
        //赛事管理员会员详情
        public MemberEventResponse MemberEventDetail(int id)
        {
            MemberEventResponse detail = null;
            try
            {
                var response = _dbContext.Get<t_member_event>(id);
                if (response != null)
                {
                    detail = new MemberEventResponse
                    {
                        MemberId = response.memberId,
                        Card = response.card,
                        CardType = response.cardType,
                        Code = response.code,
                        CompleteName = response.completename,
                        PinYinName = response.pinyinname,
                        CompletePinYin = response.completepinyin,
                        ContactAddress = response.contactaddress,
                        ContactMobile = response.contactmobile,
                        EmergencyContact = response.emergencycontact,
                        EmergencyContactMobile = response.emergencycontactmobile,
                        Gender = response.gender,
                        Name = response.name,
                        PinYinSurName = response.pinyinsurname,
                        SurName = response.surname
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.MemberEventDetail", ex);
            }
            return detail;
        }
        #endregion

        // 保存用户缓存
        private void SaveCurrentUser(WebUserContext context)
        {
            try
            {
                DateTime expireTime = DateTime.Now.AddHours(24);
                SessionCookieUtility.RemoveCookie(Constant.WebCookieKey);
                SessionCookieUtility.WriteCookie(Constant.WebCookieKey, DesEncoderAndDecoder.Encrypt(context.Serialize()), expireTime);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SaveCurrentUser", ex);
            }
        }
    }
}
