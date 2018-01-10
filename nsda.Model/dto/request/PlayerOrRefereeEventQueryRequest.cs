using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerOrRefereeEventQueryRequest:PagedQuery
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 是否是裁判
        /// </summary>
        public bool IsReferee { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        /// <summary>
        /// 赛事级别
        /// </summary>
        public EventLevelEm? EventLevel { get; set; }
        /// <summary>
        /// 比赛时间区间
        /// </summary>
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 比赛编码或名称
        /// </summary>
        public string KeyValue { get; set; }
    }
}
