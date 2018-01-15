using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerTrainerResponse
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string ToMemberName { get; set; }
        public int MemberId { get; set; }
        public int ToMemberId { get; set; }
        public PlayerTrainerStatusEm PlayerTrainerStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Flag { get; set; }
    }
}

