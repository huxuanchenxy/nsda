using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    //临时选手
    public class TempPlayerRequest
    {
        /// <summary>
        /// 赛事id
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// 赛事组别id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// 报名时学校
        /// </summary>
        public int? SchoolId { get; set; }
    }

    //临时教练
    public class TempRefereeRequest
    {      
        /// <summary>
        /// 赛事id
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// 赛事组别id
        /// </summary>
        public int? GroupId { get; set; }
        public string Name { get; set; }
        public string ContactMobile { get; set; }
    }

    //绑定临时选手
    public class BindTempPlayerRequest
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 队伍编码
        /// </summary>
        public string GroupNum { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactMobile { get; set; }
    }

    //绑定临时裁判
    public class BindTempRefereeRequest
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 临时裁判编码
        /// </summary>
        public string TempRefereeNum { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactMobile { get; set; }
    }

    //临时会员数据查询
    public class TempMemberQueryRequest:PagedQuery
    {
        public TempTypeEm? TempType { get; set; }
        public TempStatusEm? TempStatus { get; set; }
    }
}
