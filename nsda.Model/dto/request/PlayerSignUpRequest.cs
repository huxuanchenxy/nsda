using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerSignUpRequest
    {
        /// <summary>
        /// 赛事类型
        /// </summary>
        public EventTypeEm EventType{ get; set; }
        /// <summary>
        /// 邀请者
        /// </summary>
        public int FromMemberId { get; set; }
        /// <summary>
        /// 被邀请者
        /// </summary>
        public int ToMemberId { get; set; }

        /// <summary>
        /// 赛事id
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 赛事组别
        /// </summary>
        public int EventGroupId { get; set; }

        public decimal SignFee { get; set; }

        public string GroupNum { get; set; }
    }

    public class EventPlayerSignUpQueryRequest : PageQuery
    {
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public string KeyValue { get; set; }
    }

    public class PlayerSignUpQueryRequest : PageQuery
    {
        public int MemberId { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        /// <summary>
        /// 比赛时间区间
        /// </summary>
        public DateTime? StartDate { get; set; }
    }
}
