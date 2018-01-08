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
	[Table("t_player_trainer")]
    public class t_player_trainer
    {
	   //默认构造函数
	   public t_player_trainer()
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
        /// 教练id
        /// </summary>
        public int objMemberId { get; set; }
    
        /// <summary>
        /// 状态
        /// </summary>
        public MemberTrainerStatusEm memberTrainerStatus { get; set; }
    
        /// <summary>
        /// 是否正面 1 选手绑定教练 2. 教练绑定选手
        /// </summary>
        public bool IsPositive { get; set; }
    
        /// <summary>
        /// 是否是教练
        /// </summary>
        public bool IsTrainer { get; set; }
    
        public DateTime startdate { get; set; }
    
        public DateTime? enddate { get; set; }
    
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
