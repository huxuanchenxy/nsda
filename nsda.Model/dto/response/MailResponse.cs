using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MailResponse
    {
        public int Id { get; set; }
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
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
