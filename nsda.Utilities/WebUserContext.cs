using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class WebUserContext
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public int MemberType  { get; set; }
        public int Status { get; set; }
        public string Role { get; set; }
    }
}
