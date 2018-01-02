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
                t_loginlog model = new t_loginlog {
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
        public PagedList<LoginLogResponse> List(LoginLogQueryRequest request)
        {
            PagedList<LoginLogResponse> list = new PagedList<LoginLogResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from t_loginlog where isdelete=0");
                if (request.Account.IsNotEmpty())
                {
                    request.Account = "%" + request.Account + "%";
                    sb.Append(" and account like @Account");
                }
                if (request.DataType>0)
                {
                    sb.Append(" and dataType = @DataType");
                }
                if (request.CreateStart.HasValue)
                {
                    sb.Append(" and createtime >= @CreateStart");
                }
                if (request.CreateEnd.HasValue)
                {
                    request.CreateEnd = request.CreateEnd.Value.AddDays(1).AddSeconds(-1);
                    sb.Append("  and createTime<=@CreateEnd");
                }
                list = _dbContext.Page<LoginLogResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LoginLogService.List", ex);
            }
            return list;
        }

    }
}
