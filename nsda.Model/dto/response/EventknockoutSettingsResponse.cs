using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class EventknockoutSettingsResponse
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int Teamnumber { get; set; }

        public List<EventKnockoutResponse> ListKnockout { get; set; }
        public EventknockoutSettingsResponse()
        {
            ListKnockout = new List<EventKnockoutResponse>();
        }
    }

    public class EventKnockoutResponse
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int SettingsId { get; set; }

        public KnockoutTypeEm KnockoutType { get; set; }

        public int RefereeCount { get; set; }

        public KnockoutStatusEm KnockoutStatus { get; set; }

        public List<EventKnockoutDetailResponse> ListKnockoutDetail { get; set; }
        public EventKnockoutResponse()
        {
            ListKnockoutDetail = new List<EventKnockoutDetailResponse>();
        }
    }

    public class EventKnockoutDetailResponse
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int KnockoutId { get; set; }

        public int Screenings { get; set; }

        public DateTime StartTime { get; set; }

    }
}
