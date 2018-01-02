using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EmailLogRequest
    {
        public DataTypeEm DataType { get; set; }
        public string Account { get; set; }
        public string Content { get; set; }
    }

    public class EmailLogQueryRequest : PagedQuery
    {
        public string Account { get; set; }
        public DataTypeEm DataType { get; set; }
        public DateTime? CreateStart { get; set; }
        public DateTime? CreateEnd { get; set; }
    }
}
