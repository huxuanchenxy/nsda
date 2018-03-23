using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class KnockoutsettingResponse
    {
        public List<EventGroupResponse> groups { get; set; }
        public List<EventknockoutSettingsResponse> settings { get; set; }
    }


}
