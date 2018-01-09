using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    public interface IEventService: IDependency
    {
        //发起比赛
        bool Insert(out string msg);
        //编辑赛事
        bool Edit(out string msg);


        //设定赛事级别
        bool SettingLevel(int id, EventTypeEm eventType, int sysUserId, out string msg);
    }
}
