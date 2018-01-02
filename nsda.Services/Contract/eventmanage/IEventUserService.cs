using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.eventmanage
{
    /// <summary>
    /// 赛事管理员
    /// </summary>
    public interface IEventUserService : IDependency
    {
        //1.0 注册
        bool Register(out string msg);
        //1.1 登录
        bool Login(string account, string pwd, out string msg);
        //1.2 修改
        bool Edit(out string msg);
        //1.3 修改密码
        bool UpdatePwd(out string msg);
        //1.4 审核
        void Check();
        //1.5 赛事管理员列表 
        void List();
        //1.6 删除赛事管理信息
        bool Delete(int id, out string msg);
        //1.7 重置密码
        bool Reset(int id,out string msg);
    }
}
