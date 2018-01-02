using nsda.Utilities;
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
    public class MemberService
    {
        //1.0 注册
        public bool Register(out string msg)
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
        public bool Login(string account,string pwd,out string msg)
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
        //1.2 修改
        public bool Update(out string msg)
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
        //1.3 修改密码
        public bool UpdatePwd(out string msg)
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
        //1.4 实名认证回调 修改用户状态
        public void CallBack()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberService.Register", ex);
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
                LogUtils.LogError("MemberService.Register", ex);
            }
        }
    }
}
