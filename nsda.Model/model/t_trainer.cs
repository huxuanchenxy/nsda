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
	[Table("t_trainer")]
    public class t_trainer
    {
	   //默认构造函数
	   public t_trainer()
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
    
        /// <summary>
        /// 中文名
        /// </summary>
        public string chinessname { get; set; }
    
        /// <summary>
        /// 拼音姓
        /// </summary>
        public string firstname { get; set; }
    
        /// <summary>
        /// 拼音名
        /// </summary>
        public string lastname { get; set; }
    
        /// <summary>
        /// 联系电话
        /// </summary>
        public string contactmobile { get; set; }
    
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string emergencycontact { get; set; }
    
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEm gender { get; set; }
    
        /// <summary>
        /// 会员状态
        /// </summary>
        public TrainerStatusEm trainerStatus { get; set; }
    
        /// <summary>
        /// 联系地址
        /// </summary>
        public string contactaddress { get; set; }
    
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
        public DateTime updatetime { get; set; }
    
        public bool isdelete { get; set; }
    }
}
