using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class LoginRequest
    {
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string RedirectUrl { get; set; }
        public MemberTypeEm MemberType { get; set; }
    }
}
