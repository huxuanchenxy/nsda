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
    public interface ISchoolService
    {
        bool Insert(SchoolRequest request, int sysUserId, out string msg);
        bool Edit(SchoolRequest request,int sysUserId, out string msg);
        bool Delete(int id, int sysUserId, out string msg);
        PagedList<SchoolResponse> List(SchoolQueryRequest request);
        SchoolResponse Detail(int id);
        List<BaseDataResponse> Select(int? provinceId, int? cityId, string name);
    }
}
