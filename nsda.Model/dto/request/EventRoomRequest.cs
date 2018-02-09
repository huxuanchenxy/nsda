using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventRoomRequest
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int EventGroupId { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
    }
}
