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
        public string Ids { get; set; }
        public string SignDates { get; set; }
        public string EventSignStatuss { get; set; }
        public List<PlayerSignSplitResponse> List { get; set; }
    }

    public class PlayerSignSplitResponse
    {
        public int Id { get; set; }
        public DateTime SignDate { get; set; }
        public EventSignStatusEm EventSignStatus{ get; set; }
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
    }

    public class RefereeSignSplitResponse
    {
        public int Id { get; set; }
        public DateTime SignDate { get; set; }
        public EventSignStatusEm EventSignStatus { get; set; }
    }
}
