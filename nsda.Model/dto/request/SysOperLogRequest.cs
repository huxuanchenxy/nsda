﻿using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class SysOperLogRequest
    {
        public int SysUserId { get; set; }
        public string OperData { get; set; }
        public string OperRemark { get; set; }
    }

    public class SysOperLogQueryRequest : PageQuery
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        public string OperData { get; set; }
        public DateTime? CreateStart { get; set; }
        public DateTime? CreateEnd { get; set; }
    }
}
