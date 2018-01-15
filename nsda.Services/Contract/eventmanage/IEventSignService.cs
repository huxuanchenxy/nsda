using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
        bool BatchSign(List<int> id,int eventId,bool isNormal,out string msg);
        //选手签到列表
        List<MemberSignResponse> PlayerSignList(MemberSignQueryRequest request);
        //裁判签到列表
        List<MemberSignResponse> RefereeSignList(MemberSignQueryRequest request);
        //选手 裁判获取签到信息
        SignResponse GetSign(int eventId,int memberId);
    }
}
