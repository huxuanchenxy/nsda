using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MemberPointsModelResponse
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 裁判积分
        /// </summary>
        public int RefereePoints { get; set; }
        /// <summary>
        /// 选手积分
        /// </summary>
        public int PlayerPoints { get; set; }
        /// <summary>
        /// 教练积分
        /// </summary>
        public int CoachPoints { get; set; }
    }
}
