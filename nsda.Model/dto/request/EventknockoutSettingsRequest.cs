using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventknockoutSettingsRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int Teamnumber { get; set; }

        public List<EventKnockoutRequest> ListKnockout { get; set; }
        public EventknockoutSettingsRequest()
        {
            ListKnockout = new List<EventKnockoutRequest>();
        }
    }

    public class EventKnockoutRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int SettingsId { get; set; }

        public KnockoutTypeEm KnockoutType { get; set; }

        public int TrainerCount { get; set; }

        public KnockoutStatusEm KnockoutStatus { get; set; }

        public List<EventKnockoutDetailRequest> ListKnockoutDetail{ get; set; }
        public EventKnockoutRequest()
        {
            ListKnockoutDetail = new List<EventKnockoutDetailRequest>();
        }
    }

    public class EventKnockoutDetailRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int EventGroupId { get; set; }

        public int KnockoutId { get; set; }

        public int Screenings { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
