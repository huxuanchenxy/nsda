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
        被替换=0,
        等待队员确认邀请= 1,
        报名邀请中 = 2,
        组队成功=3,
        组队失败 = 4,
        拒绝组队 = 5,
        报名成功 = 6,
        等待队友确认退赛 = 7,
        退费申请中=8,
        拒绝退赛=9,
        退费中=10,
        已退费 = 11,
        已退赛=12,
        重新组队=13,
        比赛完成
    }
}
