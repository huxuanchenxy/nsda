﻿using nsda.Model.dto.request;
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
        public LeavingMsgService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
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
                t_leavingmsg model = new t_leavingmsg {
                    email = request.Email,
                    name = request.Name,
                    message = request.Message,
                    mobile = request.Mobile,
                    leavingStatus =LeavingStatusEm.待处理
                };
                _dbContext.Insert(model);
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
        public  bool Process(int id,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_leavingmsg>(id);
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
        public PagedList<LeavingMsgResponse> List(LeavingMsgQueryRequest request)
        {
            PagedList<LeavingMsgResponse> list = new PagedList<LeavingMsgResponse>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from t_leavingmsg where isdelete=0");
                if (request.Name.IsNotEmpty())
                {
                    request.Name = "%" + request.Name + "%";
                    sb.Append(" and name like @Name");
                }
                if (request.Mobile.IsNotEmpty())
                {
                    request.Mobile = "%" + request.Mobile + "%";
                    sb.Append(" and mobile like @Mobile");
                }
                if (request.Email.IsNotEmpty())
                {
                    request.Email = "%" + request.Email + "%";
                    sb.Append(" and email like @Email");
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
                list = _dbContext.Page<LeavingMsgResponse>(sb.ToString(), request, pageindex: request.PageIndex,pagesize: request.PagesSize);
            }
            catch (Exception ex)
            {
                LogUtils.LogError("LeavingMsgService.List", ex);
            }
            return list;
        }

        //1.3 删除留言
        public  bool Delete(int id,out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_leavingmsg>(id);
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
                var detail = _dbContext.Get<t_leavingmsg>(id);
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
