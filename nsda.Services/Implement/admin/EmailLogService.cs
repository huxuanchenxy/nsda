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
    /// 邮件日志管理
    /// </summary>
    public class EmailLogService: IEmailLogService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public EmailLogService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 新增
        public  void Insert(EmailLogRequest request)
        {
            try
            {
                _dbContext.Insert(new t_sys_emaillog {
                    account = request.Account,
                    content = request.Content
                });
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EmailLogService.Insert", ex);
            }
        }
        //1.1 邮件日志列表
        public  List<EmailLogResponse> List(EmailLogQueryRequest request)
        {
            List<EmailLogResponse> list = new List<EmailLogResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Account.IsNotEmpty())
                {
                    request.Account = "%" + request.Account + "%";
                    join.Append(" and account like @Account");
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
                var  sql=$@"select * from t_sys_emaillog where isdelete=0 {join.ToString()} order by createtime desc";            
                int totalCount = 0;
                list = _dbContext.Page<EmailLogResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EmailLogService.List", ex);
            }
            return list;
        }
    }
}
