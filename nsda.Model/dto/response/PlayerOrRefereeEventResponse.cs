using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerOrRefereeEventResponse
    {
        /// <summary>
        /// 赛事Id
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// 赛事编码
        /// </summary>
        public string EventCode { get; set; }
        /// <summary>
        /// 赛事名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 赛事类型
        /// </summary>
        public EventTypeEm EventType { get; set; }
        /// <summary>
        /// 举办国家
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 举办省份
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 举办城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 赛事级别
        /// </summary>
        public EventLevelEm EventLevel { get; set; }
        /// <summary>
        /// 比赛开始时间
        /// </summary>
        public DateTime EventStartDate { get; set; }
        /// <summary>
        /// 报名费
        /// </summary>
        public decimal SignFee { get; set; }
        /// <summary>
        /// 是否显示报名
        /// </summary>
        public bool IsVisiable{ get; set; }
        /// <summary>
        /// 赛事状态
        /// </summary>
        public EventStatusEm EventStatus { get; set; }
        /// <summary>
        /// 比赛开始时间
        /// </summary>
        public DateTime EndSignDate { get; set; }
    }
}
