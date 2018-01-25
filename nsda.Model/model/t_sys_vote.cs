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
	[Table("t_sys_vote")]
    public class t_sys_vote
    {
	   //默认构造函数
	   public t_sys_vote()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 辩题标题
        /// </summary>
        public string title { get; set; }
    
        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }
    
        /// <summary>
        /// 投票开始时间
        /// </summary>
        public DateTime voteStartTime { get; set; }
    
        /// <summary>
        /// 投票结束时间
        /// </summary>
        public DateTime voteEndTime { get; set; }
    
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
