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
	[Table("t_member_event")]
    public class t_member_event
    {
	   //默认构造函数
	   public t_member_event()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        public int memberId { get; set; }
    
        public CardTypeEm cardType { get; set; }
    
        public string card { get; set; }
    
        /// <summary>
        /// 会员编码
        /// </summary>
        public string code { get; set; }
    
        /// <summary>
        /// 姓
        /// </summary>
        public string surname { get; set; }
    
        /// <summary>
        /// 名
        /// </summary>
        public string name { get; set; }
    
        /// <summary>
        /// 完整姓名
        /// </summary>
        public string completename { get; set; }
    
        /// <summary>
        /// 拼音姓
        /// </summary>
        public string pinyinsurname { get; set; }
    
        /// <summary>
        /// 拼音名
        /// </summary>
        public string pinyinname { get; set; }
    
        /// <summary>
        /// 完整拼音姓名
        /// </summary>
        public string completepinyin { get; set; }
    
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
    
        public string emergencycontactmobile { get; set; }
    
        /// <summary>
        /// 联系地址
        /// </summary>
        public string contactaddress { get; set; }
    
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
