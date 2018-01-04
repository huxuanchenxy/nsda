using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class VoteRequest
    {
        public int VoteId { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public DateTime VoteStartTime { get; set; }
        public DateTime VoteEndTime { get; set; }
        public List<VoteDetailRequest> VoteDetail { get; set; }
    }

    public class VoteDetailRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class VoteQueryRequest : PagedQuery
    {
        public string Title { get; set; }
        public string Remark { get; set; }
        public DateTime? VoteStartTime1 { get; set; }
        public DateTime? VoteStartTime2 { get; set; }
        public DateTime? VoteEndTime1 { get; set; }
        public DateTime? VoteEndTime2 { get; set; }
    }
}
