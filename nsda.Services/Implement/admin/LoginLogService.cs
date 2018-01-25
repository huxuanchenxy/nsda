using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
    /// 登录日志管理
    /// </summary>
    public class LoginLogService: ILoginLogService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public LoginLogService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 新增
        public  void Insert(LoginLogRequest request)
        {
            try
            {
                t_sys_loginlog model = new t_sys_loginlog {
                    loginresult=request.LoginResult,
                    account=request.Account,
                    dataType=request.DataType,  
                };
                _dbContext.Insert(model);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LoginLogService.Insert", ex);
            }
        }
        //1.1 登录日志列表
        public List<LoginLogResponse> List(LoginLogQueryRequest request)
        {
            List<LoginLogResponse> list = new List<LoginLogResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Account.IsNotEmpty())
                {
                    request.Account = "%" + request.Account + "%";
                    join.Append(" and account like @Account");
                }
                if (request.DataType>0)
                {
                    join.Append(" and dataType = @DataType");
                }
                if (request.CreateStart.HasValue)
                {
                    join.Append(" and createtime >= @CreateStart");
                }
                if (request.CreateEnd.HasValue)
                {
                    request.CreateEnd = request.CreateEnd.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and createtime<=@CreateEnd");
                }
                var sql = $"select * from t_sys_loginlog where isdelete=0 {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<LoginLogResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LoginLogService.List", ex);
            }
            return list;
        }

    }
}
