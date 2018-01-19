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
         报名邀请中=1,
         待付款=2,
         已付款=3,
         报名成功=4,
         退赛申请中=5,
         已退赛=6,
         组队失败=7,
         已完成=8
    }
}
