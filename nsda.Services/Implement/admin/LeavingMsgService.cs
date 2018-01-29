using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
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
    /// 留言表管理
    /// </summary>
    public class LeavingMsgService:ILeavingMsgService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public LeavingMsgService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        //1.0 留言
        public  bool Insert(LeavingMsgRequest request,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Name.IsEmpty())
                {
                    msg = "姓名不能为空";
                    return flag;
                }
                if (request.Email.IsEmpty())
                {
                    msg = "邮箱不能为空";
                    return flag;
                }
                if (!request.Email.IsSuccessEmail())
                {
                    msg = "邮箱格式有误";
                    return flag;
                }
                t_sys_leavingmsg model = new t_sys_leavingmsg {
                    email = request.Email,
                    name = request.Name,
                    message = request.Message,
                    mobile = request.Mobile,
                    leavingStatus =LeavingStatusEm.待处理
                };
                _dbContext.Insert(model);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("LeavingMsgService.Insert", ex);
            }
            return flag;
        }

        //1.1 处理留言
        public  bool Process(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_leavingmsg>(id);
                if (detail != null)
                {
                    detail.leavingStatus = LeavingStatusEm.已处理;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                }
                else {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("LeavingMsgService.Insert", ex);
            }
            return flag;
        }

        //1.3 留言列表
        public List<LeavingMsgResponse> List(LeavingMsgQueryRequest request)
        {
            List<LeavingMsgResponse> list = new List<LeavingMsgResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Name.IsNotEmpty())
                {
                    request.Name = $"%{request.Name}%";
                    join.Append(" and name like @Name");
                }
                if (request.Mobile.IsNotEmpty())
                {
                    request.Mobile = $"%{request.Mobile}%";
                    join.Append(" and mobile like @Mobile");
                }
                if (request.Email.IsNotEmpty())
                {
                    request.Email = $"%{request.Email}%";
                    join.Append(" and email like @Email");
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
                var sql = $"select * from t_sys_leavingmsg where isdelete=0 {join.ToString()} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<LeavingMsgResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LeavingMsgService.List", ex);
            }
            return list;
        }

        //1.3 删除留言
        public  bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_leavingmsg>(id);
                if (detail != null)
                {
                    detail.isdelete = true;
                    detail.updatetime = DateTime.Now;
                    _dbContext.Update(detail);
                    flag = true;
                }
                else
                {
                    msg = "数据不存在";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("LeavingMsgService.Delete", ex);
            }
            return flag;
        }
        
        //1.4 留言详情
        public LeavingMsgResponse Detail(int id)
        {
            LeavingMsgResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_sys_leavingmsg>(id);
                if (detail != null)
                {
                    response = new LeavingMsgResponse {
                        Id = detail.id,
                        IsDelete = detail.isdelete,
                        UpdateTime = detail.updatetime,
                        CreateTime = detail.createtime,
                        Email = detail.email,
                        Name = detail.name,
                        Mobile = detail.mobile,
                        Message = detail.message
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LeavingMsgService.Detail", ex);
            }
            return response;
        }
    }
}
