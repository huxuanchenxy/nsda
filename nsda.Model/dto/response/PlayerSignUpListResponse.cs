using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventPlayerSignUpListResponse
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int Id { get; set; }
        public int EventId { get; set; }
        public SignUpStatusEm SignUpStatus { get; set; }
        public int GroupId { get; set; }
        public bool IsTemp { get; set; }
        public int MemberId { get; set; }
        public string GroupNum { get; set; }
        public GenderEm Gender { get; set; }
        public string ContactMobile { get; set; }
        public GradeEm Grade { get; set; }
        public string SchoolName { get; set; }
        public string CityName { get; set; }
    }

    public class PlayerSignUpListResponse
    {
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int Id { get; set; }
        public int EventId { get; set; }
        public SignUpStatusEm SignUpStatus { get; set; }
        public int EventGroupId { get; set; }
        public int MemberId { get; set; }
        public string GroupNum { get; set; }
        public string EventGroupName { get; set; }
    }


    public class PlayerRefundListResponse
    {
        public string EventName { get; set; }
        public string EventCode { get; set; }
        public EventTypeEm  EventType{ get; set; }
        public string EventGroupName { get; set; }
        public OrderStatusEm OrderStatus{ get; set; }
    }
}
