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
        //注册选手
        bool RegisterMemberPlayer(RegisterPlayerRequest request,out string msg);

        //注册教练
        bool RegisterMemberCoach(RegisterCoachRequest request, out string msg);

        //注册裁判
        bool RegisterMemberReferee(RegisterRefereeRequest request, out string msg);

        //注册赛事管理员
        bool RegisterMemberEvent(RegisterEventRequest request, out string msg);

        //编辑选手
        bool EditMemberPlayer(RegisterPlayerRequest request,WebUserContext userContext, out string msg);

        //编辑教练
        bool EditMemberCoach(RegisterCoachRequest request, WebUserContext userContext, out string msg);

        //编辑裁判
        bool EditMemberReferee(RegisterRefereeRequest request, WebUserContext userContext, out string msg);

        //编辑赛事管理员
        bool EditMemberEvent(RegisterEventRequest request, WebUserContext userContext, out string msg);

        //选手会员详情
        MemberPlayerResponse MemberPlayerDetail(int id);
        //教练会员详情
        MemberCoachResponse MemberCoachDetail(int id);
        //裁判会员详情
        MemberRefereeResponse MemberRefereeDetail(int id);
        //赛事管理员会员详情
        MemberEventResponse MemberEventDetail(int id);
        //登录
        WebUserContext Login(LoginRequest request, out string msg);
       //修改密码
        bool EditPwd(int memberId,string pwd, out string msg);
        //实名认证回调 修改用户状态
        void CallBack(int id);
        //会员列表 
        List<MemberResponse> List(MemberQueryRequest request);
        //删除会员信息
        bool Delete(int id, int sysUserId, out string msg);
        //重置密码
        bool Reset(int id, int sysUserId, out string msg);
        // 验证邮箱是否有效 并返回用户id
        int SendEmail(string email, out string msg);
        // 强制认证选手信息
        bool Force(int id, int sysUserId, out string msg);
        // 模糊查找选手
        List<MemberSelectResponse> SelectPlayer(string key, string value, int memberId);
        // 模糊查找教练
        List<MemberSelectResponse> SelectCoach(string key, string value, int memberId);
        //去认证
        int GoAuth(int memberId, out string msg);
        //账号是否存在
        bool IsExist(string account);
        //更换头像
        bool ReplaceHead(string headUrl, int memberId);
        //账号轮询 看账号是否已认证
        bool MemberPlayerPolling(WebUserContext userContext);
        bool ExtendPlayer(RegisterPlayerRequest request, WebUserContext userContext, out string msg);
        bool ExtendCoach(RegisterCoachRequest request, WebUserContext userContext, out string msg);
        bool ExtendReferee(RegisterRefereeRequest request, WebUserContext userContext, out string msg);
    }
}
