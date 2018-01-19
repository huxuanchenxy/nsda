using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    public enum AvoidRulesEm
    {
        不规避=1,
        尽量规避同校=2,
        尽量规避同教练=3
    }

    public enum RefereeAvoidRulesEm
    {
        不规避 = 1,
        尽量规避自己的学生=2,
        尽量规避自己已经裁判过的学生=3
    }
}
