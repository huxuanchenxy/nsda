﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Models;
using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.admin;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Implement.admin
{
    /// <summary>
    /// 议题投票管理
    /// </summary>
    public class VoteService : IVoteService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        ISysOperLogService _sysOperLogService;
        public VoteService(IDBContext dbContext, IDataRepository dataRepository, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _sysOperLogService = sysOperLogService;
        }

        //删除辩题
        public bool Delete(int id, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_vote>(id);
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
                LogUtils.LogError("VoteService.Delete", ex);
            }
            return flag;
        }

        //辩题投票
        public bool Vote(int voteId, List<int> detailId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                var detail = _dbContext.Get<t_sys_vote>(voteId);
                if (detail != null)
                {
                    if (detail.voteStartTime < DateTime.Now && detail.voteEndTime > DateTime.Now)
                    {
                        var sql = "update t_sys_vote_detail set numberOfVotes=numberOfVotes+1 where Id in @Id and VoteId=@VoteId";
                        _dbContext.Execute(sql, new
                        {
                            Id = detailId.ToArray(),
                            VoteId = voteId
                        });
                        flag = true;
                    }
                    else
                    {
                        msg = "投票已结束";
                    }
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
                LogUtils.LogError("VoteService.Vote", ex);
            }
            return flag;
        }

        // 新增辩题
        public bool Insert(VoteRequest request, int sysUserId, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                if (request.Title.IsEmpty())
                {
                    msg = "辩题标题不能为空";
                    return flag;
                }

                if (request.VoteStartTime == DateTime.MinValue || request.VoteStartTime == DateTime.MaxValue|| request.VoteStartTime < DateTime.Now)
                {
                    msg = "投票开始时间有误";
                    return flag;
                }

                if (request.VoteEndTime == DateTime.MinValue || request.VoteEndTime == DateTime.MaxValue)
                {
                    msg = "投票结束时间有误";
                    return flag;
                }

                if (request.VoteStartTime > request.VoteEndTime)
                {
                    msg = "投票结束时间必须晚于开始时间";
                    return flag;
                }

                if (request.VoteDetail == null || request.VoteDetail.Count == 0)
                {
                    msg = "投票辩题不能为空";
                }
                try
                {
                    _dbContext.BeginTransaction();
                    request.VoteId = _dbContext.Insert(new t_sys_vote
                    {
                        remark = request.Remark,
                        title = request.Title,
                        voteEndTime = request.VoteEndTime,
                        voteStartTime = request.VoteStartTime
                    }).ToObjInt();

                    foreach (var item in request.VoteDetail)
                    {
                        _dbContext.Insert(new t_sys_vote_detail
                        {
                            numberOfVotes = 0,
                            title = item.Title,
                            voteId = request.VoteId
                        });
                    }
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("VoteService.InsertTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("VoteService.Insert", ex);
            }
            return flag;
        }

        //修改辩题
        public bool Update(VoteRequest request, int sysUserId, out string msg)
        {
            msg = string.Empty;
            bool flag = false;
            try
            {
                if (request.Title.IsEmpty())
                {
                    msg = "辩题标题不能为空";
                    return flag;
                }

                if (request.VoteStartTime == DateTime.MinValue || request.VoteStartTime == DateTime.MaxValue || request.VoteStartTime < DateTime.Now)
                {
                    msg = "投票开始时间有误";
                    return flag;
                }

                if (request.VoteEndTime == DateTime.MinValue || request.VoteEndTime == DateTime.MaxValue)
                {
                    msg = "投票结束时间有误";
                    return flag;
                }

                if (request.VoteStartTime > request.VoteEndTime)
                {
                    msg = "投票结束时间必须晚于开始时间";
                    return flag;
                }

                if (request.VoteDetail == null || request.VoteDetail.Count == 0)
                {
                    msg = "投票辩题不能为空";
                }

                var model = _dbContext.Get<t_sys_vote>(request.VoteId);
                model.updatetime = DateTime.Now;
                model.voteEndTime = request.VoteEndTime;
                model.voteStartTime = request.VoteStartTime;
                model.title = request.Title;
                model.remark = request.Remark;
                var voteDetail = _dbContext.Select<t_sys_vote_detail>(c => c.voteId == request.VoteId).ToList();
                try
                {
                    _dbContext.BeginTransaction();
                    if (voteDetail != null && voteDetail.Count > 0)
                    {
                        foreach (var item in voteDetail)
                        {
                            var detail = request.VoteDetail.FirstOrDefault(c => c.Id == item.id);
                            if (detail == null)
                            {
                                _dbContext.Delete(item);
                            }
                            else
                            {
                                item.title = detail.Title;
                                _dbContext.Update(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in request.VoteDetail)
                        {
                            _dbContext.Insert(new t_sys_vote_detail
                            {
                                numberOfVotes = 0,
                                title = item.Title,
                                voteId = request.VoteId
                            });
                        }
                    }
                    _dbContext.Update(model);
                    _dbContext.CommitChanges();
                    flag = true;
                }
                catch (Exception ex)
                {
                    _dbContext.Rollback();
                    flag = false;
                    msg = "服务异常";
                    LogUtils.LogError("VoteService.UpdateTran", ex);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("VoteService.Update", ex);
            }
            return flag;
        }

        //辩题详情
        public VoteResponse Detail(int id)
        {
            VoteResponse response = null;
            try
            {
                var detail = _dbContext.Get<t_sys_vote>(id);
                if (detail != null)
                {
                    response = new VoteResponse
                    {
                        Id = detail.id,
                        UpdateTime = detail.updatetime,
                        CreateTime = detail.createtime,
                        Remark = detail.remark,
                        Title = detail.title,
                        VoteEndTime = detail.voteEndTime,
                        VoteStartTime = detail.voteStartTime
                    };

                    var votedetail = _dbContext.Select<t_sys_vote_detail>(c => c.voteId == id).ToList();
                    if (votedetail != null && votedetail.Count > 0)
                    {
                        foreach (var item in votedetail)
                        {
                            response.VoteDetail.Add(new VoteDetailResponse
                            {
                                Id = item.id,
                                NumberOfVotes = item.numberOfVotes,
                                Title = item.title,
                                VoteId = item.voteId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("VoteService.Detail", ex);
            }
            return response;
        }

        //分页查询辩题
        public List<VoteResponse> List(VoteQueryRequest request)
        {
            List<VoteResponse> list = new List<VoteResponse>();
            try
            {
                StringBuilder join = new StringBuilder();
                if (request.Title.IsNotEmpty())
                {
                    request.Title = $"%{request.Title}%";
                    join.Append(" and title like @Title");
                }
                if (request.Remark.IsNotEmpty())
                {
                    request.Remark = $"%{request.Remark}%";
                    join.Append(" and remark like @Remark");
                }
                if (request.VoteStartTime1.HasValue)
                {
                    join.Append(" and voteStartTime >= @VoteStartTime1");
                }
                if (request.VoteStartTime2.HasValue)
                {
                    request.VoteStartTime2 = request.VoteStartTime2.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and voteStartTime<=@VoteStartTime2");
                }
                if (request.VoteEndTime1.HasValue)
                {
                    join.Append(" and voteEndTime >= @VoteEndTime1");
                }
                if (request.VoteStartTime2.HasValue)
                {
                    request.VoteEndTime1 = request.VoteEndTime1.Value.AddDays(1).AddSeconds(-1);
                    join.Append("  and voteEndTime<=@VoteEndTime2");
                }
                var sql=$@"select * from t_sys_vote where isdelete=0 {join.ToString()} order by createtime";                
                int totalCount = 0;
                list = _dbContext.Page<VoteResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("VoteService.List", ex);
            }

            return list;
        }
    }
}
