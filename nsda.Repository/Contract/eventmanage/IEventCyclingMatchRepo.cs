using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Models;
using nsda.Model.dto.response;

namespace nsda.Repository.Contract.eventmanage
{
    public interface IEventCyclingMatchRepo : IDependency
    {
        /// <summary>
        /// 生成循环赛当前轮次的对垒表
        /// </summary>
        /// <param name="list"></param>
        void GenerEventCyclingMatch(List<t_event_cycling_match> list, t_event_cycling cyc);

        List<t_event_cycling_match> GetCurEventCyclingMatch(t_event_cycling cyc);

        void GotoNext(int eventId, int eventGroupId, int currentRound);
        List<TrackCyclingResponse> GetTrackCyclingCur(int eventId, int eventGroupId, string keyValue);
    }
}
