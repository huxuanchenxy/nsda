using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    public interface IEventRuleService: IDependency
    {
        //编辑循环赛规则
        bool CyclingRaceRule(CyclingRaceRuleRequest request,out string msg);
        //编辑淘汰赛规则
        bool KnockoutRule(KnockoutRuleRequest request,out string msg);
        //淘汰赛规则详情
        KnockoutRuleResponse KnockoutRuleDetail(int eventId);
        //循环赛规则详情
        CyclingRaceRuleResponse CyclingRaceRuleDetail(int eventId);
    }
}
