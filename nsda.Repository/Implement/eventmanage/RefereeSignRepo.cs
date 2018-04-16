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
using nsda.Models;
using nsda.Model.enums;

namespace nsda.Repository.Implement.eventmanage
{
    public class RefereeSignRepo : IRefereeSignRepo
    {
        IDBContext _dbContext;
        private static object lockObject = new object();
        public RefereeSignRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<t_event_sign> RefereeSignData(int eventId,string manMemberId)
        {
            List<t_event_sign> resp = new List<t_event_sign>();
            var sql = $@" SELECT
	                            a.*
                            FROM
	                            t_event_sign a
                            inner join t_event b on a.eventId = b.id
                            WHERE
	                            a.eventId = {eventId}
                            AND a.eventSignType = 2
                            AND a.isdelete = 0
                            and b.memberId = {manMemberId}
                           ";
            var list = _dbContext.Query<t_event_sign>(sql).ToList();
            return list;
        }


    }
}
