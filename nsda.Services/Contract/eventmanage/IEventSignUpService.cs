using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 赛事签到管理
    /// </summary>
    public interface IEventSignUpService: IDependency
    {
        //选手 教练签到
        bool SignUp(out string msg);
        //赛事管理员批量签到
        bool BatchSignUp(out string msg);
    }
}
