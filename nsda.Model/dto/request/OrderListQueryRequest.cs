using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class OrderListQueryRequest:PageQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderTypeEm? OrderType { get; set; }
        public OrderStatusEm? OrderStatus { get; set; }
        public string KeyValue { get; set; }
    }

    public class RefundOrderListQueryRequest : PageQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderOperTypeEm? OrderOperType { get; set; }
        /// <summary>
        /// 会员信息
        /// </summary>
        public string KeyValue { get; set; }
    }
}
