using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum OrderStatusEm
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        等待支付 = 0,
        /// <summary>
        /// 支付中
        /// </summary>
        支付中 = 1,
        /// <summary>
        /// 支付失败
        /// </summary>
        支付失败 = 2,
        /// <summary>
        /// 支付成功
        /// </summary>
        支付成功 = 3,
        /// <summary>
        /// 订单取消
        /// </summary>
        订单取消 = 4,
        /// <summary>
        /// 已发货
        /// </summary>
        已发货 = 5,
        /// <summary>
        /// 已完成
        /// </summary>
        已完成 = 6,
        /// <summary>
        /// 退货中
        /// </summary>
        退货中 = 7,
        /// <summary>
        /// 退货成功
        /// </summary>
        退货完成 = 8,
        /// <summary>
        /// 订单取消中
        /// </summary>
        订单取消中 = 9,
        /// <summary>
        /// 换货中
        /// </summary>
        换货中 = 10,
        /// <summary>
        /// 换货成功
        /// </summary>
        换货完成 = 11,
        /// <summary>
        /// 退款中
        /// </summary>
        退款中 = 12,
        /// <summary>
        /// 退款失败
        /// </summary>
        退款失败 = 13,
        /// <summary>
        /// 退款成功
        /// </summary>
        退款成功 = 14
    }
}
