using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerSignQueryRequest:PageQuery
    {
        public int EventGroupId { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public string KeyValue { get; set; }
    }

    public class RefereeSignQueryRequest : PageQuery
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public string KeyValue { get; set; }
    }
}
