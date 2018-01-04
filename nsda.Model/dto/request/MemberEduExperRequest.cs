using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MemberEduExperRequest
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string ReserveName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MemberId { get; set; }
    }

    public class MemberEduExperQueryRequest : PagedQuery
    {
        public int MemberId { get; set; }
    }
}
