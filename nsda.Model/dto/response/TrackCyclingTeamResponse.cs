using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.enums;
using nsda.Models;

namespace nsda.Model.dto.response
{
    public class TrackCyclingTeamResponse
    {
        public int id { get; set; }

        public int eventId { get; set; }

        public int eventGroupId { get; set; }

        public int cyclingMatchId { get; set; }

        public string groupNum { get; set; }

        public decimal totalScore { get; set; }

        public bool isWin { get; set; }

        public WinTypeEm winType { get; set; }
        public List<t_event_cycling_match_playerresult> players { get; set; }
    }
}
