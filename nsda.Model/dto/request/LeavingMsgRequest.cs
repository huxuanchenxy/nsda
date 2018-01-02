using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class LeavingMsgRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
        public string Mobile { get; set; }
    }

    public class LeavingMsgQueryRequest: PagedQuery
    {
        public LeavingStatusEm? LeavingStatus { get; set; }
        public DateTime? CreateStart { get; set; }
        public DateTime? CreateEnd { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
