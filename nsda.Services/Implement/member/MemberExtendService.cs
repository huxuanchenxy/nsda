using nsda.Repository;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.enums;

namespace nsda.Services.Implement.member
{
    /// <summary>
    /// 会员扩展 
    /// </summary>
    public class MemberExtendService: IMemberExtendService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberExtendService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }
        //1.0 申请
        public bool Apply(MemberExtendRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var extend = _dbContext.Select<t_memberextend>(c => c.memberId == request.MemberId  && c.role == request.RoleType).FirstOrDefault();
                if (extend != null && extend.memberExtendStatus == MemberExtendStatusEm.申请通过)
                {
                    msg = "您的申请已经通过，请刷新页面后重试";
                    return flag;
                }

                if (extend != null)
                {
                    extend.updatetime = DateTime.Now;
                    extend.memberExtendStatus = MemberExtendStatusEm.待审核;
                    _dbContext.Update(extend);
                }
                else
                {
                    _dbContext.Insert(new t_memberextend
                    {
                        memberExtendStatus = MemberExtendStatusEm.待审核,
                        memberId = request.MemberId,
                        role = request.RoleType,
                        remark = string.Empty
                    });
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberExtendService.Apply", ex);
            }
            return flag;
        }
        //2.0 管理员审核处理
        public bool Process(int id, string remark, bool isAppro, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_memberextend>(id);
                if (detail != null)
                {
                    detail.remark = remark;
                    detail.memberExtendStatus = isAppro ? MemberExtendStatusEm.申请通过 : MemberExtendStatusEm.待审核;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    if (detail.role == RoleEm.选手&&isAppro)
                    {
                       _dbContext.Execute($"update t_member set memberStatus={MemberStatusEm.待认证} where id={detail.memberId}");
                    }
                    flag = true;
                }
                else
                {
                    msg = "数据信息不存在";
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberExtendService.Process", ex);
            }
            return flag;
        }
        //3.0 申请列表
        public List<MemberExtendResponse> List(MemberExtendQueryRequest request)
        {
            List<MemberExtendResponse> list = new List<MemberExtendResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.RoleType.HasValue && request.RoleType > 0)
                {
                    join.Append(" and a.role = @RoleType");
                }
                if (request.Status.HasValue && request.Status > 0)
                {
                    join.Append(" and a.memberExtendStatus = @Status");
                }
                var sql=$@" select a.*,b.completename OperUserName from t_memberextend a 
                            inner join  t_member b on a.memberId=b.id
                            where isdelete=0 {join.ToString()} order by a.createtime desc";

                int totalCount = 0;
                list = _dbContext.Page<MemberExtendResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberExtendService.List", ex);
            }
            return list;
        }

        public List<RoleEm> List(int memberId)
        {
            List<RoleEm> list = new List<RoleEm>();
            try
            {
                var sql = $" select role from t_memberextend  where isdelete=0 and memberId={memberId}";
                list = _dbContext.Query<RoleEm>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberExtendService.List", ex);
            }
            return list;
        }
    }
}
