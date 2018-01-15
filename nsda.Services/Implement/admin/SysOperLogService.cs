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
    /// 操作日志管理
    /// </summary>
    public class SysOperLogService: ISysOperLogService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public SysOperLogService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }

        //1.0 新增
        public  void Insert(SysOperLogRequest request)
        {
            try
            {
                t_sysoperlog model = new t_sysoperlog {
                     operdata=request.OperData,
                     operremark=request.OperRemark,
                     sysuserId=request.SysUserId
                };
                _dbContext.Insert(model);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysOperLogService.Insert", ex);
            }
        }
        //1.1 系统操作日志列表
        public  List<SysOperLogResponse> List(SysOperLogQueryRequest request)
        {
            List<SysOperLogResponse> list = new List<SysOperLogResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.name OperUserName from t_sysoperlog a 
                            inner join  t_sysuser b on a.sysuserId=b.id
                            where isdelete=0");
                if (request.OperData.IsNotEmpty())
                {
                    request.OperData = "%" + request.OperData + "%";
                    sb.Append(" and a.operdata like @OperData");
                }
                if (request.CreateStart.HasValue)
                {
                    sb.Append(" and a.createtime >= @CreateStart");
                }
                if (request.CreateEnd.HasValue)
                {
                    request.CreateEnd = request.CreateEnd.Value.AddDays(1).AddSeconds(-1);
                    sb.Append("  and a.createtime<=@CreateEnd");
                }
                int totalCount = 0;
                list = _dbContext.Page<SysOperLogResponse>(sb.ToString(), out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("SysOperLogService.List", ex);
            }
            return list;
        }
    }
}
