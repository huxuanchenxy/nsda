﻿using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model
{
    public class ParamsConfig
    { 
        /// <summary>
        /// 参与比赛了的选手
        /// </summary>
        public static string _signup_notin = new int[] { (int)SignUpStatusEm.组队失败, (int)SignUpStatusEm.已退赛}.Splice();
        /// <summary>
        /// 会员类型
        /// </summary>
        public static string _membertype = new int[] { (int)MemberTypeEm.教练, (int)MemberTypeEm.裁判, (int)MemberTypeEm.选手, (int)MemberTypeEm.赛事管理员 }.Splice();
        /// <summary>
        /// 临时会员类型
        /// </summary>
        public static string _tempmembertype = new int[] { (int)MemberTypeEm.临时裁判, (int)MemberTypeEm.临时选手}.Splice();
        /// <summary>
        /// 实际比赛的状态
        /// </summary>
        public static string _signup_in = new int[] { (int)SignUpStatusEm.报名成功 }.Splice();
        /// <summary>
        /// 赛事可报名的状态
        /// </summary>
        public static string _eventstatus = new int[] { (int)EventStatusEm.停止报名,(int)EventStatusEm.报名中,(int)EventStatusEm.比赛中,(int)EventStatusEm.比赛完成}.Splice();
        /// <summary>
        /// 裁判可查询当日比赛的状态
        /// </summary>
        public static string _refereestatus = new int[] { (int)RefereeSignUpStatusEm.已录取}.Splice();  
        /// <summary>
        /// 赛事不可查询状态
        /// </summary>
        public static string _eventnoquerystatus = new int[] { (int)EventStatusEm.审核中, (int)EventStatusEm.拒绝 }.Splice();
    }
}
