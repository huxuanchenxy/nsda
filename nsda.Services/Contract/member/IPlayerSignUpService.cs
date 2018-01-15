using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.member
{
    /// <summary>
    /// 选手报名管理
    /// </summary>
    public interface IPlayerSignUpService: IDependency
    {
        //发起组队
        bool Insert(PlayerSignUpRequest request,out string msg);
        //是否接受组队
        bool IsAcceptTeam(int id, bool isAgree, int memberId, out string msg);
        //替换队友
        bool ReplaceTeammate(int id, int newMemberId, int memberId, out string msg);
        // 去支付
        bool GoPay(int id,int memberId,out string msg);
        // 申请退赛
        bool ApplyRetire(int id, int memberId, out string msg);
        // 确认退赛
        bool ConfirmRetire(int id, int memberId, out string msg);
        // 比赛列表
        // 支付成功回调
        void Callback(int memberId,int sourceId);
        // 审核退赛
        bool CheckRetire(int id, bool isAppro,int memberId,out string msg);
        //选手获取当天比赛信息
        List<CurrentEventResponse> CurrentPlayerEvent(int memberId);
        //选手报名列表
        List<PlayerSignUpListResponse> EventPlayerList(PlayerSignUpQueryRequest request);
        //邀请队友下拉框
        List<MemberSelectResponse> Invitation(string keyvalue, int eventId, int memberId);
    }
}
