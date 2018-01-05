using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    public interface IBaseService: IDependency
    {
        //省份列表
        List<BaseDataResponse> Province();
        // 城市列表
        List<BaseDataResponse> City(int provinceId);
    }
}
