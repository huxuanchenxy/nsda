using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.admin
{
    /// <summary>
    /// 操作日志管理
    /// </summary>
    public interface ISysOperLogService : IDependency
    {
        //1.0 新增
        void Insert(SysOperLogRequest request);
        //1.1 系统操作日志列表
        PagedList<SysOperLogResponse> List(SysOperLogQueryRequest request);
    }
}
