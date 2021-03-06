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
	[Table("t_event_cycling_match_playerresult")]
    public class t_event_cycling_match_playerresult
    {
	   //默认构造函数
	   public t_event_cycling_match_playerresult()
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
    
        public int cyclingMatchId { get; set; }
    
        public int playerId { get; set; }
    
        public int refereeId { get; set; }
    
        public string groupNum { get; set; }
    
        public int ranking { get; set; }
    
        public decimal score { get; set; }
    
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
