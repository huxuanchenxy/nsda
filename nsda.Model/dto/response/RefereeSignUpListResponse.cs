using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventRefereeSignUpListResponse
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int Id { get; set; }
        public int EventId { get; set; }
        public RefereeSignUpStatusEm RefereeSignUpStatus { get; set; }
        public int? EventGroupId { get; set; }
        public bool IsTemp { get; set; }
        public int MemberId { get; set; }
        public string Email { get; set; }
        public string ContactMobile { get; set; }
    }

    public class RefereeSignUpListResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventCode { get; set; }
        public string EventName { get; set; }
        public RefereeSignUpStatusEm RefereeSignUpStatus { get; set; }
        public EventTypeEm EventType { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
    }
}
