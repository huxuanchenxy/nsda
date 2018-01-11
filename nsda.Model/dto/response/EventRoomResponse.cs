using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventRoomResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? EventGroupId { get; set; }
        public string EventGroupName { get; set; }
        public string Code{ get; set; }
        public string Name { get; set; }
        public RoomStatusEm RoomStatus { get; set; }
        public string MemberName { get; set; }
    }
}
