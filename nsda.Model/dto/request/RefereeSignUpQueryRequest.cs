﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class RefereeSignUpQueryRequest:PagedQuery
    {
        public int MemberId { get; set; }
    }
}
