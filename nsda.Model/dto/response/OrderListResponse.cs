using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class OrderListResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        /// <summary>
        /// 主订单id
        /// </summary>
        public int? MainOrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEm OrderType { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEm OrderStatus { get; set; }

        /// <summary>
        /// 最晚支付时间
        /// </summary>
        public DateTime PayExpiryDate { get; set; }

        /// <summary>
        /// 总折扣金额
        /// </summary>
        public decimal? TotalDiscount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal? TotalCoupon { get; set; }

        /// <summary>
        /// 是否需要发票
        /// </summary>
        public bool IsNeedInvoice { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单来源id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public bool IsDelete { get; set; }
    }

    public class OrderDetailResponse
    {
        public OrderDetailResponse()
        {
            OrderDetail = new List<OrderDetail>();
        }
        public int Id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        /// <summary>
        /// 主订单id
        /// </summary>
        public int? MainOrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEm OrderType { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEm OrderStatus { get; set; }

        /// <summary>
        /// 最晚支付时间
        /// </summary>
        public DateTime PayExpiryDate { get; set; }

        /// <summary>
        /// 总折扣金额
        /// </summary>
        public decimal? TotalDiscount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal? TotalCoupon { get; set; }

        /// <summary>
        /// 是否需要发票
        /// </summary>
        public bool IsNeedInvoice { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单来源id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public bool IsDelete { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }

    }

    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MemberId { get; set; }

        public string Name { get; set; }

        public int? ProductId { get; set; }

        public decimal Money { get; set; }

        public decimal UnitPrice { get; set; }

        public int Number { get; set; }

        public decimal? DiscountPrice { get; set; }

        public decimal? Coupon { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }


    public class RefundOrderListResponse
    {
        /// <summary>
        /// 操作id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public OrderOperTypeEm OrderOperType { get; set; }
        /// <summary>
        /// 申请标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public OperationStatusEm OperationStatus { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }
    }

    public class RefundOrderDetailResponse
    {
        /// <summary>
        /// 操作id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public OrderOperTypeEm OrderOperType { get; set; }
        /// <summary>
        /// 申请标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public OperationStatusEm OperationStatus { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
        public OrderPayLog OrderPayLogDetail { get; set; }
        public RefundOrderDetailResponse()
        {
            OrderDetail = new List<OrderDetail>();
        }
    }
    public class OrderPayLog
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int OrderId { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal? ActualAmount { get; set; }

        public decimal? PaymentFee { get; set; }

        public PayTypeEm PayType { get; set; }

        public string Paytransaction { get; set; }

        public PayStatusEm PayStatus { get; set; }

        public DateTime PayTime { get; set; }

        public DateTime? NotifyTime { get; set; }

        public string NotifyExt { get; set; }
    }
    public class OrderResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        /// <summary>
        /// 主订单id
        /// </summary>
        public int? MainOrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEm OrderType { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEm OrderStatus { get; set; }

        /// <summary>
        /// 最晚支付时间
        /// </summary>
        public DateTime PayExpiryDate { get; set; }

        /// <summary>
        /// 总折扣金额
        /// </summary>
        public decimal? TotalDiscount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal? TotalCoupon { get; set; }

        /// <summary>
        /// 是否需要发票
        /// </summary>
        public bool IsNeedInvoice { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单来源id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }

}
