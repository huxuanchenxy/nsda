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
    /// 邮件日志管理
    /// </summary>
    public interface IEmailLogService : IDependency
    {
        //1.0 新增
        void Insert(EmailLogRequest request);
        //1.1 邮件日志列表
        PagedList<EmailLogResponse> List(EmailLogQueryRequest request);
    }
}
