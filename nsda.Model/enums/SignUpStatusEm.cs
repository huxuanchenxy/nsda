using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    public enum SignUpStatusEm
    {
        等待队员确认邀请= 1,
        报名邀请中 = 2,
        确认组队= 3,
        报名成功=4,
        组队成功=5,
        组队失败 = 6,
        等待队友确认退赛=7,
        队友确认退赛 = 8,
        退费处理中=9,
        已退费
    }
}
