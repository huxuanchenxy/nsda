using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class SignResponse
    {
        public int Id { get; set; }
        public string EventCode { get; set; }
        public string EventName { get; set; }
        public DateTime SignDate { get; set; }
        public EventSignStatusEm EventSignStatus { get; set; }
    }
}
