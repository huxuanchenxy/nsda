using nsda.Model.enums;
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
        public int GroupId { get; set; }

        public decimal SignFee { get; set; }

        public string GroupNum { get; set; }
    }

    public class PlayerSignUpQueryRequest : PagedQuery
    {
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int MemberId { get; set; }
    }
}
