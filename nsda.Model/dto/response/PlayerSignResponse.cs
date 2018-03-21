using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class PlayerSignResponse
    {
        public PlayerSignResponse()
        {
            List = new List<PlayerSignSplitResponse>();
        }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public string GroupNum { get; set; }
        public string SignDates { get; set; }
        public string EventSignStatuss { get; set; }
        public List<PlayerSignSplitResponse> List { get; set; }
        public string SchoolName { get; set; }
        public string CityName { get; set; }
        public string ContactMobile { get; set; }
        public GradeEm Grade { get; set; }
        public GenderEm Gender { get; set; }
        public bool IsStop { get; set; }
    }

    public class PlayerSignSplitResponse
    {
        public string SignDate { get; set; }
        public EventSignStatusEm EventSignStatus{ get; set; }
        public int SignType { get; set; }
    }

    public class RefereeSignResponse
    {
        public RefereeSignResponse()
        {
            List = new List<RefereeSignSplitResponse>();
        }
        public List<RefereeSignSplitResponse> List { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public string Ids { get; set; }
        public string SignDates { get; set; }
        public string EventSignStatuss { get; set; }
        public string ContactMobile { get; set; }
        public string EventGroupName { get; set; }
        public RefereeStatusEm RefereeStatus { get; set; }
        public int eventGroupId { get; set; }
    }

    public class RefereeSignSplitResponse
    {
        public string SignDate { get; set; }
        public EventSignStatusEm EventSignStatus { get; set; }
        public int SignType { get; set; }
    }
}
