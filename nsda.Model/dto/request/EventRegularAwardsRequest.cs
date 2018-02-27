using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventRegularAwardsRequest
    {
        public int EventId { get; set; }
        public int EventGroupId { get; set; }
        public int Personal { get; set; }
        public int PrizeGroup { get; set; }
    }
}
