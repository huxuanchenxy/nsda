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
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public EventSignStatusEm EventSignStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GroupNum { get; set; }
    }
    public class RefereeSignResponse
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public EventSignStatusEm EventSignStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
