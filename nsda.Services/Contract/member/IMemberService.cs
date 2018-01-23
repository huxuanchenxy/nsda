using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
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
    public interface IMemberService : IDependency
    {
        //1.0 注册
        bool Register(MemberRequest request,out string msg);
        //登录
        WebUserContext Login(LoginRequest request, out string msg);
        //修改
        bool Edit(MemberRequest request, out string msg);
        //修改密码
        bool EditPwd(int memberId, string oldPwd, string newPwd, out string msg);
        //实名认证回调 修改用户状态
        void CallBack(int id);
        //会员列表 
        List<MemberResponse> List(MemberQueryRequest request);
        //删除会员信息
        bool Delete(int id, int sysUserId, out string msg);
        //重置密码
        bool Reset(int id, int sysUserId, out string msg);
        //找回密码
        bool FindPwd(int memberId,string pwd, out string msg);
        // 验证邮箱是否有效 并返回用户id
        int SendEmail(string email, out string msg);
        // 审核赛事管理员账号
        bool Check(int id, string remark, bool isAgree, int sysUserId, out string msg);
        // 强制认证选手信息
        bool Force(int id, int sysUserId, out string msg);
        // 会员详情
        MemberResponse Detail(int id);
        // 模糊查找选手
        List<MemberSelectResponse> SelectPlayer(string key, string value, int memberId);
        // 模糊查找教练
        List<MemberSelectResponse> SelectTrainer(string key, string value, int memberId);
        //去认证
        int GoAuth(int memberId, out string msg);
        bool IsExist(string account);
    }
}
