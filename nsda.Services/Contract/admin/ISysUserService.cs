using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.admin
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public interface ISysUserService : IDependency
    {
        //1.0 添加系统用户
        bool Insert(out string msg);
        //1.1 登录
        bool Login(string account, string pwd, out string msg);
        //1.2 修改
        bool Edit(out string msg);
        //1.3 修改密码
        bool UpdatePwd(out string msg);
        //1.4 系统用户列表 
        void List();
        //1.5 删除系统用户信息
        bool Delete(int id, out string msg);
        //1.6 重置密码
        bool Reset(int id, out string msg);
    }
}
