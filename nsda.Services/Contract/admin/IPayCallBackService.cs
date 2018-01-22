using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    public interface IPayCallBackService: IDependency
    {
        void Callback(string out_trade_no, string paytransaction);
    }
}
