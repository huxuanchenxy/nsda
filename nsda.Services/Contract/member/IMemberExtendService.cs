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
    //会员申请 增加角色
    public interface IMemberExtendService: IDependency
    {
        //1.0 申请
        bool Apply(MemberExtendRequest request,out string msg);
        //2.0 处理
        bool IsProcess(int id,int sysUserId,string remark, bool isAppro,out string msg);
        //3.0 会员扩展列表
        PagedList<MemberExtendResponse> List(MemberExtendQueryRequest request);
    }
}
