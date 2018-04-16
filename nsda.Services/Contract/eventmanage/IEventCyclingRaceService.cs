using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Models;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 循环赛管理
    /// </summary>
    public interface IEventCyclingRaceService : IDependency
    {
        //开始循环赛  生成循环赛对垒表
        bool Start(int eventId, int eventGroupId, out string msg);
        //开始下一轮
        bool Next(int eventId, int eventGroupId,int current, out string msg);
        List<TrackCyclingResponse> TrackCycling(int eventId, int eventGroupId, string keyValue);

        List<TrackCyclingResponse> TrackCyclingCur(int eventId, int eventGroupId, string keyValue);

        List<t_event_cycling_match> GetCurEventCyclingMatch(t_event_cycling cyc);

        //取当前比赛没结束的当前轮次的运行状态,左侧菜单跳转判断用,1就是未开始跳生成对垒表页面，2就是一开始只能跳track确认页面
        t_event_cycling GetCurCycling(int eventId, int eventGroupId);
    }
}
