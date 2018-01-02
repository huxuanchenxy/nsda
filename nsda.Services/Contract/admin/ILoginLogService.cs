using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.admin
{
    /// <summary>
    /// 登录日志管理
    /// </summary>
    public interface ILoginLogService : IDependency
    {
        //1.0 新增
        void Insert(LoginLogRequest request);
        //1.1 登录日志列表
        PagedList<LoginLogResponse> List(LoginLogQueryRequest request);
    }
}
