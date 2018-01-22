using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 淘汰赛
    /// </summary>
    public interface IEventknockoutService : IDependency
    {
        bool Start(int eventId, out string msg);
        bool Next(int eventId, out string msg);
    }
}
