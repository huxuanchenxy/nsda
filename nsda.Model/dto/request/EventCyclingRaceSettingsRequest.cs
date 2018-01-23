using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventCyclingRaceSettingsRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public bool IsAllow { get; set; }

        public int StartRange { get; set; }

        public int EndRange { get; set; }

        public int Screenings { get; set; }

        public int TotalRound { get; set; }
        public List<EventCyclingRaceRequest> ListCyclingRace { get; set; }
        public EventCyclingRaceSettingsRequest()
        {
            ListCyclingRace = new List<EventCyclingRaceRequest>();
        }
    }

    public class EventCyclingRaceRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int SettingsId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public PairRuleEm PairRule { get; set; }

        public int CurrentRound { get; set; }

        public int NextRound { get; set; }

        public CyclingRaceStatusEm CyclingRaceStatus { get; set; }
        public List<EventCyclingRaceDetailRequest> ListCyclingRaceDetail { get; set; }
        public EventCyclingRaceRequest()
        {
            ListCyclingRaceDetail = new List<EventCyclingRaceDetailRequest>();
        }
    }

    public class EventCyclingRaceDetailRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int CyclingRaceId { get; set; }

        public int Screenings { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
