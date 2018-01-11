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
	[Table("t_orderdetail")]
    public class t_orderdetail
    {
	   //默认构造函数
	   public t_orderdetail()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int orderId { get; set; }
    
        public int memberId { get; set; }
    
        public string name { get; set; }
    
        public int? productId { get; set; }
    
        public decimal money { get; set; }
    
        public decimal unitprice { get; set; }
    
        public int number { get; set; }
    
        public decimal? discountprice { get; set; }
    
        public decimal? coupon { get; set; }
    
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
