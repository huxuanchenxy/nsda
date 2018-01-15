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
    public interface IDataSourceService: IDependency
    {
        //1.0 新增资料
        bool Insert(DataSourceRequest request,int sysUserId, out string msg);
        //2.0 编辑资料
        bool Edit(DataSourceRequest request, int sysUserId, out string msg);
        //3.0 删除资料
        bool Delete(int id, int sysUserId, out string msg);
        //4.0 资料详情
        DataSourceResponse Detail(int id);
        //5.0 资料列表
        List<DataSourceResponse> List(DataSourceQueryRequest request);
    }
}
