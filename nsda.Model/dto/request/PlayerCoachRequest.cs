using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerCoachRequest
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ObjMemberId { get; set; }
        public bool IsPositive { get; set; }
        public bool IsCoach { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class PlayerCoachQueryRequest:PageQuery
    {
        public int MemberId { get; set; }
    }
}
