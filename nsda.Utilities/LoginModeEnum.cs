using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public enum LoginModeEnum
    {
        [Description("执行")]
        Enforce = 1,

        [Description("忽略")]
        Ignore = 2
    }
}
