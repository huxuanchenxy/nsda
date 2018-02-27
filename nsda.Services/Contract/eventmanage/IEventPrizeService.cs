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
    /// <summary>
    /// 赛事奖项设定
    /// </summary>
    public interface IEventPrizeService:IDependency
    {
        //新增奖项 
        bool Insert(EventPrizeRequest request,out string msg);
        //编辑奖项
        bool Edit(EventPrizeRequest request, out string msg);
        //删除奖项
        bool Delete(int id, int memberId, out string msg);
        //奖项列表
        List<EventPrizeResponse> List(int eventId, int eventGroupId);
        //奖项详情
        EventPrizeResponse Detail(int id);
    }
}
