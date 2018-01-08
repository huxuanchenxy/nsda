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
        public MemberService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService,ISysOperLogService sysOperLogService)
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
                bool validateflag = Validate(request, out msg);
                if (!validateflag)
                {
                    return flag;
                }
                _dbContext.BeginTransaction();
                int id = _dbContext.Insert(new t_member
                {
                    account = request.Account,
                    card = request.Card,
                    cardType = request.CardType,
                    code = _dataRepository.MemberRepo.RenderCode(),
                    completename = request.Name,
                    completepinyin = request.CompletePinYin,
                    contactaddress = request.ContactAddress,
                    contactmobile = request.ContactMobile,
                    emergencycontact = request.EmergencyContact,
                    gender = request.Gender,
                    grade = request.Grade,
                    lastlogintime = DateTime.Now,
                    memberStatus = request.MemberType != MemberTypeEm.选手 ? MemberStatusEm.已认证 : MemberStatusEm.待认证,
                    name = request.Name,
                    memberType = request.MemberType,
                    pinyinname = request.PinYinName,
                    pinyinsurname = request.PinYinSurName,
                    pwd = request.Pwd,
                    surname = request.SurName
                }).ToObjInt();

                _dbContext.Insert(new t_memberpoints
                {
                    eventPoints = 0,
                    memberId = id,
                    points = 0,
                    servicePoints = 0,
                });

                _dbContext.CommitChanges();
                var userContext = new WebUserContext
                {
                    Name = request.Name,
                    Account = request.Account,
                    Role = ((int)request.MemberType).ToString(),
                    MemberType = (int)request.MemberType,
                    Id=id,
                    Status=(request.MemberType==MemberTypeEm.赛事管理员||request.MemberType==MemberTypeEm.选手)?(int)MemberStatusEm.待认证:(int)MemberStatusEm.已认证
                };
                SaveCurrentUser(userContext);
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
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
                    //读取已经认证的
                    string role = ((int)detail.memberType).ToString();
                    var memberextend = _dbContext.Select<t_memberextend>(c => c.memberId == detail.id && c.memberExtendStatus == MemberExtendStatusEm.通过).ToList();
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
                        Status=(int)detail.memberStatus
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

                bool validateflag = Validate(request, out msg);
                if (!validateflag)
                {
                    return flag;
                }

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
                _dbContext.Update(member);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Edit", ex);
            }
            return flag;
        }

        private bool Validate(MemberRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            switch (request.MemberType)
            {
                case MemberTypeEm.选手:
                    //默认 待认证
                    break;
                case MemberTypeEm.教练:
                    //正常
                    break;
                case MemberTypeEm.裁判:
                    //正常
                    break;
                case MemberTypeEm.赛事管理员:
                    //待认证
                    break;
            }
            return flag;
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

                if (!string.Equals(oldPwd, newPwd))
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
                        var memberextend = _dbContext.Select<t_memberextend>(c => c.memberId == member.id && c.memberExtendStatus == MemberExtendStatusEm.通过).ToList();
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
        public PagedList<MemberResponse> List(MemberQueryRequest request)
        {
            PagedList<MemberResponse> list = new PagedList<MemberResponse>();
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.List", ex);
            }
            return list;
        }
        //删除会员信息
        public bool Delete(int id,int sysUserId,out string msg)
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
                var member = _dbContext.Select<t_member>(c => c.account == email && c.memberStatus != MemberStatusEm.认证失败 && c.memberType != MemberTypeEm.临时裁判 && c.memberType != MemberTypeEm.临时选手).FirstOrDefault();
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
            MemberResponse repsonse = null;
            try
            {
                var detail = _dbContext.Get<t_member>(id);
                if (detail != null)
                {
                    repsonse = new MemberResponse
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
            return repsonse;
        }

        //审核赛事管理员账号
        public bool Check(int id, int sysUserId, string remark, bool isAppro, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_member>(id);
                if (detail != null&&detail.memberType==MemberTypeEm.赛事管理员)
                {
                    detail.updatetime = DateTime.Now;
                    detail.memberStatus =isAppro?MemberStatusEm.已认证:MemberStatusEm.认证失败;
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
                if (detail != null && detail.memberType == MemberTypeEm.选手)
                {
                    detail.updatetime = DateTime.Now;
                    detail.memberStatus = MemberStatusEm.已认证;
                    _dbContext.Update(detail);
                    DeleteCurrentUser(id);//清除用户缓存 使其重新登录
                    flag = true;
                }
                else {
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
    }
}
