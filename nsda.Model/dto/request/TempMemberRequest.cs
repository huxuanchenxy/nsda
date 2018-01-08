using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    //临时选手
    public class TempPlayerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactMobile { get; set; }
        public int? SchoolId { get; set; }
    }

    //临时教练
    public class TempRefereeRequest
    {
        public string Name { get; set; }
        public string ContactMobile { get; set; }
    }
}
