﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Models;
using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsda.Services.admin
{
    /// <summary>
    /// 系统用户管理
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

                var isexist = _dbContext.Select<t_sys_user>(c => c.name == request.Name).ToList();
                if (isexist != null && isexist.Count > 0)
                {
                    msg = "已存在此账号";
                    return flag;
                }

                _dbContext.Insert(new t_sys_user
                {
                    account = request.Account,
                    name = request.Name,
                    pwd = request.Pwd,
                    mobile = request.Mobile,
                    sysUserStatus = SysUserStatusEm.启用
                });
                flag = true;
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
                var detail = _dbContext.QueryFirstOrDefault<t_sys_user>(@"select * from t_sys_user where account=@account and pwd=@pwd ",
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
                        detail.lastlogintime = DateTime.Now;
                        _dbContext.Update(detail);
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
                if (request.Account.IsEmpty())
                {
                    msg = "账号不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }

                var sysuser = _dbContext.Get<t_sys_user>(request.Id);
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
        public bool UpdatePwd(int id, string pwd, out string msg)
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

                if (pwd.Length<6)
                {
                    msg = "密码长度不能低于6";
                    return flag;
                }

                var sysuser = _dbContext.Get<t_sys_user>(id);
                if (sysuser != null)
                {
                    sysuser.pwd = pwd;
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
                LogUtils.LogError("SysUserService.UpdatePwd", ex);
            }
            return flag;
        }
        //1.4 系统用户列表 
        public List<SysUserResponse> List(SysUserQueryRequest request)
        {
            List<SysUserResponse> list = new List<SysUserResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Name.IsNotEmpty())
                {
                    request.Name = $"%{request.Name}%";
                    join.Append(" and name like @Name");
                }
                if (request.Mobile.IsNotEmpty())
                {
                    request.Mobile = $"%{request.Mobile}%";
                    join.Append(" and mobile like @Mobile");
                }
                if (request.LastLoginTime1.HasValue)
                {
                    join.Append(" and lastlogintime >= @LastLoginTime1");
                }
                if (request.LastLoginTime2.HasValue)
                {
                    request.LastLoginTime2 = request.LastLoginTime2.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and lastlogintime<=@LastLoginTime2");
                }
                var sql=$@"select * from t_sys_user where isdelete=0 {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<SysUserResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
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
                var sysuser = _dbContext.Get<t_sys_user>(id);
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
                var sysuser = _dbContext.Get<t_sys_user>(id);
                if (sysuser != null)
                {
                    sysuser.pwd = "159357";
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
                var sysuser = _dbContext.Get<t_sys_user>(id);
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
        public bool IsEnable(int id, bool isEnable, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sys_user>(id);
                if (sysuser != null)
                {
                    sysuser.sysUserStatus = isEnable ? SysUserStatusEm.启用 : SysUserStatusEm.禁用;
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
                LogUtils.LogError("SysUserService.IsEnable", ex);
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
                SessionCookieUtility.WriteCookie(Constant.SysCookieKey, DesEncoderAndDecoder.Encrypt(context.Serialize()), expireTime);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysUserService.SaveCurrentUser", ex);
            }
        }
    }
}
