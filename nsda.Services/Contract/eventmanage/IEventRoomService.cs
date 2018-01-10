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
        bool Insert(out string msg);
        bool Update(out string msg);
        bool Delete(int id,string msg);
    }
}
