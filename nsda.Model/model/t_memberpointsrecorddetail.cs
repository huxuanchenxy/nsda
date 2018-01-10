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
	[Table("t_memberpointsrecorddetail")]
    public class t_memberpointsrecorddetail
    {
	   //默认构造函数
	   public t_memberpointsrecorddetail()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 会员id 冗余字段
        /// </summary>
        public int memberId { get; set; }
    
        /// <summary>
        /// 积分记录id
        /// </summary>
        public int recordId { get; set; }
    
        /// <summary>
        /// 循环赛/淘汰赛
        /// </summary>
        public ObjEventTypeEm objEventType { get; set; }
    
        /// <summary>
        /// 积分
        /// </summary>
        public decimal points { get; set; }
    
        /// <summary>
        /// 获取积分备注
        /// </summary>
        public string remark { get; set; }
    
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
