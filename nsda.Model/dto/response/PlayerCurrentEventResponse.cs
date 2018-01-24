using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerCurrentEventResponse
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public EventTypeEm EventType { get; set; }
    }

    public class RefereeCurrentEventResponse
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public EventTypeEm EventType { get; set; }
    }
}
