using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 选手报名状态
    /// </summary>
    public enum SignUpStatusEm
    {
        等待队员确认邀请= 1,
        报名邀请中 = 2,
        组队成功=3,
        组队失败 = 4,
        报名成功 = 5,
        等待队友确认退赛 = 6,
        退费申请中=7,
        拒绝退赛=8,
        退费中=9,
        已退费 = 10,
        已退赛=11,
        重新组队=12,
        比赛完成
    }
}
