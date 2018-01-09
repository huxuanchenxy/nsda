using nsda.Repository;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.dto.request;

namespace nsda.Services.Implement.member
{
    public class MemberPointsService: IMemberPointsService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        public MemberPointsService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
        }

        public MemberPointsModelResponse Detail(int memberId)
        {
            MemberPointsModelResponse response = new MemberPointsModelResponse {
                MemberId=memberId,
                Points=0,
                EventPoints=0,
                ServicePoints=0
            };
            try
            {
                var detail = _dbContext.Select<t_memberpoints>(c=>c.memberId==memberId).FirstOrDefault();
                if (detail != null)
                {
                    response.Points = detail.points;
                    response.EventPoints = detail.eventPoints;
                    response.ServicePoints = detail.servicePoints;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.Detail", ex);
            }
            return response;
        }

        public PagedList<PlayerPointsResponse> PlayerPoints(PlayerPointsQueryRequest request, out decimal totalPoints)
        {
            totalPoints = 0m;
            PagedList<PlayerPointsResponse> list = new PagedList<PlayerPointsResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PlayerPoints", ex);
            }
            return list;
        }

        public List<PlayerPointsRecordResponse> PointsRecordDetail(int recordId)
        {
            List<PlayerPointsRecordResponse> list = new List<PlayerPointsRecordResponse>();
            try
            {
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PointsRecordDetail", ex);
            }
            return list;
        }
    }
}
