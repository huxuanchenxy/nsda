using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 签到状态
    /// </summary>
    public enum EventSignStatusEm
    {
        待签到=1,
        已签到=2
    }

    /// <summary>
    /// 签到状态
    /// </summary>
    public enum EventSignTypeEm
    {
        选手 = 1,
        裁判 = 2
    }

    /// <summary>
    /// 裁判当日使用状态
    /// </summary>
    public enum RefereeStatusEm
    {
        闲置 = 1,
        使用中 = 2
    }
}
