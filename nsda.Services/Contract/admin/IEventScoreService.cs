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
        //1.0 新增赛事评分表
        bool Insert(EventScoreRequest request,out string msg);
        //2.0 编辑赛事评分表
        bool Edit(EventScoreRequest request, out string msg);
        //3.0 删除赛事评分表
        bool Delete(int id, out string msg);
        //4.0 赛事评分表详情
        EventScoreResponse Detail(int id);
        //5.0 赛事评分表列表
        List<EventScoreResponse> List(int eventId,int eventGroupId);
        //6.0 选手查询评分列表
        List<EventScoreResponse> PlayerList(PlayerEventScoreQueryRequest request);
    }
}
