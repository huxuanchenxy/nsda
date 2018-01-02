using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.trainer
{
    /// <summary>
    /// 教练管理
    /// </summary>
    public interface ITrainerService : IDependency
    {
        //1.0 注册
        bool Register(out string msg);
        //1.1 登录
        bool Login(string account, string pwd, out string msg);
        //1.2 修改
        bool Edit(out string msg);
        //1.3 修改密码
        bool UpdatePwd(out string msg);
        //1.4 教练列表 
        void List();
        //1.6 手动添加临时教练信息
        bool AddTempTrainer(out string msg);
        //1.7 删除教练信息
        bool Delete(int id, out string msg);
        //1.8 重置密码
        bool Reset(int id, out string msg);
    }
}
