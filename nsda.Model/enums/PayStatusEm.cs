using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.enums
{
    /// <summary>
    /// 支付状态枚举
    /// </summary>
    public enum PayStatusEm
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
        /// 预授权成功
        /// </summary>
        预授权成功 = 5,
        /// <summary>
        /// 预授权完成
        /// </summary>
        预授权完成 = 6,
        /// <summary>
        /// 预授权撤消
        /// </summary>
        预授权撤消 = 7,
        /// <summary>
        /// 预授权完成(可无)
        /// </summary>
        预授权完成撤消 = 8,
        /// <summary>
        /// 退款
        /// </summary>
        退款 = 9,
        /// <summary>
        /// 退款中
        /// </summary>
        退款中 = 10,
        /// <summary>
        /// 退款失败
        /// </summary>
        退款失败 = 11,
        /// <summary>
        /// 退款成功
        /// </summary>
        退款成功 = 12,
        /// <summary>
        /// 放弃该支付方式
        /// </summary>
        放弃支付方式 = 13,
        /// <summary>
        /// 支付取消
        /// </summary>
        支付取消 = 14,
        /// <summary>
        /// 支付取消失败
        /// </summary>
        支付取消失败 = 15,
        /// <summary>
        /// 预授权失败
        /// </summary>
        预授权失败 = 16,
        /// <summary>
        /// 取消预授权失败
        /// </summary>
        取消预授权失败 = 17,
        /// <summary>
        /// 取消预授权完成失败
        /// </summary>
        取消预授权完成失败 = 18,
        /// <summary>
        /// 预授权完成失败
        /// </summary>
        预授权完成失败 = 19
    }

}
