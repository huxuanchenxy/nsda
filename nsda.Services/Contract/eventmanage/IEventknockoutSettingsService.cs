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
    /// 淘汰赛设置
    /// </summary>
    public interface IEventknockoutSettingsService : IDependency
    {
        //新增循环赛设置
        bool Settints(EventknockoutSettingsRequest request, out string msg);
        //循环赛详情
        EventknockoutSettingsResponse Detail(int eventId);
    }
}
