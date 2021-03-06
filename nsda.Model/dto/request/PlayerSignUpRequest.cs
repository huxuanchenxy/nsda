﻿using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class PlayerSignUpRequest
    {
        /// <summary>
        /// 邀请者
        /// </summary>
        public int FromMemberId { get; set; }
        /// <summary>
        /// 被邀请者
        /// </summary>
        public int ToMemberId { get; set; }

        /// <summary>
        /// 赛事id
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 赛事组别
        /// </summary>
        public int EventGroupId { get; set; }
    }

    public class EventPlayerSignUpQueryRequest : PageQuery
    {
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public int? EventGroupId { get; set; }
        public SignUpStatusEm? SignUpStatus { get; set; }
        public string KeyValue { get; set; }
    }

    public class PlayerSignUpQueryRequest : PageQuery
    {
        public int MemberId { get; set; }
    }
}
