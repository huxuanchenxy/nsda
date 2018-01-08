using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MemberExtendRequest
    {
        public int MemberId { get; set; }
        public RoleEm RoleType { get; set; }
    }

    public class MemberExtendQueryRequest : PagedQuery
    {
        public RoleEm? RoleType { get; set; }
        public MemberExtendStatusEm? Status { get; set; }
    }
}
