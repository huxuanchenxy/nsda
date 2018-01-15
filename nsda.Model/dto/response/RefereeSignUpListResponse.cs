using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class RefereeSignUpListResponse
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int Id { get; set; }
        public int EventId { get; set; }
        public RefereeSignUpStatusEm RefereeSignUpStatus { get; set; }
        public int? GroupId { get; set; }
        public bool IsTemp { get; set; }
        public int MemberId { get; set; }
    }
}
