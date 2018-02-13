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
    /// 常规奖项设置
    /// </summary>
    public interface IEventRegularAwardsService: IDependency
    {
        bool Settings(EventRegularAwardsRequest request,out string msg);
        EventRegularAwardsResponse Detail(int eventId, int eventGroupId);
    }
}
