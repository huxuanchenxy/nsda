using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventSignRequest
    {
        public int EventId { get; set; }
        public int EventSignStatus{ get; set; }
        public int EventSignType { get; set; }
        public string EventManMemberId { get; set; }
        public List<string> Signdates { get; set; }

    }

}
