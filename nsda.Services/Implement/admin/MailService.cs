﻿using nsda.Services.Contract.admin;
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
using Dapper;

namespace nsda.Services.Implement.admin
{
    //站内信管理
    public class MailService : IMailService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        public MailService(IDBContext dbContext, IDataRepository dataRepository)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
        }
        //新增站内信
        public void Insert(MailRequest request)
        {
            try
            {
                _dbContext.Insert(new t_mail
                {
                    content = request.Content,
                    memberId = request.MemberId,
                    isRead = false,
                    mailType = request.MailType,
                    title = request.Title
                });
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MailService.Insert", ex);
            }
        }
        // 站内信列表
        public List<MailResponse> List(MailQueryRequest request)
        {
            List<MailResponse> list = new List<MailResponse>();
            try
            {
                var sql = $"select * from t_mail where isdelete=0 and memberId={request.MemberId} order by createtime desc";
                int totalCount = 0;
                list = _dbContext.Page<MailResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MailService.List", ex);
            }
            return list;
        }
        public List<MailResponse> List(int memberId)
        {
            List<MailResponse> list = new List<MailResponse>();
            try
            {
                var sql = $"select * from t_mail where isdelete=0 and memberId={memberId} order by createtime desc  limit 5";
                list = _dbContext.Query<MailResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MailService.List", ex);
            }
            return list;
        }
        //批量处理 未读站内信
        public bool Mark(int id, int memberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var sql = $"udpate t_mail set updatetime=@UpdateTime,isread=1 where Id = @Id and memberId=@MemberId";
                var dy = new DynamicParameters();
                dy.Add("UpdateTime", DateTime.Now);
                dy.Add("Id", id);
                dy.Add("MemberId", memberId);
                _dbContext.Execute(sql, dy);
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("MailService.Process", ex);
            }
            return flag;
        }
        //删除站内信
        public bool Delete(int id, int memeberId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_mail>(id);
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
                LogUtils.LogError("MailService.Process", ex);
            }
            return flag;
        }
    }
}
