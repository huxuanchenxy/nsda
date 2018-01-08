using nsda.Repository.Contract.eventmanage;
using nsda.Repository.Contract.member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Repository
{
    public class DataRepository: IDataRepository
    {
        public IMemberRepo MemberRepo { get; set; }
        public IEventRepo  EventRepo { get; set; }
        public IEventRoomRepo EventRoomRepo { get; set; }
        public ISignUpPlayerRepo SignUpPlayerRepo { get; set; }
    }
}
