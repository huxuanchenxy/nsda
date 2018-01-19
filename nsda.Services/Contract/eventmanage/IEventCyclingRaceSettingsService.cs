﻿using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    /// <summary>
    /// 循环赛设置
    /// </summary>
    public interface IEventCyclingRaceSettingsService : IDependency
    {
        //新增循环赛设置
        bool Settints(EventCyclingRaceSettingsRequest request,out string msg);
        //循环赛详情
        EventCyclingRaceSettingsResponse Detail(int eventId);
    }
}
