﻿using nsda.Repository;
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

                t_event t_event = _dbContext.Get<t_event>(request.FirstOrDefault().EventId);
                //数据校验
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }

                foreach (var item in request)
                {
                    if (item.Name.IsEmpty())
                    {
                        msg = "姓名不能为空";
                        break;
                    }
                    if (item.Email.IsEmpty())
                    {
                        msg = "邮箱不能为空";
                        break;
                    }
                    if (item.ContactMobile.IsEmpty())
                    {
                        msg = "联系方式不能为空";
                        break;
                    }
                }

                if (msg.IsNotEmpty())
                {
                    return flag;
                }

                _dbContext.BeginTransaction();
                string groupnum = _dataRepository.SignUpPlayerRepo.RenderCode();
                foreach (var item in request)
                {
                    //创建账号
                    string code = _dataRepository.MemberRepo.RenderCode();
                    t_member member = new t_member
                    {
                        code = code,
                        account = code,
                        name = item.Email,
                        pwd = item.ContactMobile,
                        memberStatus = MemberStatusEm.待认证,
                        completename = item.Name,
                        memberType = MemberTypeEm.临时选手
                    };
                    int memberId=_dbContext.Insert(member).ToObjInt();
                    _dbContext.Insert(new t_memberpoints {
                         memberId=memberId,
                         points=0,
                         eventPoints=0,
                         servicePoints=0
                    });
                    _dbContext.Insert(new t_membertemp {
                        code = groupnum,
                        contactmobile = item.ContactMobile,
                        email = item.Email,
                        eventId = item.EventId,
                        memberId = memberId,
                        tempStatus = TempStatusEm.待绑定,
                        tempType = TempTypeEm.临时裁判
                    });
                    //组队表
                    _dbContext.Insert(new t_player_signup {
                         groupnum=groupnum,
                         eventId=item.EventId,
                         groupId=item.GroupId,
                         isTemp=true,
                         memberId=memberId,
                         signfee= t_event.signfee,
                         signUpStatus=SignUpStatusEm.组队成功,
                         signUpType=SignUpTypeEm.被邀请人
                    });

                    if (item.PlayerEdu != null && item.PlayerEdu.SchoolId > 0)
                    {
                        _dbContext.Insert(new t_playereduexper {
                             startdate=item.PlayerEdu.StartDate,
                             enddate=item.PlayerEdu.EndDate,
                             memberId=memberId,
                             schoolId=item.PlayerEdu.SchoolId 
                        });
                    }
                }
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
                t_event t_event = _dbContext.Get<t_event>(request.EventId);
                //数据校验
                if (t_event == null)
                {
                    msg = "赛事信息有误";
                    return flag;
                }

                _dbContext.BeginTransaction();
                string code = _dataRepository.MemberRepo.RenderCode();
                t_member member = new t_member {
                    code = code,
                    account=code,
                    name=request.Name,
                    pwd=request.ContactMobile,
                    memberStatus=MemberStatusEm.已认证,
                    completename=request.Name,
                    memberType=MemberTypeEm.临时裁判
                };
                int memberId = _dbContext.Insert(member).ToObjInt();
                _dbContext.Insert(new t_memberpoints
                {
                    memberId = memberId,
                    points = 0,
                    eventPoints = 0,
                    servicePoints = 0
                });
                _dbContext.Insert(new t_membertemp {
                      code= member.code,
                      contactmobile=request.ContactMobile,
                      email=string.Empty,
                      eventId=request.EventId,
                      memberId=memberId,
                      tempStatus=TempStatusEm.待绑定,
                      tempType=TempTypeEm.临时裁判                            
                });
                _dbContext.Insert(new t_referee_signup {
                    isTemp=true,
                    eventId=request.EventId,
                    memberId=memberId,
                    refereeSignUpStatus=RefereeSignUpStatusEm.申请成功
                });
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
                _dbContext.BeginTransaction();
                //执行数据
                t_memberpoints points = _dbContext.Select<t_memberpoints>(c => c.memberId == data.memberId).FirstOrDefault();
                _dbContext.Execute($"update t_memberpoints set points=points+{points.points},eventPoints=eventPoints+{points.eventPoints},servicePoints=servicePoints+{points.servicePoints} where memberId={request.MemberId}");
                _dbContext.Execute($"update t_membertemp set tomemberId={request.MemberId},updateTime={DateTime.Now},tempStatus={TempStatusEm.已绑定}  where id={data.id}");
                _dbContext.Execute($"update t_memberpointsrecord set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                _dbContext.Execute($"update t_memberpointsdetail set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                _dbContext.Execute($"update t_player_signup set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");               
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
                _dbContext.BeginTransaction();
                t_memberpoints points = _dbContext.Select<t_memberpoints>(c => c.memberId == data.memberId).FirstOrDefault();
                _dbContext.Execute($"update t_memberpoints set points=points+{points.points},eventPoints=eventPoints+{points.eventPoints},servicePoints=servicePoints+{points.servicePoints} where memberId={request.MemberId}");
                _dbContext.Execute($"update t_membertemp set tomemberId={request.MemberId},updateTime={DateTime.Now},tempStatus={TempStatusEm.已绑定}  where id={data.id}");
                _dbContext.Execute($"update t_memberpointsrecord set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                _dbContext.Execute($"update t_memberpointsdetail set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
                _dbContext.Execute($"update t_referee_signup set memberId={request.MemberId} where memberId={data.memberId} and isdelete=0");
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
