using nsda.Model.dto.request;
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
        bool InsertTempPlayer(List<TempPlayerRequest> request,out string msg);
        //2.0 新增临时裁判
        bool InsertTempReferee(TempRefereeRequest request,out string msg);
        //3.0 临时选手绑定
        bool BindTempPlayer(BindTempPlayerRequest request,out string msg);
        //4.0 临时教练绑定
        bool BindTempReferee(BindTempRefereeRequest request,out string msg);
        //5.0 临时会员数据列表
        PagedList<MemberTempResponse> List(TempMemberQueryRequest request);
    }
}
