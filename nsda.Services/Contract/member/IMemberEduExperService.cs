using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.member
{
    /// <summary>
    /// 会员教育经历
    /// </summary>
    public interface IMemberEduExperService : IDependency
    {
        //1.0 添加教育经历
        bool Insert(out string msg);
        //1.1 修改教育经历
        bool Edit(out string msg);
        //1.2 删除教育经历
        bool Delete(out string msg);
        //1.3 教育经历列表
        void List();
    }
}
