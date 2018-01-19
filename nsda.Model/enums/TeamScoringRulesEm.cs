using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    public enum TeamScoringRulesEm
    {
        获胜场数=1,
        对方获胜场数 = 2,
        队伍选手总分=3,
        对方队伍选手总分 = 4,
        队伍选手排名总和=5,
        随机数值=6
    }

    public enum ScoringRulesEm
    {
        获胜场数 = 1,
        对方获胜场数 = 2,
        选手总分 = 3,
        选手排名总和 = 4,
        对方选手排名 = 5,
        随机数值 = 6
    }
}
