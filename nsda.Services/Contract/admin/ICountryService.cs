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
    public interface ICountryService : IDependency
    {
        bool Insert(CountryRequest request, int sysUserId, out string msg);
        bool Edit(CountryRequest request, int sysUserId, out string msg);
        PagedList<CountryResponse> List(CountryQueryRequest request);
        bool Delete(int id, int sysUserId, out string msg);
        CountryResponse Detail(int id);
        List<BaseDataResponse> Country();
    }
}
