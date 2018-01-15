using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 会员状态
    /// </summary>
    public enum MemberStatusEm
    {
        待认证 = 1,
        已认证 = 2,
        待审核=3,
        通过=4,
        拒绝=5
    }
}
