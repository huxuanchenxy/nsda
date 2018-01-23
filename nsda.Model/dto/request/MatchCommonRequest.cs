using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MatchCommonRequest
    {
        public int EventId { get; set; }
        public int EventGroupId { get; set; }
        public ObjEventTypeEm ObjEventType { get; set; }
        public int ObjId { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// 评分列表时所用到
        /// </summary>
        public bool IsEnter { get; set; }
    }
}
