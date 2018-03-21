using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.response;
using nsda.Model.dto.request;
using nsda.Models;

namespace nsda.Repository.Contract.eventmanage
{
    public interface IRefereeSignRepo : IDependency
    {
        List<t_event_sign> RefereeSignData(int eventId, string manMemberId);

        void RefereeSignSetting(int memberId, int manMemberId, int refereeStatus, int eventGroupId,int eventid);
    }
}
