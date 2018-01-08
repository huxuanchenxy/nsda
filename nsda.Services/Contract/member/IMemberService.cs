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
        //1.1 登录
        WebUserContext Login(string account, string pwd,out string msg);
        //1.2 修改
        bool Edit(MemberRequest request, out string msg);
        //1.3 修改密码
        bool EditPwd(int memberId, string oldPwd, string newPwd, out string msg);
        //1.4 实名认证回调 修改用户状态
        void CallBack(int id);
        //1.5 会员列表 
        PagedList<MemberResponse> List(MemberQueryRequest request);
        //1.6 手动添加临时会员信息
        bool AddTempMember(out string msg);
        //1.7 删除会员信息
        bool Delete(int id, out string msg);
        //1.8 重置密码
        bool Reset(int id, out string msg);
        //1.9 启用、禁用账号
        bool IsEnable(int id,bool isEnable, out string msg);
        //1.10 找回密码
        bool FindPwd(int memberId,string pwd, out string msg);
        // 验证邮箱是否有效 并返回用户id
        int SendEmail(string email, out string msg);
        // 会员详情
        MemberResponse Detail(int id);
    }
}
