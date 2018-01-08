using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository.Contract.eventmanage
{
    public interface IEventRepo: IDependency
    {
        string RenderCode(string code="nsda");
    }
}
