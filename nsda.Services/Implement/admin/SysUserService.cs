using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Models;
using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Linq;
using System.Text;

namespace nsda.Services.admin
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public class SysUserService : ISysUserService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public SysUserService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        //1.0 添加系统用户
        public bool Insert(SysUserRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Account.IsEmpty()|| request.Pwd.IsEmpty())
                {
                    msg = "账号或密码为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }

                var isexist = _dbContext.Select<t_sysuser>(c => c.name == request.Name).ToList();
                if (isexist != null && isexist.Count > 0)
                {
                    msg = "已存在此账号";
                    return flag;
                }

                _dbContext.Insert(new t_sysuser
                {
                    account = request.Account,
                    name = request.Name,
                    pwd = request.Pwd,
                    mobile = request.Mobile,
                    sysUserStatus = SysUserStatusEm.正常
                });
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Insert", ex);
            }
            return flag;
        }
        //1.1 登录
        public bool Login(string account, string pwd, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.QueryFirstOrDefault<t_sysuser>(@"select * from t_sysuser where account=@account and pwd=@pwd ",
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
                    if (detail.sysUserStatus == SysUserStatusEm.禁用)
                    {
                        msg = "账号已禁用请联系管理员";
                    }
                    else
                    {
                        //记录缓存
                        var users = new SysUserContext
                        {
                            Id = detail.id,
                            Name = detail.name,
                            Account = detail.account
                        };
                        SaveCurrentUser(users);
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Login", ex);
            }
            return flag;
        }
        //1.2 修改
        public bool Edit(SysUserRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "账号不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }

                var sysuser = _dbContext.Get<t_sysuser>(request.Id);
                if (sysuser != null)
                {
                    sysuser.name = request.Name;
                    sysuser.mobile = request.Mobile;
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                    flag = true;
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Edit", ex);
            }
            return flag;
        }
        //1.3 修改密码
        public bool UpdatePwd(int id, string oldPwd, string newPwd, out string msg)
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

                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    if (!string.Equals(oldPwd, sysuser.pwd, StringComparison.OrdinalIgnoreCase))
                    {
                        msg = "原密码有误";
                    }
                    else
                    {
                        sysuser.pwd = newPwd;
                        sysuser.updatetime = DateTime.Now;
                        _dbContext.Update(sysuser);
                        flag = true;
                    }
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.UpdatePwd", ex);
            }
            return flag;
        }
        //1.4 系统用户列表 
        public PagedList<SysUserResponse> List(SysUserQueryRequest request)
        {
            PagedList<SysUserResponse> list = new PagedList<SysUserResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from t_sysuser where isdelete=0");
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    sb.Append(" and name like @Name");
                }
                if (request.Mobile.IsNotEmpty())
                {
                    request.Mobile = "%" + request.Mobile + "%";
                    sb.Append(" and mobile like @Mobile");
                }
                if (request.LastLoginTime1.HasValue)
                {
                    sb.Append(" and lastlogintime >= @LastLoginTime1");
                }
                if (request.LastLoginTime2.HasValue)
                {
                    request.LastLoginTime2 = request.LastLoginTime2.Value.AddDays(1).AddSeconds(-1);
                    sb.Append("  and lastlogintime<=@LastLoginTime2");
                }
                list = _dbContext.Page<SysUserResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysUserService.List", ex);
            }
            return list;
        }
        //1.5 删除系统用户信息
        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    sysuser.isdelete = true;
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                    flag = true;
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Delete", ex);
            }
            return flag;
        }
        //1.6 重置密码
        public bool Reset(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    sysuser.pwd = "159357";
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Reset", ex);
            }
            return flag;
        }
        //1.7 系统用户详情
        public SysUserResponse Detail(int id)
        {
            SysUserResponse detail = null;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    detail = new SysUserResponse
                    {
                        Account = sysuser.account,
                        Id = id,
                        CreateTime = sysuser.createtime,
                        UpdateTime = sysuser.updatetime,
                        LastLoginTime = sysuser.lastlogintime,
                        Mobile = sysuser.mobile,
                        Name = sysuser.name,
                        SysUserStatus = sysuser.sysUserStatus
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysUserService.Detail", ex);
            }
            return detail;
        }
        //1.8 启/禁用账号
        public bool IsEnable(int id, int sysUserId, bool isEnable, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    sysuser.sysUserStatus = isEnable ? SysUserStatusEm.正常 : SysUserStatusEm.禁用;
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Reset", ex);
            }
            return flag;
        }

        /// <summary>
        /// 保存用户缓存
        /// </summary>
        private  void SaveCurrentUser(SysUserContext context)
        {
            try
            {
                DateTime expireTime = DateTime.Now.AddHours(12);
                SessionCookieUtility.WriteCookie(Constant.SysCookieKey, MemberEncoderAndDecoder.encrypt(context.Serialize()), expireTime);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysUserService.SaveCurrentUser", ex);
            }
        }

    }
}
