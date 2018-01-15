using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EmailLogRequest
    {
        public string Account { get; set; }
        public string Content { get; set; }
    }

    public class EmailLogQueryRequest : PageQuery
    {
        public string Account { get; set; }
        public DateTime? CreateStart { get; set; }
        public DateTime? CreateEnd { get; set; }
    }
}
