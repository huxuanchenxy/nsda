using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 对垒业务操作
    /// </summary>
    public interface IEventCyclingMatchService : IDependency
    {
        //替换教室
        bool ReplaceRoom(int cyclingMatchId, int roomId,int memberId,out string msg);
        //替换裁判
        bool ReplaceReferee(int cyclingMatchId, int refereeId,int memberId, out string msg);
        //正反方互换
        bool ReplaceMatch(int cyclingMatchId, int memberId,out string msg);
        //预览评分单标签 或者打印评分单标签
        List<RatingSinglLabelResponse> PreviewRatingSinglLabel(MatchCommonRequest request);
        //对垒表
        List<ListMatchResponse> ListMatch(ListMatchRequest request);
        //录入成绩
        bool RecordOfEntry(RecordOfEntryRequest request,out string msg);
        //未录入成绩的对垒 已录入成绩的对垒
        List<RecordOfListResponse> RecordOfList(MatchCommonRequest request);
        //评分队伍
        RecordOfDetailResponse RecordOfDetail(RecordOfDetailRequest request);
        //闲置的裁判
        List<BaseListResponse> ListReferee(MatchCommonRequest request);
        //闲置的教室
        List<BaseListResponse> ListRoom(MatchCommonRequest request);
    }
}
