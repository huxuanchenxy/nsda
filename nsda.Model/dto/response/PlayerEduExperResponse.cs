﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerEduExperResponse
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string ReserveName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SchoolName { get; set; }
    }
}
