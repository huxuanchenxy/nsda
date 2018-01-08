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
    /// 留言表管理
    /// </summary>
    public interface ILeavingMsgService : IDependency
    {
        //1.0 留言
        bool Insert(LeavingMsgRequest request, out string msg);
        //1.1 处理留言
        bool Process(int id,int sysUserId, out string msg);
        //1.3 留言列表
        PagedList<LeavingMsgResponse> List(LeavingMsgQueryRequest requset);
        //1.3 删除留言
        bool Delete(int id, int sysUserId, out string msg);
        //1.4 留言详情
        LeavingMsgResponse Detail(int id);
    }
}
