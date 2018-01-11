using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model
{
    public class ParamsConfig
    {
        /// <summary>
        /// 参与比赛了的选手
        /// </summary>
        public static string _signup_in = new int[] { (int)SignUpStatusEm.组队成功, (int)SignUpStatusEm.报名成功 }.Splice();
    }
}
