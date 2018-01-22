using nsda.Model.dto.request;
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
    public interface IEventMatchService : IDependency
    {
        //替换教室
        bool ReplaceRoom(out string msg);
        //替换裁判
        bool ReplaceReferee(out string msg);
        //替换对垒位置
        bool ReplaceMatch(out string msg);
        //预览评分单标签 或者打印评分单标签
        void PreviewRatingSinglLabel();
        //对垒表
        void ListMatch(ListMatchRequest request);
        //录入成绩
        bool RecordOfEntry(out string msg);
        //未录入成绩的对垒 已录入成绩的对垒
        void RecordOfList();
        //评分队伍
        void RecordOfDetail();
    }
}
