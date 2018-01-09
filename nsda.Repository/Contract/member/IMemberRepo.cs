using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository.Contract.member
{
    public interface IMemberRepo : IDependency
    {
        //生成会员编码
        string RenderCode(string code = "nsda");
        bool IsExist(string account);
    }
}
