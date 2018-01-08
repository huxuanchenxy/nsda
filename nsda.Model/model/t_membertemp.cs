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
	[Table("t_membertemp")]
    public class t_membertemp
    {
	   //默认构造函数
	   public t_membertemp()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int memberId { get; set; }
    
        public TempTypeEm tempType { get; set; }
    
        public string code { get; set; }
    
        public string email { get; set; }
    
        public string contactmobile { get; set; }
    
        public int? tomemberId { get; set; }
    
        /// <summary>
        /// 临时会员状态
        /// </summary>
        public TempStatusEm tempStatus { get; set; }
    
        /// <summary>
        /// 具体赛事添加的临时会员
        /// </summary>
        public int eventId { get; set; }
    
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
