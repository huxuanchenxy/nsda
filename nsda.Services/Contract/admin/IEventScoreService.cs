using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    public interface IEventScoreService: IDependency
    {
        // 新增赛事评分表
        bool Insert(EventScoreRequest request,out string msg);
        // 删除赛事评分表
        bool Delete(int id,int sysUserId,out string msg);
        // 赛事评分表列表
        List<EventScoreResponse> List(int eventId,int eventGroupId);
        // 选手查询评分列表
        List<EventScoreResponse> PlayerList(PlayerEventScoreQueryRequest request);
    }
}
