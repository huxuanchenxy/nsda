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
	[Table("t_signupform_referee")]
    public class t_signupform_referee
    {
	   //默认构造函数
	   public t_signupform_referee()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 裁判id
        /// </summary>
        public int memberId { get; set; }
    
        /// <summary>
        /// 赛事id
        /// </summary>
        public int eventId { get; set; }
    
        /// <summary>
        /// 赛事组别id
        /// </summary>
        public int groupId { get; set; }
    
        public RefereeSignUpStatusEm refereeSignUpStatus { get; set; }
    
        public bool isTemp { get; set; }
    
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
