using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    public enum TeamScoringRulesEm
    {
        队伍获胜的场数 = 1,
        队伍中两位辩手的个人总分 = 2,
        所遇到的对手获胜的场数总和 = 3,
        对手的个人总分 = 4,
        队伍中两位辩手的个人排名总和 = 5,
        电脑随机分配数值 = 6
    }

    public enum ScoringRulesEm
    {
        选手个人总分 = 1,
        个人排名总和 = 2,
        队伍获胜场数总和 = 3,
        所遇到的对手获胜的场数总和 = 4,
        对手的个人总分 = 5,
        电脑随机分配数值 = 6
    }
}
