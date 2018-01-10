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
                var extend = _dbContext.Select<t_memberextend>(c => c.memberId == request.MemberId && c.memberExtendStatus != MemberExtendStatusEm.拒绝 && c.role == request.RoleType).ToList();
                if (extend != null && extend.Count > 0)
                {
                    msg = "已有您的申请，请等待管理员的审核";
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
        public bool Process(int id, int sysUserId, string remark, bool isAppro, out string msg)
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
        public PagedList<MemberExtendResponse> List(MemberExtendQueryRequest request)
        {
            PagedList<MemberExtendResponse> list = new PagedList<MemberExtendResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select a.*,b.completename OperUserName from t_memberextend a 
                            inner join  t_member b on a.memberId=b.id
                            where isdelete=0");
                if (request.RoleType.HasValue)
                {
                    sb.Append(" and a.roleType = @RoleType");
                }
                if (request.Status.HasValue)
                {
                    sb.Append(" and a.memberExtendStatus = @Status");
                }
                list = _dbContext.Page<MemberExtendResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberExtendService.List", ex);
            }
            return list;
        }
    }
}
