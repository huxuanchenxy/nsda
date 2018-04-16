using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.enums;
using nsda.Models;

namespace nsda.Model.dto.response
{
    public class TrackCyclingResponse
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int eventGroupId { get; set; }
        public string progroupNum { get; set; }
        public string congroupNum { get; set; }
        public int roomId { get; set; }
        public int refereeId { get; set; }
        public int cyclingDetailId { get; set; }
        public bool isBye { get; set; }
        public CyclingMatchStatusEm cyclingMatchStatus { get; set; }
        public List<TrackCyclingTeamResponse> teams { get; set; }
    }
}
