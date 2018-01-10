using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    public interface IEventService: IDependency
    {
        //发起比赛
        bool Insert(out string msg);
        //编辑赛事
        bool Edit(out string msg);
        // 赛事详情
        EventResponse Detail(int id);
        //设定赛事级别
        bool SettingLevel(int id, EventLevelEm eventLevel, int sysUserId, out string msg);
        //赛事列表查询
        PagedList<PlayerOrRefereeEventResponse> PlayerOrRefereeEvent(PlayerOrRefereeEventQueryRequest request);
    }
}
