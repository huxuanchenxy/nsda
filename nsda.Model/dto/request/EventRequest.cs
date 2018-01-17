using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class EventRequest
    {
        public EventRequest()
        {
            EventGroup = new List<EventGroupRequest>();
        }
        public List<EventGroupRequest> EventGroup { get; set; }
        public int Id { get; set; }
        public int MemberId { get; set; }
        /// <summary>
        /// 赛事编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 赛事名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 赛事开始时间
        /// </summary>
        public DateTime StartEventDate { get; set; }

        /// <summary>
        /// 赛事结束时间
        /// </summary>
        public DateTime EndEventDate { get; set; }

        /// <summary>
        /// 报名开始时间
        /// </summary>
        public DateTime StartSignDate { get; set; }

        /// <summary>
        /// 报名结束时间
        /// </summary>
        public DateTime EndSignDate { get; set; }

        /// <summary>
        /// 退款截止时间
        /// </summary>
        public DateTime EndRefundDate { get; set; }

        /// <summary>
        /// 赛事描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 赛事地址
        /// </summary>
        public string Address { get; set; }

        public int CountryId { get; set; }

        public int ProvinceId { get; set; }

        public int CityId { get; set; }

        /// <summary>
        /// 是否是国际赛事
        /// </summary>
        public bool IsInter { get; set; }

        public EventTypeEm EventType { get; set; }

        public EventTypeNameEm EventTypeName { get; set; }

        public EventLevelEm? EventLevel { get; set; }

        /// <summary>
        /// 报名费
        /// </summary>
        public decimal Signfee { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string Filepath { get; set; }

        /// <summary>
        /// 最大队伍数
        /// </summary>
        public int Maxnumber { get; set; }

        /// <summary>
        /// 赛事状态
        /// </summary>
        public EventStatusEm EventStatus { get; set; }
    }

    public class EventGroupRequest
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 最小年级
        /// </summary>
        public int? MinGrade { get; set; }

        /// <summary>
        /// 最大年级
        /// </summary>
        public int? MaxGrade { get; set; }

        /// <summary>
        /// 最小积分
        /// </summary>
        public decimal? MinPoints { get; set; }

        /// <summary>
        /// 最大积分
        /// </summary>
        public decimal? MaxPoints { get; set; }

        /// <summary>
        /// 最小比赛次数
        /// </summary>
        public int? MinTimes { get; set; }

        /// <summary>
        /// 最大比赛次数
        /// </summary>
        public int? MaxTimes { get; set; }

        /// <summary>
        /// 队伍成员数
        /// </summary>
        public int TeamNumber { get; set; }

    }

    public class EventQueryRequest:PageQuery
    {
        public EventTypeEm? EventType { get; set; }
        public string KeyValue { get; set; }
        public int MemberId { get; set; }
    }

    public class EventManageQueryRequest : PageQuery
    {
        public EventStatusEm? EventStatus { get; set; }
        public EventTypeEm? EventType { get; set; }
        public string KeyValue { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
    }
}
