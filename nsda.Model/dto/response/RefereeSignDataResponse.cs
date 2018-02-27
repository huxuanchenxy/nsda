using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class RefereeSignDataResponse
    {
        public RefereeSignDataResponse()
        {
            RefereeSignGroup = new List<RefereeSignGroupResponse>();
        }
        /// <summary>
        /// 已签到的裁判数量
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 建议最少裁判数
        /// </summary>
        public int LeastCount { get; set; }
        public List<RefereeSignGroupResponse> RefereeSignGroup { get; set; }
        /// <summary>
        /// 使用中数量
        /// </summary>
        public int UsedCount { get; set; }
        /// <summary>
        /// 停用数量
        /// </summary>
        public int StopCount { get; set; }
        /// <summary>
        /// 闲置数量
        /// </summary>
        public int UnusedCount { get; set; }
    }

    public class RefereeSignGroupResponse
    {
        /// <summary>
        /// 赛事组别Id
        /// </summary>
        public int EventGroupId { get; set; }
        /// <summary>
        /// 已签到的裁判数量
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 建议最少裁判数
        /// </summary>
        public int LeastCount { get; set; }
    }
}
