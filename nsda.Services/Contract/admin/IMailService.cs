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
    public interface IMailService:IDependency
    {
        //1.0 新增站内信
        void Insert(MailRequest request);
        //1.1 标记为已读
        bool Mark(List<int> id,int memberId,out string msg);
        //1.2 删除站内信
        bool Delete(int id, int memeberId, out string msg);
        //1.3 站内信列表
        PagedList<MailResponse> List(MailQueryRequest request);
    }
}
