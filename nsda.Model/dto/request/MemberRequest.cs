﻿using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class MemberRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public CardTypeEm? CardType { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 完整姓名
        /// </summary>
        public string CompleteName { get; set; }

        /// <summary>
        /// 拼音姓
        /// </summary>
        public string PinYinSurName { get; set; }

        /// <summary>
        /// 拼音名
        /// </summary>
        public string PinYinName { get; set; }

        /// <summary>
        /// 完整拼音姓名
        /// </summary>
        public string CompletePinYin { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        public GradeEm? Grade { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEm? Gender { get; set; }

        public MemberTypeEm MemberType { get; set; }

        /// <summary>
        /// 会员状态
        /// </summary>
        public MemberStatusEm MemberStatus { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }
    }

    public class MemberQueryRequest : PagedQuery
    {
        public MemberStatusEm? MemberType { get; set; }
    }
}
