using nsda.Services.Contract.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Utilities.Orm;
using nsda.Repository;
using nsda.Models;

namespace nsda.Services.Implement.member
{
    /// <summary>
    /// 会员操作日志
    /// </summary>
    public class MemberOperLogService : IMemberOperLogService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberOperLogService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public void Insert(MemberOperLogRequest request)
        {
            try
            {
                t_memberoperlog model = new t_memberoperlog
                {
                    operdata = request.OperData,
                    operremark = request.OperRemark,
                    memberId=request.MemberId
                };
                _dbContext.Insert(model);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberOperLogService.Insert", ex);
            }
        }

        public List<MemberOperLogResponse> List(MemberOperLogQueryRequest request)
        {
            List<MemberOperLogResponse> list = new List<MemberOperLogResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.OperData.IsNotEmpty())
                {
                    request.OperData = "%" + request.OperData + "%";
                    join.Append(" and a.operdata like @OperData");
                }
                if (request.CreateStart.HasValue)
                {
                    join.Append(" and a.createtime >= @CreateStart");
                }
                if (request.CreateEnd.HasValue)
                {
                    request.CreateEnd = request.CreateEnd.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and a.createtime<=@CreateEnd");
                }
                var sql=$@"select a.*,b.completename OperUserName from t_memberoperlog a 
                            inner join  t_member b on a.memberId=b.id
                            where isdelete=0 {join.ToString()} order by a.createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<MemberOperLogResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberOperLogService.List", ex);
            }
            return list;
        }
    }
}
