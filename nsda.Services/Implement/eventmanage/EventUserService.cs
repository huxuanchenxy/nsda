using nsda.Models;
using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
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
    public class EventUserService: IEventUserService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EventUserService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 注册
        public  bool Register(out string msg)
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
                LogUtils.LogError("EventUserService.Register", ex);
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
                LogUtils.LogError("EventUserService.Login", ex);
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
                LogUtils.LogError("EventUserService.Edit", ex);
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
                LogUtils.LogError("EventUserService.UpdatePwd", ex);
            }
            return flag;
        }
        //1.4 审核
        public  void Check()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventUserService.Check", ex);
            }
        }
        //1.5 赛事管理员列表 
        public  void List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventUserService.List", ex);
            }
        }
        //1.6 删除赛事管理信息
        public  bool Delete(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var eventuser = _dbContext.Get<t_eventuser>(id);
                if (eventuser != null)
                {
                    eventuser.isdelete = true;
                    eventuser.updatetime = DateTime.Now;
                    _dbContext.Update(eventuser);
                    flag = true;
                }
                else
                {
                    msg = "赛事管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventUserService.Delete", ex);
            }
            return flag;
        }
        //1.7 重置密码
        public  bool Reset(int id,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var eventuser = _dbContext.Get<t_eventuser>(id);
                if (eventuser != null)
                {
                    eventuser.pwd = "159357";
                    eventuser.updatetime = DateTime.Now;
                    _dbContext.Update(eventuser);
                }
                else
                {
                    msg = "赛事管理员信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventUserService.Reset", ex);
            }
            return flag;
        }
    }
}
