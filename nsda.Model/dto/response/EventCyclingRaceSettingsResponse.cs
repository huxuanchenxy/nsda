using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventCyclingRaceSettingsResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public bool IsAllow { get; set; }

        public int StartRange { get; set; }

        public int EndRange { get; set; }

        public int Screenings { get; set; }

        public int RefereeCount { get; set; }

        public int TotalRound { get; set; }
        public string EventGroupName { get; set; }
        public List<EventCyclingRaceResponse> ListCyclingRace { get; set; }
        public EventCyclingRaceSettingsResponse()
        {
            ListCyclingRace = new List<EventCyclingRaceResponse>();
        }
    }

    public class EventCyclingRaceResponse
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
        public List<EventCyclingRaceDetailResponse> ListCyclingRaceDetail { get; set; }
        public EventCyclingRaceResponse()
        {
            ListCyclingRaceDetail = new List<EventCyclingRaceDetailResponse>();
        }
    }

    public class EventCyclingRaceDetailResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int EventGroupId { get; set; }
        public int CyclingRaceId { get; set; }
        public int Screenings { get; set; }
        public DateTime CompTime { get; set; }
    }
}
