using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerOrRefereeEventQueryRequest:PageQuery
    {
        public int? CountryId { get; set; }
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
    }
}
