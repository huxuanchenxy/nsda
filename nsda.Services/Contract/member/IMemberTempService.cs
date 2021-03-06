﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.member
{
    /// <summary>
    /// 临时 选手、裁判管理
    /// </summary>
    public interface IMemberTempService : IDependency
    {
        //1.0 新增临时选手
        bool InsertTempPlayer(List<TempPlayerRequest> request, int memberId, out string msg);
        //2.0 新增临时裁判
        bool InsertTempReferee(TempRefereeRequest request,int memberId,out string msg);
        //3.0 临时选手绑定
        int BindTempPlayer(BindTempPlayerRequest request,out string msg);
        //4.0 临时教练绑定
        bool BindTempReferee(BindTempRefereeRequest request,out string msg);
        //5.0 临时选手会员数据列表
        List<MemberTempResponse> ListPlayer(TempMemberQueryRequest request);
        //5.0 临时裁判会员数据列表
        List<MemberTempResponse> ListReferee(TempMemberQueryRequest request);
        //6.0 临时会员绑定 支付回调
        void Callback(int memberId,int sourceId);
    }
}
