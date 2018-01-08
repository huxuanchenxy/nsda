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

namespace nsda.Services.Implement.member
{
    public class MemberTempService: IMemberTempService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberTempService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        //新增临时选手
        public bool InsertTempPlayer(List<TempPlayerRequest> request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request == null || request.Count == 0)
                {
                    msg = "请先录入数据";
                    return flag;
                }

                _dbContext.BeginTransaction();


                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("MemberTempService.InsertTempPlayer", ex);
            }
            return flag;
        }
        //新增临时裁判
        public bool InsertTempReferee(TempRefereeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系号码不能为空";
                    return flag;
                }

                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }

                _dbContext.BeginTransaction();

                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("MemberTempService.InsertTempReferee", ex);
            }
            return flag;
        }
        //临时选手绑定
        public bool BindTempPlayer(BindTempPlayerRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //校验数据的真实性
                if (request.GroupNum.IsEmpty())
                {
                    msg = "队伍编码不能为空";
                    return flag;
                }
                if (request.Email.IsEmpty())
                {
                    msg = "邮箱不能为空";
                    return flag;
                }
                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }

                var data = _dbContext.Select<t_membertemp>(c => c.email == request.Email && c.contactmobile == request.ContactMobile && c.code == request.GroupNum && c.tempType == Model.enums.TempTypeEm.临时选手 && c.tempStatus == Model.enums.TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在，请核对后再操作";
                    return flag;
                }
                data.tomemberId = request.MemberId;
                data.updatetime = DateTime.Now;
                data.tempStatus = Model.enums.TempStatusEm.已绑定;

                _dbContext.BeginTransaction();

                //执行数据
                _dbContext.Update(data);
                //牵涉到的数据转移到账号下


                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("MemberTempService.BindTempPlayer", ex);
            }
            return flag;
        }
        //临时教练绑定 
        public bool BindTempReferee(BindTempRefereeRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                //校验数据的真实性
                if (request.TempRefereeNum.IsEmpty())
                {
                    msg = "临时裁判不能为空";
                    return flag;
                }

                if (request.ContactMobile.IsEmpty())
                {
                    msg = "联系电话不能为空";
                    return flag;
                }

                var data = _dbContext.Select<t_membertemp>(c => c.contactmobile == request.ContactMobile && c.code == request.TempRefereeNum && c.tempType == Model.enums.TempTypeEm.临时裁判 && c.tempStatus == Model.enums.TempStatusEm.待绑定).FirstOrDefault();
                if (data == null)
                {
                    msg = "数据不存在，请核对后再操作";
                    return flag;
                }
                data.tomemberId = request.MemberId;
                data.updatetime = DateTime.Now;
                data.tempStatus = Model.enums.TempStatusEm.已绑定;

                _dbContext.BeginTransaction();
                //执行数据
                _dbContext.Update(data);
                //牵涉到的数据转移到账号下


                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                LogUtils.LogError("MemberTempService.BindTempReferee", ex);
            }
            return flag;
        }
        //临时会员数据列表
        public PagedList<MemberTempResponse> List(TempMemberQueryRequest request)
        {
            PagedList<MemberTempResponse> list = new PagedList<MemberTempResponse>();
            try
            {
                StringBuilder sb = new StringBuilder(@"select a.*,b.completename  MemberName,c.completename  ToMemberName,d.name EventName  from t_membertemp a 
                                                        inner join t_member b on a.memberId=b.Id 
                                                        left  join t_member c on a.tomemberId=c.Id 
                                                        inner join t_event  d on a.eventId=d.Id
                                                        where a.IsDelete=0
                                                     ");
                if (request.TempStatus.HasValue)
                {
                    sb.Append(" and a.tempStatus=@TempStatus");
                }
                if (request.TempType.HasValue)
                {
                    sb.Append(" and a.tempType=@TempType");
                }
                list = _dbContext.Page<MemberTempResponse>(sb.ToString(), request, pageindex: request.PageIndex, pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberTempService.List", ex);
            }
            return list;
        }
    }
}
