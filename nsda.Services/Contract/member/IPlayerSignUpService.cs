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
        // 去支付
        int GoPay(int id,int memberId,out string msg);
        // 申请退赛
        bool ApplyRetire(int id, int memberId, out string msg);
        // 确认退赛
        bool IsConfirmRetire(int id, bool isConfirm, int memberId, out string msg);
        // 比赛列表
        // 支付成功回调
        void Callback(int memberId,int sourceId);
        //选手获取当天比赛信息
        List<PlayerCurrentEventResponse> CurrentPlayerEvent(int memberId);
        //赛事管理员 选手报名列表
        List<EventPlayerSignUpListResponse> EventPlayerList(EventPlayerSignUpQueryRequest request);
        //邀请队友下拉框
        List<MemberSelectResponse> Invitation(string keyvalue, int eventId,int groupId,int memberId);
        //跟据赛事id 以及会员 找出适合他的赛事组别
        List<EventGroupResponse> EventGroup(int eventId, int memberId);
        //选手报名列表
        List<PlayerSignUpListResponse> PlayerSignUpList(PlayerSignUpQueryRequest request);
        //生成签到表
        bool RenderSign(int eventId, out string msg);
        //选手退赛列表
        List<PlayerRefundListResponse> PlayerRefundList(PlayerSignUpQueryRequest request);
        //未报名成功的队伍 申请退费
        bool ApplyRefund(int eventId, int operUserId, out string msg);
        // 教室设定特殊学员
        List<MemberSelectResponse> SelectPlayer(int eventId, string keyvalue);
    }
}
