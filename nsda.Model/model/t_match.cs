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
	[Table("t_match")]
    public class t_match
    {
	   //默认构造函数
	   public t_match()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int eventId { get; set; }
    
        public int eventGroupId { get; set; }
    
        public string progroupNum { get; set; }
    
        public string congroupNum { get; set; }
    
        public int? roomId { get; set; }
    
        public ObjEventTypeEm objEventType { get; set; }
    
        public int objId { get; set; }
    
        public bool isBye { get; set; }
    
        public bool isConfirm { get; set; }
    
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
    
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
    
        public bool isdelete { get; set; }
    }
}
