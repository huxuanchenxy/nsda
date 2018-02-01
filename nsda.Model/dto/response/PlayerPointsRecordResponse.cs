using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerPointsRecordResponse
    {
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string EventName { get; set; }
        public string EventCode { get; set; }
        //public string CityName { get; set; }
        //public string ProvinceName { get; set; }
        public int Points { get; set; }
        public int Id { get; set; }
        public string EventGroupName { get; set; }
        public EventTypeEm EventType { get; set; }
        public EventTypeNameEm EventTypeName { get; set; }
    }

    public class PlayerPointsRecordDetailResponse
    {
        public string GroupNum { get; set; }
        public string FromMemberCode { get; set; }
        public string FromMemberName { get; set; }
        public string ToMemberCode { get; set; }
        public string ToMemberName { get; set; }
    }
}
