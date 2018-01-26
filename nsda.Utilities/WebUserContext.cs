﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class WebUserContext
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public int MemberType  { get; set; }
        public decimal Points { get; set; }
        public bool IsExtendPlayer { get; set; }
        public bool IsExtendCoach { get; set; }
        public bool IsExtendReferee { get; set; }
        public int MemberStatus { get; set; }
    }
}
