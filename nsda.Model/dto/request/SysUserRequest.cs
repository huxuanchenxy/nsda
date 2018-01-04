using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class SysUserRequest
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public SysUserStatusEm sysUserStatus { get; set; }
        public string Mobile { get; set; }
    }

    public class SysUserQueryRequest : PagedQuery
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DateTime? LastLoginTime1 { get; set; }
        public DateTime? LastLoginTime2 { get; set; }
    }
}
