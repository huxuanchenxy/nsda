using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class MemberPlayerResponse
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public CardTypeEm CardType { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string Card { get; set; }

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
        public GradeEm Grade { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEm Gender { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }
    }

    public class MemberCoachResponse
    {
        public int MemberId { get; set; }

        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }
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
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }


        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContactMobile { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }
    }

    public class MemberRefereeResponse
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }

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
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContactMobile { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }
    }

    public class MemberEventResponse
    {
        public int MemberId { get; set; }
        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public CardTypeEm CardType { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string Card { get; set; }
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
        /// 联系电话
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContactMobile { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress { get; set; }
        public GenderEm Gender { get; set; }
    }

    public class MemberResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 会员编码
        /// </summary>
        public string Code { get; set; }

        public MemberTypeEm MemberType { get; set; }

        /// <summary>
        /// 会员状态
        /// </summary>
        public MemberStatusEm MemberStatus { get; set; }

        public string Head { get; set; }

        public bool IsExtendPlayer { get; set; }

        public bool IsExtendCoach { get; set; }

        public bool IsExtendReferee { get; set; }

        /// <summary>
        /// 最晚登录时间
        /// </summary>
        public DateTime? Lastlogintime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        public bool IsDelete { get; set; }
    }
}
