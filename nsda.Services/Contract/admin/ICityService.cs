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
    public interface ICityService: IDependency
    {
        bool Insert(CityRequest request,int sysUserId,out string msg);
        bool Edit(CityRequest request, int sysUserId, out string msg);
        PagedList<CityResponse> List(CityQueryRequest request);
        bool Delete(int id, int sysUserId, out string msg);
        CityResponse Detail(int id);
        List<BaseDataResponse> City(int provinceId);
    }
}
