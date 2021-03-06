﻿using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerCoachResponse
    {
        public int Id { get; set; }
        public string CoachCode { get; set; }
        public string CoachName { get; set; }
        public int MemberId { get; set; }
        public int ToMemberId { get; set; }
        public PlayerCoachStatusEm PlayerCoachStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Flag { get; set; }
    }

    public class CoachPlayerResponse
    {
        public int Id { get; set; }
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
        public string PlayerPinYinName { get; set; }
        public int MemberId { get; set; }
        public int ToMemberId { get; set; }
        public PlayerCoachStatusEm PlayerCoachStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Flag { get; set; }
        /// <summary>
        /// 当前积分
        /// </summary>
        public decimal PlayerPoints { get; set; }
        /// <summary>
        /// 执教期间比赛次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 执教期间获胜次数
        /// </summary>
        public int WinTimes { get; set; }
    }

    public class CurrentCoachResponse
    {
        public int Id { get; set; }
        public string CoachName { get; set; }
        public int CoachId { get; set; }
    }
}

