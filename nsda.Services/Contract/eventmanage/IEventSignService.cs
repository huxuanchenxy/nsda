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
        bool Sign(out string msg);
        //赛事管理员批量签到
        bool BatchSign(out string msg);
        //选手签到列表
        void PlayerSignList();
        //裁判签到列表
        void RefereeSignList();
    }
}
