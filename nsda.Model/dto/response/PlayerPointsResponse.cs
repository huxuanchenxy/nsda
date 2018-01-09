using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerPointsResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventName { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public decimal Points { get; set; }
        public int Id { get; set; }
    }

    public class PlayerPointsRecordResponse
    {
        public string EventName { get; set; }
        public string GroupName { get; set; }
        public EventTypeEm EventType { get; set; }
        public decimal Points { get; set; }
        public string Remark { get; set; }
    }
}
