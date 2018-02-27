using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventRegularAwardsResponse
    {
        public int EventId { get; set; }
        public int EventGroupId { get; set; }
        //优秀辩手数量
        public int Personal { get; set; }
        // 团队奖项
        public int Group { get; set; }
    }
}
