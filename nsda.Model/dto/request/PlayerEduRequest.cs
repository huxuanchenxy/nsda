﻿using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerEduRequest
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int MemberId { get; set; }
    }

    public class PlayerEduExperQueryRequest
    {
        public int MemberId { get; set; }
    }
}
