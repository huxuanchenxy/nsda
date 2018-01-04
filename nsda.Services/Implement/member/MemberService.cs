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
        public  bool Register(out string msg)
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
                LogUtils.LogError("MemberService.Register", ex);
            }
            return flag;
        }
        //1.1 登录
        public  bool Login(string account,string pwd,out string msg)
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
                LogUtils.LogError("MemberService.Login", ex);
            }
            return flag;
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
        public  bool UpdatePwd(out string msg)
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
