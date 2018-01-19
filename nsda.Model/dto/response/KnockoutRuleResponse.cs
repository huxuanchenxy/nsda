using nsda.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Model.dto.response
{
    public class KnockoutRuleResponse
    {
        public KnockoutRuleResponse()
        {
            RefereeAvoidrule = new List<RefereeAvoidruleResponse>();
        }
        public List<RefereeAvoidruleResponse> RefereeAvoidrule { get; set; }
    }

    public class CyclingRaceRuleResponse
    {
        public CyclingRaceRuleResponse()
        {
            Scoringrule = new List<ScoringruleResponse>();
            Teamscoringrule = new List<TeamscoringruleResponse>();
            Avoidrule = new List<AvoidruleResponse>();
            RefereeAvoidrule = new List<RefereeAvoidruleResponse>();
        }
      
        public List<ScoringruleResponse> Scoringrule { get; set; }
        public List<TeamscoringruleResponse> Teamscoringrule { get; set; }
        public List<AvoidruleResponse> Avoidrule { get; set; }
        public List<RefereeAvoidruleResponse> RefereeAvoidrule { get; set; }
    }

    public class ScoringruleResponse
    {
        public int Id { get; set; }
        public ScoringRulesEm ScoringRules { get; set; }
        public int ViewIndex { get; set; }
        public int EventId { get; set; }
    }

    public class TeamscoringruleResponse
    {
        public int Id { get; set; }
        public TeamScoringRulesEm TeamScoringRules { get; set; }
        public int ViewIndex { get; set; }
        public int EventId { get; set; }
    }

    public class AvoidruleResponse
    {
        public int Id { get; set; }
        public AvoidRulesEm AvoidRules { get; set; }
        public int ViewIndex { get; set; }
        public int EventId { get; set; }
    }


    public class RefereeAvoidruleResponse
    {
        public int Id { get; set; }
        public RefereeAvoidRulesEm RefereeAvoidRules { get; set; }
        public int ViewIndex { get; set; }
        public int EventId { get; set; }
    }
}
