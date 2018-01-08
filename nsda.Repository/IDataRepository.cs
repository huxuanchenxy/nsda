using nsda.Repository.Contract.eventmanage;
using nsda.Repository.Contract.member;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository
{
    public interface IDataRepository: IDependency
    {
        IMemberRepo MemberRepo { get; set; }
        IEventRepo  EventRepo { get; set; }
        IEventRoomRepo EventRoomRepo { get; set; }
        ISignUpPlayerRepo SignUpPlayerRepo { get; set; }
    }
}
