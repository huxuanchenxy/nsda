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
        bool Insert(ProvinceRequest request,int sysUserId,out string msg);
        bool Edit(ProvinceRequest request, int sysUserId, out string msg);
        List<ProvinceResponse> List(ProvinceQueryRequest request);
        bool Delete(int id, int sysUserId, out string msg);
        ProvinceResponse Detail(int id);
        List<BaseDataResponse> Province(bool? isInter);
    }
}
