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
        /// 积分
        /// </summary>
        public decimal Points { get; set; }
        /// <summary>
        /// 赛事积分
        /// </summary>
        public decimal EventPoints { get; set; }
        /// <summary>
        /// 服务积分
        /// </summary>
        public decimal ServicePoints { get; set; }
    }
}
