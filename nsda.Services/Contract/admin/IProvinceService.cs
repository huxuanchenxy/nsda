using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.admin
{
    //省份 国家管理
    public interface IProvinceService: IDependency
    {
        bool Insert(ProvinceRequest request, out string msg);
        PagedList<ProvinceResponse> List(ProvinceQueryRequest request);
        bool Delete(int id, out string msg);
        ProvinceResponse Detail(int id);
    }
}
