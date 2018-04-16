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
    public class EventPlayerSignUpRepo : IEventPlayerSignUpRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public EventPlayerSignUpRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EventPlayerSignUpListResponse> EventPlayerList(EventPlayerSignUpQueryRequest request)
        {
            List<EventPlayerSignUpListResponse> list = new List<EventPlayerSignUpListResponse>();

            StringBuilder join = new StringBuilder();
            if (request.KeyValue.IsNotEmpty())
            {
                request.KeyValue = $"%{request.KeyValue}%";
                join.Append(" and (b.code like @KeyValue or b.completename like @KeyValue or a.groupnum like @KeyValue)");
            }
            if (request.EventGroupId != null && request.EventGroupId > 0)
            {
                join.Append(" and a.eventGroupId=@EventGroupId");
            }
            if (request.SignUpStatus != null && request.SignUpStatus > 0)
            {
                join.Append(" and a.signUpStatus=@SignUpStatus");
            }
            var sql = $@"SELECT
	                        a.*, b. CODE MemberCode,
	                        b.completename MemberName,
	                        b.grade,
	                        b.gender,
	                        b.contactmobile,
	                        d. NAME EventGroupName,
	                        j.chinessname SchoolName,
	                        j. NAME CityName
                        FROM
	                        t_event_player_signup a
                        LEFT JOIN t_member_player b ON a.memberId = b.memberId
                        LEFT JOIN t_event c ON a.eventId = c.id
                        LEFT JOIN t_event_group d ON a.eventGroupId = d.id
                        LEFT JOIN (
	                        SELECT
		                        memberid,
		                        chinessname,
		                        NAME
	                        FROM
		                        (
			                        SELECT
				                        e.*
			                        FROM
				                        (
					                        SELECT
						                        *
					                        FROM
						                        t_player_edu
					                        ORDER BY
						                        enddate DESC
				                        ) e
			                        GROUP BY
				                        e.memberid
		                        ) f
	                        LEFT JOIN t_sys_school g ON g.id = f.schoolId
	                        LEFT JOIN t_sys_city h ON g.cityId = h.id
                        ) j ON j.memberid = b.memberId
                        WHERE
	                        c.memberId = @MemberId
                        AND a.eventId = @EventId {join.ToString()}
                        ORDER BY
	                        a.groupnum DESC 
                                                 ";

            int totalCount = 0;
            list = _dbContext.Page<EventPlayerSignUpListResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
            request.Records = totalCount;

            return list;

        }


    }
}
