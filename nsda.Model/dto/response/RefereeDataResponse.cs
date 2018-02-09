using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class RefereeDataResponse
    {
        /// <summary>
        /// 总人数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 待审核人数
        /// </summary>
        public int Pending { get; set; }
        /// <summary>
        /// 已录取人数
        /// </summary>
        public int Passed { get; set; }
        /// <summary>
        /// 未录取人数
        /// </summary>
        public int NoPassed { get; set; }
        /// <summary>
        /// 候选名单
        /// </summary>
        public int Candidate { get; set; }
        /// <summary>
        /// 临时裁判
        /// </summary>
        public int TempReferee { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public int Flag { get; set; }
    }
}
