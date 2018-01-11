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
	[Table("t_order_shoopingcart")]
    public class t_order_shoopingcart
    {
	   //默认构造函数
	   public t_order_shoopingcart()
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
        public int? memberId { get; set; }
    
        public string sessionId { get; set; }
    
        public int productId { get; set; }
    
        public int stockId { get; set; }
    
        public int priceId { get; set; }
    
        public string productAttribute { get; set; }
    
        public int number { get; set; }
    
        public bool isSelect { get; set; }
    
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
