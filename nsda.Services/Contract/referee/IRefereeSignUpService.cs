﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.referee
{
    /// <summary>
    /// 裁判报名管理
    /// </summary>
    public interface IRefereeSignUpService : IDependency
    {
        // 申请当裁判
        bool Apply(int eventId, int memberId, out string msg);
        //裁判审核
        bool Check(int id, CheckRefereeEnum checkReferee, int memberId, out string msg);
        //裁判获取当天比赛信息
        List<RefereeCurrentEventResponse> CurrentRefereeEvent(int memberId);
        //赛事管理员 裁判报名列表
        List<EventRefereeSignUpListResponse> EventRefereeList(EventRefereeSignUpQueryRequest request);
        //裁判赛事报名列表
        List<RefereeSignUpListResponse> RefereeSignUpList(RefereeSignUpQueryRequest request);
        //裁判数据统计
        RefereeDataResponse RefereeData(int eventId, int memberId);
        //标记
        bool Flag(int id, int memberId, out string msg);
    }
}
