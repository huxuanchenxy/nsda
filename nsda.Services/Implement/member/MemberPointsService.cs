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
using Dapper;

namespace nsda.Services.Implement.member
{
    /// <summary>
    /// 会员积分管理
    /// </summary>
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
        //会员积分
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
        //选手积分列表
        public List<PlayerPointsRecordResponse> PlayerPointsRecord(PlayerPointsRecordQueryRequest request)
        {
            List<PlayerPointsRecordResponse> list = new List<PlayerPointsRecordResponse>();
            try
            {
                StringBuilder sqljoin = new StringBuilder();
                if (request.StartDate != null)
                {
                    sqljoin.Append(" and createtime>=@StartDate ");
                }
                if (request.EndDate != null)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    sqljoin.Append(" and createtime<=@EndDate ");
                }
                var sql= $@"select b.starteventtime StartDate,b.endeventtime EndDate,b.name EventName, 
                            a.points,c.name ProvinceName,d.name CityName,a.Id,c.name CountryName
                            from t_memberpointsrecord a
                            inner join t_event b on a.eventId=b.id
                            left join  t_country c on b.countryId=c.id
                            left join t_province d on b.provinceId=d.id
                            left join t_city e on b.cityId = e.id
                            where a.isdelete=0 and a.memberId=@memberId {sqljoin.ToString()}
                            order by a.createtime desc
                        ";
                int totalCount = 0;
                list = _dbContext.Page<PlayerPointsRecordResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                request.Records = totalCount;
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PlayerPointsRecord", ex);
            }
            return list;
        }

        public List<PlayerPointsRecordResponse> PlayerPointsRecord(PlayerPointsRecordQueryRequest request, out decimal totalPoints)
        {
            totalPoints = 0m;
            List<PlayerPointsRecordResponse> list = new List<PlayerPointsRecordResponse>();
            try
            {
                StringBuilder sqljoin = new StringBuilder();
                if (request.StartDate != null)
                {
                    sqljoin.Append(" and createtime>=@StartDate ");
                }
                if (request.EndDate != null)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    sqljoin.Append(" and createtime<=@EndDate ");
                }
                var sqlTotalPoints = $@"select IFNULL(sum(points),0) from t_memberpointsrecord where  memberId=@MemberId and isdelete=0 {sqljoin.ToString()}";
                totalPoints = _dbContext.ExecuteScalar(sqlTotalPoints, request).ToObjDecimal();
                if (totalPoints > 0)//有积分再查询列表
                {
                    var sql=$@"select b.starteventtime StartDate,b.endeventtime EndDate,b.name EventName, 
                            a.points,c.name ProvinceName,d.name CityName,a.Id,c.name CountryName
                            from t_memberpointsrecord a
                            inner join t_event b on a.eventId=b.id
                            left join  t_country c on b.countryId=c.id
                            left join t_province d on b.provinceId=d.id
                            left join t_city e on b.cityId = e.id
                            where a.isdelete=0 and a.memberId=@memberId {sqljoin.ToString()}
                            order by a.createtime desc
                            ";
                    int totalCount = 0;
                    list = _dbContext.Page<PlayerPointsRecordResponse>(sql, out totalCount, request.PageIndex, request.PageSize, request);
                    request.Records = totalCount;
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PlayerPointsRecord", ex);
            }
            return list;
        }
        //选手积分详情
        public List<PlayerPointsRecordDetailResponse> PointsRecordDetail(int recordId, int memberId)
        {
            List<PlayerPointsRecordDetailResponse> list = new List<PlayerPointsRecordDetailResponse>();
            try
            {
                var sql = $@"select a.points,a.objEventType,c.name EventName,d.name GroupName,a.remark,a.createtime 
                            from t_memberpointsrecordetail a
                            inner join t_memberpointsrecord b on a.recordId=b.id
                            inner join t_event c on c.id=b.eventId
                            inner join t_eventgroup d on b.groupId=d.id
                            where b.isdelete=0 and a.isdelete=0 and a.memberId={memberId} and a.recordId={recordId}";
                list = _dbContext.Query<PlayerPointsRecordDetailResponse>(sql).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PointsRecordDetail", ex);
            }
            return list;
        }
    }
}
