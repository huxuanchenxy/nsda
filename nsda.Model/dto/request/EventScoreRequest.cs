using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventScoreRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Title { get; set; }

        public string Remark { get; set; }

        public string FilePath { get; set; }
    }

    public class EventScoreQueryRequest:PagedQuery
    {
        public int EventId { get; set; }
    }

    public class PlayerEventScoreQueryRequest:PagedQuery
    {
        public int MemberId { get; set; }
    }
}
