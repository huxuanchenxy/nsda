using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventRefereeSignUpQueryRequest:PageQuery
    {
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public string KeyValue { get; set; }
    }

    public class RefereeSignUpQueryRequest : PageQuery
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
