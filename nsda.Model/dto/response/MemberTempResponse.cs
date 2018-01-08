using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MemberTempResponse
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public TempTypeEm tempType { get; set; }

        /// <summary>
        /// 队伍编码 或者裁判临时编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 绑定至此会员
        /// </summary>
        public int? ToMemberId { get; set; }

        /// <summary>
        /// 临时会员状态
        /// </summary>
        public TempStatusEm TempStatus { get; set; }

        /// <summary>
        /// 具体赛事添加的临时会员
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
