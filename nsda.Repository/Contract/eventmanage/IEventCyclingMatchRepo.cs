using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Models;

namespace nsda.Repository.Contract.eventmanage
{
    public interface IEventCyclingMatchRepo : IDependency
    {
        /// <summary>
        /// 生成循环赛当前轮次的对垒表
        /// </summary>
        /// <param name="list"></param>
        void GenerEventCyclingMatch(List<t_event_cycling_match> list, t_event_cycling cyc);
    }
}
