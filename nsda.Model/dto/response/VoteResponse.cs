using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class VoteDetailResponse
    {
        public int Id { get; set; }
        public int NumberOfVotes { get; set; }
        public string Title { get; set; }
        public int VoteId { get; set; }
    }

    public class VoteResponse
    {
        public VoteResponse()
        {
            VoteDetail = new List<VoteDetailResponse>();
        }
        public List<VoteDetailResponse> VoteDetail{ get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public DateTime VoteStartTime { get; set; }
        public DateTime VoteEndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
