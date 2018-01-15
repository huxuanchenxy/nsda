using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MailRequest
    {
        /// <summary>
        /// 站内信类型
        /// </summary>
        public MailTypeEm MailType { get; set; }

        /// <summary>
        /// 站内信标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 站内信内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 跳转url
        /// </summary>
        public string Url { get; set; }
    }
    public class MailQueryRequest : PageQuery
    {
        public int MemberId { get; set; }
    }
}
