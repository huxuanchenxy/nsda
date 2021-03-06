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
	[Table("t_member")]
    public class t_member
    {
	   //默认构造函数
	   public t_member()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
    
        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }
    
        /// <summary>
        /// 会员编码
        /// </summary>
        public string code { get; set; }
    
        public MemberTypeEm memberType { get; set; }
    
        /// <summary>
        /// 会员状态
        /// </summary>
        public MemberStatusEm memberStatus { get; set; }
    
        public string head { get; set; }
    
        public bool isExtendPlayer { get; set; }
    
        public bool isExtendCoach { get; set; }
    
        public bool isExtendReferee { get; set; }
    
        /// <summary>
        /// 最晚登录时间
        /// </summary>
        public DateTime? lastlogintime { get; set; }
    
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
    
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? updatetime { get; set; }
    
        public bool isdelete { get; set; }
    }
}
