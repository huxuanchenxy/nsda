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
        bool IsAcceptTeam(int id, int memberId, bool isAgree, out string msg);
        //替换队友
        bool ReplaceTeammate(int id, int memberId, int newMemberId,out string msg);
        // 去支付
        bool GoPay(int id,int memberId,out string msg);
        // 申请退赛
        bool Cancel(int id, int memberId, out string msg);
        // 确认退赛
        bool ConfirmCancel(int id, int memberId, out string msg);
        // 比赛列表
        PagedList<PlayerSignUpResponse> List(PlayerSignUpQueryRequest request);
        // 支付成功回调
        void Callback(int id);
    }
}
