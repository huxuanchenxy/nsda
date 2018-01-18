using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.request
{
    public class CyclingRaceRuleRequest
    {
        public CyclingRaceRuleRequest()
        {
            Scoringrule = new List<ScoringruleRequest>();
            Teamscoringrule = new List<TeamscoringruleRequest>();
            Avoidrule = new List<AvoidruleRequest>();
            RefereeAvoidrule = new List<RefereeAvoidruleRequest>();
        }
        public List<ScoringruleRequest> Scoringrule { get; set; }
        public List<TeamscoringruleRequest> Teamscoringrule { get; set; }
        public List<AvoidruleRequest> Avoidrule { get; set; }
        public List<RefereeAvoidruleRequest> RefereeAvoidrule { get; set; }
        public int SysUserId { get; set; }
    }
    public class KnockoutRuleRequest
    {
        public KnockoutRuleRequest()
        {
            RefereeAvoidrule = new List<RefereeAvoidruleRequest>();
        }
        public List<RefereeAvoidruleRequest> RefereeAvoidrule { get; set; }
        public int SysUserId { get; set; }

    }

    public class ScoringruleRequest
    {
        public int Id { get; set; }
        public ScoringRulesEm ScoringRules { get; set; }
    }

    public class TeamscoringruleRequest
    { 
        public int Id { get; set; }
        public TeamScoringRulesEm TeamScoringRules { get; set; }
    }

    public class AvoidruleRequest
    {
        public int Id { get; set; }
        public AvoidRulesEm AvoidRules { get; set; }
    }


    public class RefereeAvoidruleRequest
    {
        public int Id { get; set; }
        public RefereeAvoidRulesEm RefereeAvoidRules { get; set; }
    }
}
