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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventName { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public decimal Points { get; set; }
        public int Id { get; set; }
    }

    public class PlayerPointsRecordDetailResponse
    {
        public string EventName { get; set; }
        public string GroupName { get; set; }
        public ObjEventTypeEm ObjEventType { get; set; }
        public decimal Points { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
