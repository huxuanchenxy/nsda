﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
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
        bool Check(int id, bool isAppro, int memberId, out string msg);
        //裁判获取当天比赛信息
        List<CurrentEventResponse> CurrentRefereeEvent(int memberId);
        //赛事裁判报名列表
        List<RefereeSignUpListResponse> EventRefereeList(RefereeSignUpQueryRequest request);
        // 修改设置
        bool Settings(int id, int statusOrGroup, out string msg);
    }
}
