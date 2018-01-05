using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MemberTrainerRequest
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ObjMemberId { get; set; }
        public bool Flag { get; set; }
        public MemberTrainerStatusEm memberTrainerStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class MemberTrainerQueryRequest:PagedQuery
    {

    }
}
