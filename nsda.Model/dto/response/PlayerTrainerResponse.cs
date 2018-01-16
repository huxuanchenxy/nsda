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
        public string MemberCode { get; set; }
        public string ToMemberCode { get; set; }
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

    public class TrainerPlayerResponse
    {
        public int Id { get; set; }
        public string MemberCode { get; set; }
        public string ToMemberCode { get; set; }
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
        /// <summary>
        /// 当前积分
        /// </summary>
        public decimal MemberPoints { get; set; }
        /// <summary>
        /// 当前积分
        /// </summary>
        public decimal ToMemberPoints { get; set; }
        /// <summary>
        /// 执教期间比赛次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 执教期间获胜次数
        /// </summary>
        public int WinTimes { get; set; }
        /// <summary>
        /// 所在学校
        /// </summary>
        public string School { get; set; }
    }

}

