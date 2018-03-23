using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 淘汰赛类型
    /// </summary>
    public  enum KnockoutTypeEm
    {
        //部分赛
        partial=1,
        //1/32
        tripleoctafinals=2,
        //1/16
        doubleoctafinals =3,
        //1/8
        octafinals =4,
        //1/4
        quarterfinals =5,
        //半决赛
        semifinals =6,
        //决赛
        finals =7
    }


}
