using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.response;
using nsda.Model.dto.request;

namespace nsda.Repository.Contract.eventmanage
{
    public interface IRoomRepo : IDependency
    {
        List<EventRoomResponse> GetList(EventRoomQueryRequest request);
    }
}
