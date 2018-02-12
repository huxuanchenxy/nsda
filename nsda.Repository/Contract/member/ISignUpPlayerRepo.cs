using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository.Contract.member
{
    public interface ISignUpPlayerRepo : IDependency
    {
        //生成组别编码
        string RenderCode(int eventId, string code = "nsda");
    }
}
