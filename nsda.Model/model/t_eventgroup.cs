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
	[Table("t_eventgroup")]
    public class t_eventgroup
    {
	   //默认构造函数
	   public t_eventgroup()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int eventId { get; set; }
    
        public string name { get; set; }
    
        /// <summary>
        /// 最小年级
        /// </summary>
        public int? mingrade { get; set; }
    
        /// <summary>
        /// 最大年级
        /// </summary>
        public int? maxgrade { get; set; }
    
        /// <summary>
        /// 最小积分
        /// </summary>
        public decimal? minPoints { get; set; }
    
        /// <summary>
        /// 最大积分
        /// </summary>
        public decimal? maxPoints { get; set; }
    
        /// <summary>
        /// 最小比赛次数
        /// </summary>
        public int? mintimes { get; set; }
    
        /// <summary>
        /// 最大比赛次数
        /// </summary>
        public int? maxtimes { get; set; }
    
        /// <summary>
        /// 队伍人数
        /// </summary>
        public int teamnumber { get; set; }
    
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
