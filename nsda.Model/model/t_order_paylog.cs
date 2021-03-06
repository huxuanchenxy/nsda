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
	[Table("t_order_paylog")]
    public class t_order_paylog
    {
	   //默认构造函数
	   public t_order_paylog()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int memberId { get; set; }
    
        public int orderId { get; set; }
    
        public decimal paymentAmount { get; set; }
    
        public decimal? actualAmount { get; set; }
    
        public decimal? paymentFee { get; set; }
    
        public PayTypeEm payType { get; set; }
    
        public string paytransaction { get; set; }
    
        public PayStatusEm payStatus { get; set; }
    
        public DateTime payTime { get; set; }
    
        public DateTime? notifyTime { get; set; }
    
        public string notifyExt { get; set; }
    
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
