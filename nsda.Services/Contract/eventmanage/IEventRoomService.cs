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
    /// 赛事教室管理
    /// </summary>
    public interface IEventRoomService : IDependency
    {
        //循环插入教室
        bool Insert(int eventId,int num,out string msg);
        //更新教室信息
        bool Edit(int id, string name, out string msg);
        //修改教室设置
        bool EidtSettings(int id, int statusOrGroup, out string msg);
        //更新教室信息
        bool SettingSpec(int id,int memberId, out string msg);
        //新增教室特殊限制
        bool ClearSpec(int id,out string msg);
        //赛事教室列表
        List<EventRoomResponse> List(EventRoomQueryRequest request);
    }
}
