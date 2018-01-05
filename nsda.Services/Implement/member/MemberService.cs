using nsda.Model.dto.request;
using nsda.Model.enums;
using nsda.Models;
using nsda.Repository;
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
    public class MemberService: IMemberService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MemberService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 注册
        public bool Register(RegisterRequest request,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var userContext = new WebUserContext { };
                SaveCurrentUser(userContext); 
            }
            catch (Exception ex)
            {
                msg = "服务异常";
                LogUtils.LogError("MemberService.Register", ex);
            }
            return flag;
        }
        //1.1 登录
        public WebUserContext Login(string account,string pwd,out string msg)
        {
            WebUserContext userContext = null;
            msg = string.Empty;
            try
            {
                if (account.IsEmpty() || pwd.IsEmpty())
                {
                    msg = "账号或密码不能为空";
                    return userContext;
                }

                var detail = _dbContext.QueryFirstOrDefault<t_member>(@"select * from t_member where account=@account and pwd=@pwd ",
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
                    //记录缓存
                    userContext = new WebUserContext
                    {
                        Id = detail.id,
                        Name = detail.name,
                        Account = detail.account,
                        Role="1",
                        MemberType=(int)detail.memberType
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
        //1.2 修改
        public  bool Edit(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.Edit", ex);
            }
            return flag;
        }
        //1.3 修改密码
        public bool UpdatePwd(int memberId, string oldPwd, string newPwd, out string msg)
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
        //1.4 实名认证回调 修改用户状态
        public void CallBack()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.CallBack", ex);
            }
        }
        //1.5 会员列表 
        public void List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.List", ex);
            }
        }
        //1.6 手动添加临时会员信息
        public  bool AddTempMember(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MemberService.AddTempMember", ex);
            }
            return flag;
        }
        //1.7 删除会员信息
        public  bool Delete(int id, out string msg)
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
        //1.8 重置密码
        public bool Reset(int id,out string msg)
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
                else {
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
        //1.9 启用禁用账号
        public bool IsEnable(int id, bool isEnable, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var member = _dbContext.Get<t_member>(id);
                if (member != null)
                {
                    member.memberStatus = isEnable ? MemberStatusEm.启用 : MemberStatusEm.禁用;
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
                LogUtils.LogError("MemberService.IsEnable", ex);
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
                SessionCookieUtility.WriteCookie(Constant.WebCookieKey, MemberEncoderAndDecoder.encrypt(context.Serialize()), expireTime);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.SaveCurrentUser", ex);
            }
        }
    }
}
