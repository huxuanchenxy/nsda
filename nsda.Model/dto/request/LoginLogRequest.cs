using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class LoginLogRequest
    {
        public string Account { get; set; }
        public DataTypeEm DataType { get; set; }
        public string LoginResult { get; set; }
    }

    public class LoginLogQueryRequest:PageQuery
    {
        public string Account { get; set; }
        public DataTypeEm DataType { get; set; }
        public DateTime? CreateStart { get; set; }
        public DateTime? CreateEnd { get; set; }
    }
}
