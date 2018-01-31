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
                CoachPoints = 0,
                PlayerPoints=0,
                RefereePoints=0
            };
            try
            {
                var detail = _dbContext.Select<t_member_points>(c=>c.memberId==memberId).FirstOrDefault();
                if (detail != null)
                {
                    response.RefereePoints = detail.refereePoints;
                    response.PlayerPoints = detail.playerPoints;
                    response.CoachPoints = detail.coachPoints;
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
                    sqljoin.Append(" and a.createtime>=@StartDate ");
                }
                if (request.EndDate != null)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1).AddSeconds(-1);
                    sqljoin.Append(" and a.createtime<=@EndDate ");
                }
                var sql= $@"select b.name EventName,b.code EventCode,a.points,a.Id
                            from t_member_points_record a
                            inner join t_event b on a.eventId=b.id
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
                var sqlTotalPoints = $@"select IFNULL(sum(points),0) from t_member_points_record where  memberId=@MemberId and isdelete=0 {sqljoin.ToString()}";
                totalPoints = _dbContext.ExecuteScalar(sqlTotalPoints, request).ToObjDecimal();
                if (totalPoints > 0)//有积分再查询列表
                {
                    var sql=$@"select b.starteventtime StartDate,b.endeventtime EndDate,b.name EventName, 
                            a.points,c.name ProvinceName,d.name CityName,a.Id,c.name CountryName
                            from t_member_points_record a
                            inner join t_event b on a.eventId=b.id
                            left join  t_sys_country c on b.countryId=c.id
                            left join t_sys_province d on b.provinceId=d.id
                            left join t_sys_city e on b.cityId = e.id
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
            //参照赛果
            List<PlayerPointsRecordDetailResponse> list = new List<PlayerPointsRecordDetailResponse>();
            try
            {
                var record = _dbContext.Get<t_member_points_record>(recordId);
                if (record != null&& memberId==record.memberId)
                {
                    //根据赛事 组别 选手id 查询此次比赛获奖次数
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("MemberPointsService.PointsRecordDetail", ex);
            }
            return list;
        }
    }
}
