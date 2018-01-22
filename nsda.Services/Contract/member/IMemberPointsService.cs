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
    /// 会员积分管理
    /// </summary>
    public interface IMemberPointsService : IDependency
    {
        //会员积分
        MemberPointsModelResponse Detail(int memberId);
        //选手积分列表
        List<PlayerPointsRecordResponse> PlayerPointsRecord(PlayerPointsRecordQueryRequest request,out decimal totalPoints);
        List<PlayerPointsRecordResponse> PlayerPointsRecord(PlayerPointsRecordQueryRequest request);
        //选手积分详情
       List<PlayerPointsRecordDetailResponse> PointsRecordDetail(int recordId,int memberId);
    }
}
