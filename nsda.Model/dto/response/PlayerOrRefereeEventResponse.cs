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
        public int Id { get; set; }
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
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 报名费
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否展示报名
        /// </summary>
        public bool IsVisiable{ get; set; }
        /// <summary>
        /// 赛事状态
        /// </summary>
        public EventStatusEm EventStatus { get; set; }
    }
}
