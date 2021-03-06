﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MemberSelectResponse
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
    }

    public class InvitationMemberResponse
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public string SchoolName { get; set; }
        public string Grade { get; set; }
    }
}
