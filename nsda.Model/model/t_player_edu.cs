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
	[Table("t_player_edu")]
    public class t_player_edu
    {
	   //默认构造函数
	   public t_player_edu()
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
        /// 开始时间
        /// </summary>
        public string startdate { get; set; }
    
        /// <summary>
        /// 结束时间
        /// </summary>
        public string enddate { get; set; }
    
        /// <summary>
        /// 学校id
        /// </summary>
        public int schoolId { get; set; }
    
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
