using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.enums;
using nsda.Models;

namespace nsda.Model.dto.response
{
    public class TrackCyclingPlayerResponse
    {
        public int id { get; set; }

        public int eventId { get; set; }

        public int eventGroupId { get; set; }

        public int cyclingMatchId { get; set; }

        public int playerId { get; set; }

        public int refereeId { get; set; }

        public string groupNum { get; set; }

        public int ranking { get; set; }

        public decimal score { get; set; }
    }
}
