//-----------------------------------------------------------------------
//<copyright>
// * Copyright (C) 2018 NERICHIST All Rights Reserved
// * version : 4.0.30319.42000
// * author  : generated t4 by guanlei
// </copyright>
//-----------------------------------------------------------------------

using System;
using Dapper;
using nsda.Model.enums;
namespace nsda.Models
{
    /// <summary>
    /// TableModel
    /// </summary>    
	[Table("t_order")]
    public class t_order
    {
	   //默认构造函数
	   public t_order()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 会员id
        /// </summary>
        public int memberId { get; set; }
    
        /// <summary>
        /// 主订单id
        /// </summary>
        public int? mainOrderId { get; set; }
    
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderTypeEm orderType { get; set; }
    
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal money { get; set; }
    
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEm orderStatus { get; set; }
    
        /// <summary>
        /// 最晚支付时间
        /// </summary>
        public DateTime payExpiryDate { get; set; }
    
        /// <summary>
        /// 总折扣金额
        /// </summary>
        public decimal? totaldiscount { get; set; }
    
        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal? totalcoupon { get; set; }
    
        /// <summary>
        /// 是否需要发票
        /// </summary>
        public bool isNeedInvoice { get; set; }
    
        /// <summary>
        /// 订单备注
        /// </summary>
        public string remark { get; set; }
    
        /// <summary>
        /// 订单来源id
        /// </summary>
        public int sourceId { get; set; }
    
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
    
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
    
        public bool isdelete { get; set; }
    }
}
