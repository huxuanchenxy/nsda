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
        确认组队=3,
        已付款=4,
        已退款=5,
        组队成功=6,
        组队失败 = 7,
        正在比赛中=8,
        比赛完成=9
    }
}
