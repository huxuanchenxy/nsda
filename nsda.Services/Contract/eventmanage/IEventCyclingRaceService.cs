﻿using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 循环赛管理
    /// </summary>
    public interface IEventCyclingRaceService : IDependency
    {
        //开始循环赛  生成循环赛对垒表
        bool Start(int eventId, out string msg);
        //开始下一轮
        bool Next(int eventId, out string msg);
    }
}
