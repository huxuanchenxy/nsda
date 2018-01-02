using nsda.Repository;
using nsda.Utilities;
using nsda.Utilities.Orm;
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
    public class MemberEduExperService: IMemberEduExperService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MemberEduExperService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 添加教育经历
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
                LogUtils.LogError("MemberEduExperService.Insert", ex);
            }
            return flag;
        }
        //1.1 修改教育经历
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
                LogUtils.LogError("MemberEduExperService.Edit", ex);
            }
            return flag;
        }
        //1.2 删除教育经历
        public  bool Delete(out string msg)
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
                LogUtils.LogError("MemberEduExperService.Delete", ex);
            }
            return flag;
        }
        //1.3 教育经历列表
        public  void List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberEduExperService.List", ex);
            }
        }
    }
}
