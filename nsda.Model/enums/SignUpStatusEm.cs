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
        已付款=4,
        组队成功=5,
        组队失败 = 6,
        等待队友确认退赛=7,
        队友确认对赛=8,
        退赛成功
    }
}
