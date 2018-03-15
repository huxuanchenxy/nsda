using Dapper;
using nsda.Repository.Contract.eventmanage;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.response;
using nsda.Model.dto.request;

namespace nsda.Repository.Implement.eventmanage
{
    public class RoomRepo : IRoomRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public RoomRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EventRoomResponse> GetList(EventRoomQueryRequest request)
        {
            List<EventRoomResponse> list = new List<EventRoomResponse>();

            StringBuilder join = new StringBuilder();
            
            if (request.EventGroupId != null && request.EventGroupId > 0)
            {
                join.Append(" and a.eventGroupId=@EventGroupId ");
            }
            if (request.RoomStatus != null && request.RoomStatus > 0)
            {
                join.Append(" and a.RoomStatus=@RoomStatus ");
            }
            var sql = string.Empty;
            sql = $@"select a.*,b.completename MemberName,c.name EventGroupName 
                            from t_event_room a
                            left join t_member_player b on a.memberId=b.memberId
                            left join t_event_group c on a.eventgroupId=c.id
                            where a.eventId=@EventId and a.isdelete=0 {join.ToString()}  order by a.eventgroupId desc ,a.roomStatus asc ";
            int totalCount = 0;
            list = _dbContext.Page<EventRoomResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
            request.Records = totalCount;

            return list;

        }
    }
}
