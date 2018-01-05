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
        bool Insert(CityRequest request, out string msg);
        PagedList<CityResponse> List(CityQueryRequest request);
        bool Delete(int id, out string msg);
        CityResponse Detail(int id);
    }
}
