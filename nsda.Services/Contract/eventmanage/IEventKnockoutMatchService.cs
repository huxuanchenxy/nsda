using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    public interface IEventKnockoutMatchService : IDependency
    {
        //替换教室
        bool ReplaceRoom(int knockoutMatchId, int roomId, int memberId, out string msg);
        //替换裁判
        bool ReplaceReferee(int knockoutMatchRefereeId, int refereeId, int memberId, out string msg);
        //正反方互换
        bool ReplaceMatch(int knockoutMatchId, int memberId, out string msg);
    }
}
