using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 赛事签到管理
    /// </summary>
    public interface IEventSignService: IDependency
    {
        //选手 裁判签到
        bool Sign(int id,int memberId,out string msg);
        //赛事管理员批量签到
        bool BatchSign(List<int> memberId, int eventId, EventSignTypeEm eventSignType, out string msg);
        //选手签到列表
        List<PlayerSignResponse> PlayerSignList(PlayerSignQueryRequest request);
        //裁判签到列表
        List<RefereeSignResponse> RefereeSignList(RefereeSignQueryRequest request);
        //选手 裁判获取签到信息
        SignResponse GetSign(int eventId,int memberId,EventSignTypeEm eventSignType);
        //选手批量签到
        bool PlayerBatchSign(List<string> groupNum, int eventId,out string msg);
        //停赛
        bool Stop(string groupNum, int eventId, out string msg);
        //裁判批量签到或设置组别
        bool BatchReferee(List<int> memberId, int eventId, int status, out string msg);
    }
}
