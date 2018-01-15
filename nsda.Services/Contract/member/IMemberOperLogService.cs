using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.member
{
    public interface IMemberOperLogService : IDependency
    {
        //1.0 新增
        void Insert(MemberOperLogRequest request);
        //1.1 操作日志列表
        List<MemberOperLogResponse> List(MemberOperLogQueryRequest request);
    }
}
