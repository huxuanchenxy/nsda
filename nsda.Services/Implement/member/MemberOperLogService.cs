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

        public PagedList<MemberOperLogResponse> List(MemberOperLogQueryRequest request)
        {
            PagedList<MemberOperLogResponse> list = new PagedList<MemberOperLogResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.name OperUserName from t_memberoperlog a 
                            inner join  t_member b on a.memberId=b.id
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
                    sb.Append("  and a.createTime<=@CreateEnd");
                }
                list = _dbContext.Page<MemberOperLogResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberOperLogService.List", ex);
            }
            return list;
        }
    }
}
