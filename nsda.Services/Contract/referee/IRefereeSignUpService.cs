using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.referee
{
    /// <summary>
    /// 裁判报名管理
    /// </summary>
    public interface IRefereeSignUpService : IDependency
    {
        // 申请当裁判
        bool Apply(int eventId, int memberId, out string msg);
    }
}
