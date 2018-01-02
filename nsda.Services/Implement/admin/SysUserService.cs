using nsda.Models;
using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
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
    public class SysUserService: ISysUserService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public SysUserService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 添加系统用户
        public  bool Insert(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Insert", ex);
            }
            return flag;
        }
        //1.1 登录
        public  bool Login(string account, string pwd, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Login", ex);
            }
            return flag;
        }
        //1.2 修改
        public  bool Edit(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Edit", ex);
            }
            return flag;
        }
        //1.3 修改密码
        public  bool UpdatePwd(out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.UpdatePwd", ex);
            }
            return flag;
        }
        //1.4 系统用户列表 
        public  void List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysUserService.List", ex);
            }
        }
        //1.5 删除系统用户信息
        public  bool Delete(int id,  out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    sysuser.isdelete = true;
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                    flag = true;
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Delete", ex);
            }
            return flag;
        }
        //1.6 重置密码
        public  bool Reset(int id,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sysuser = _dbContext.Get<t_sysuser>(id);
                if (sysuser != null)
                {
                    sysuser.pwd = "159357";
                    sysuser.updatetime = DateTime.Now;
                    _dbContext.Update(sysuser);
                }
                else
                {
                    msg = "管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("SysUserService.Reset", ex);
            }
            return flag;
        }
    }
}
