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
	[Table("t_event")]
    public class t_event
    {
	   //默认构造函数
	   public t_event()
	    {
			//初始化默认字段
            createtime = DateTime.Now;
            updatetime = DateTime.Now;
            isdelete = false;
        }
       
        [Key]
	    public int id { get; set; }
    
        /// <summary>
        /// 赛事编码
        /// </summary>
        public string code { get; set; }
    
        /// <summary>
        /// 赛事名称
        /// </summary>
        public string name { get; set; }
    
        public string englishname { get; set; }
    
        public EventTypeEm eventType { get; set; }
    
        public EventTypeNameEm eventTypeName { get; set; }
    
        /// <summary>
        /// 赛事管理员id
        /// </summary>
        public int memberId { get; set; }
    
        /// <summary>
        /// 赛事开始时间
        /// </summary>
        public DateTime starteventdate { get; set; }
    
        /// <summary>
        /// 赛事结束时间
        /// </summary>
        public DateTime endeventdate { get; set; }
    
        /// <summary>
        /// 报名结束时间
        /// </summary>
        public DateTime endsigndate { get; set; }
    
        /// <summary>
        /// 退款截止时间
        /// </summary>
        public DateTime endrefunddate { get; set; }
    
        /// <summary>
        /// 赛事描述
        /// </summary>
        public string remark { get; set; }
    
        /// <summary>
        /// 赛事地址
        /// </summary>
        public string address { get; set; }
    
        public int countryId { get; set; }
    
        public int provinceId { get; set; }
    
        public int cityId { get; set; }
    
        /// <summary>
        /// 是否是国际赛事
        /// </summary>
        public bool isInter { get; set; }
    
        public EventLevelEm? eventLevel { get; set; }
    
        /// <summary>
        /// 报名费
        /// </summary>
        public decimal signfee { get; set; }
    
        /// <summary>
        /// 附件路径
        /// </summary>
        public string filepath { get; set; }
    
        public int maxnumber { get; set; }
    
        /// <summary>
        /// 赛事状态
        /// </summary>
        public EventStatusEm eventStatus { get; set; }
    
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
