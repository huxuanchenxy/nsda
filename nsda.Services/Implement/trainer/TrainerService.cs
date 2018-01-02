using nsda.Models;
using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
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
    public class TrainerService: ITrainerService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public TrainerService(IDBContext dbContext, IDataRepository dataRepository)
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
                LogUtils.LogError("TrainerService.Register", ex);
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
                LogUtils.LogError("TrainerService.Login", ex);
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
                LogUtils.LogError("TrainerService.Edit", ex);
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
                LogUtils.LogError("TrainerService.UpdatePwd", ex);
            }
            return flag;
        }
        //1.4 教练列表 
        public  void List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("TrainerService.List", ex);
            }
        }
        //1.6 手动添加临时教练信息
        public  bool AddTempTrainer(out string msg)
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
                LogUtils.LogError("TrainerService.AddTempTrainer", ex);
            }
            return flag;
        }
        //1.7 删除教练信息
        public  bool Delete(int id, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var trainer = _dbContext.Get<t_trainer>(id);
                if (trainer != null)
                {
                    trainer.isdelete = true;
                    trainer.updatetime = DateTime.Now;
                    _dbContext.Update(trainer);
                    flag = true;
                }
                else
                {
                    msg = "裁判信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("TrainerService.Delete", ex);
            }
            return flag;
        }
        //1.8 重置密码
        public  bool Reset(int id,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var trainer = _dbContext.Get<t_trainer>(id);
                if (trainer != null)
                {
                    trainer.pwd = "159357";
                    trainer.updatetime = DateTime.Now;
                    _dbContext.Update(trainer);
                }
                else
                {
                    msg = "裁判信息不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("TrainerService.Reset", ex);
            }
            return flag;
        }
    }
}
