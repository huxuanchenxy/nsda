using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerSignUpResponse
    {
        public int Id { get; set; }
        /// <summary>
        /// 赛事id
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// 赛事开始时间
        /// </summary>
        public DateTime StartEventDate { get; set; }
        /// <summary>
        /// 赛事结束时间
        /// </summary>
        public DateTime EndEventDate { get; set; }
        /// <summary>
        /// 赛事名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 赛事编码
        /// </summary>
        public string EventCode { get; set; }
        /// <summary>
        /// 赛事类型
        /// </summary>
        public EventTypeEm EventType { get; set; }
        /// <summary>
        /// 举办城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 赛事状态
        /// </summary>
        public EventStatusEm EventStatus { get; set; }
        /// <summary>
        /// 报名状态
        /// </summary>
        public SignUpStatusEm SignUpStatus { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        public string MemberCode { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName { get; set; }
        public string GroupNum { get; set; }
        /// <summary>
        /// 队友id
        /// </summary>
        public int TeammateMemberId { get; set; }
        /// <summary>
        /// 队友名称
        /// </summary>
        public string TeammateMemberName { get; set; }
        /// <summary>
        /// 队友编码
        /// </summary>
        public string TeammateMemberCode { get; set; }
        /// <summary>
        /// 替换队友时 所需字段
        /// </summary>
        public int TeammateId { get; set; }
    }
}
