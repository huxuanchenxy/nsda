using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventRoomQueryRequest:PageQuery
    {
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public int? GroupId { get; set; }
        public RoomStatusEm? RoomStatus { get; set; }
    }
}
