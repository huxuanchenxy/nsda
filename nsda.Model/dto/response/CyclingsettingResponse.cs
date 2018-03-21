using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class CyclingsettingResponse
    {
        public List<EventGroupResponse> groups { get; set; }
        public List<EventCyclingRaceSettingsResponse> settings{ get; set; }
}


}
