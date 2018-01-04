using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class SysUserResponse
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public SysUserStatusEm SysUserStatus { get; set; }
        public string Mobile { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}
