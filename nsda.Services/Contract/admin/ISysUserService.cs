using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
        bool Insert(SysUserRequest request,int sysUserId,out string msg);
        //1.1 登录
        bool Login(string account, string pwd, out string msg);
        //1.2 修改
        bool Edit(SysUserRequest request, int sysUserId, out string msg);
        //1.3 修改密码
        bool UpdatePwd(int id,string oldPwd,string newPwd,out string msg);
        //1.4 系统用户列表 
        List<SysUserResponse> List(SysUserQueryRequest request);
        //1.5 删除系统用户信息
        bool Delete(int id, int sysUserId, out string msg);
        //1.6 重置密码
        bool Reset(int id, int sysUserId, out string msg);
        //1.7 系统用户详情
        SysUserResponse Detail(int id);
        //1.8 启/禁用账号
        bool IsEnable(int id, bool isEnable, int sysUserId, out string msg);
    }
}
