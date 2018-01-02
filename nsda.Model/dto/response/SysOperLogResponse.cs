﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class SysOperLogResponse
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Operdata { get; set; }
        public string Operremark { get; set; }
        public int sysuserId { get; set; }
        public string OperUserName { get; set; }
    }
}
