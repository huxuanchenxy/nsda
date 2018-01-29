using nsda.Repository;
using nsda.Services.admin;
using nsda.Services.Contract.eventmanage;
using nsda.Services.Contract.member;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Utilities;
using nsda.Models;
using nsda.Model.enums;

namespace nsda.Services.Implement.eventmanage
{
    public class EventRuleService : IEventRuleService
    {
        IDBContext _dbContext;
        IDataRepository _dataRepository;
        IMemberOperLogService _memberOperLogService;
        ISysOperLogService _sysOperLogService;
        public EventRuleService(IDBContext dbContext, IDataRepository dataRepository, IMemberOperLogService memberOperLogService, ISysOperLogService sysOperLogService)
        {
            _dbContext = dbContext;
            _dataRepository = dataRepository;
            _memberOperLogService = memberOperLogService;
            _sysOperLogService = sysOperLogService;
        }
        //编辑循环赛规则
        public bool CyclingRaceRule(CyclingRaceRuleRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.BeginTransaction();
                foreach (var item in request.Teamscoringrule)
                {
                    var sql = $"update t_event_teamscoringrule set teamScoringRules={item.TeamScoringRules},updatetime={DateTime.Now} where id={item.Id}";
                    _dbContext.Execute(sql);
                }

                foreach (var item in request.Scoringrule)
                {
                    var sql = $"update t_event_playerscoringrule set scoringRules={item.ScoringRules},updatetime={DateTime.Now} where id={item.Id}";
                    _dbContext.Execute(sql);
                }

                foreach (var item in request.Avoidrule)
                {
                    var sql = $"update t_event_cycling_avoidrule set avoidRules={item.AvoidRules},updatetime={DateTime.Now} where id={item.Id}";
                    _dbContext.Execute(sql);
                }

                foreach (var item in request.RefereeAvoidrule)
                {
                    var sql = $"update t_event_refereeavoidrule set refereeAvoidRules={item.RefereeAvoidRules},updatetime={DateTime.Now} where id={item.Id}";
                    _dbContext.Execute(sql);
                }
                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRuleService.CyclingRaceRule", ex);
            }
            return flag;
        }
        //编辑淘汰赛规则
        public bool KnockoutRule(KnockoutRuleRequest request, out string msg)
        {
            bool flag = false;
            msg = string.Empty;
            try
            {
                _dbContext.BeginTransaction();
                foreach (var item in request.RefereeAvoidrule)
                {
                    var sql = $"update t_event_refereeavoidrule set refereeAvoidRules={item.RefereeAvoidRules},updatetime={DateTime.Now} where id={item.Id}";
                    _dbContext.Execute(sql);
                }
                _dbContext.CommitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                flag = false;
                msg = "服务异常";
                LogUtils.LogError("EventRuleService.KnockoutRule", ex);
            }
            return flag;
        }
        //淘汰赛规则详情
        public KnockoutRuleResponse KnockoutRuleDetail(int eventId)
        {
            KnockoutRuleResponse response = new KnockoutRuleResponse();
            try
            {
                //淘汰赛裁判规避原则
                var list = _dbContext.Select<t_event_refereeavoidrule>(c => c.eventId == eventId && c.objEventType == ObjEventTypeEm.淘汰赛).OrderBy(c => c.viewindex).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        response.RefereeAvoidrule.Add(new RefereeAvoidruleResponse
                        {
                            EventId = item.eventId,
                            Id = item.id,
                            ViewIndex = item.viewindex,
                            RefereeAvoidRules = item.refereeAvoidRules
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventRuleService.KnockoutRuleDetail", ex);
            }
            return response;
        }
        //循环赛规则详情
        public CyclingRaceRuleResponse CyclingRaceRuleDetail(int eventId)
        {
            CyclingRaceRuleResponse response = new CyclingRaceRuleResponse();
            try
            {
                //裁判规避原则
                var list = _dbContext.Select<t_event_refereeavoidrule>(c => c.eventId == eventId && c.objEventType == ObjEventTypeEm.循环赛).OrderBy(c => c.viewindex).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        response.RefereeAvoidrule.Add(new RefereeAvoidruleResponse
                        {
                            EventId = item.eventId,
                            Id = item.id,
                            ViewIndex = item.viewindex,
                            RefereeAvoidRules = item.refereeAvoidRules
                        });
                    }
                }
                //对垒规避原则
                var avoidrule = _dbContext.Select<t_event_cycling_avoidrule>(c => c.eventId == eventId).OrderBy(c => c.viewindex).ToList();
                if (avoidrule != null && avoidrule.Count > 0)
                {
                    foreach (var item in avoidrule)
                    {
                        response.Avoidrule.Add(new AvoidruleResponse
                        {
                            EventId = item.eventId,
                            Id = item.id,
                            ViewIndex = item.viewindex,
                            AvoidRules=item.avoidRules
                        });
                    }
                }
                //队伍排名参考数值
                var teamscoringrule = _dbContext.Select<t_event_teamscoringrule>(c => c.eventId == eventId).OrderBy(c => c.viewindex).ToList();
                if (teamscoringrule != null && teamscoringrule.Count > 0)
                {
                    foreach (var item in teamscoringrule)
                    {
                        response.Teamscoringrule.Add(new TeamscoringruleResponse
                        {
                            EventId = item.eventId,
                            Id = item.id,
                            ViewIndex = item.viewindex,
                            TeamScoringRules=item.teamScoringRules
                        });
                    }
                }
                //选手排名参考数值
                var playerscoringrule = _dbContext.Select<t_event_playerscoringrule>(c => c.eventId == eventId).OrderBy(c => c.viewindex).ToList();
                if (playerscoringrule != null && playerscoringrule.Count > 0)
                {
                    foreach (var item in playerscoringrule)
                    {
                        response.Scoringrule.Add(new ScoringruleResponse
                        {
                            EventId = item.eventId,
                            Id = item.id,
                            ViewIndex = item.viewindex,
                            ScoringRules = item.scoringRules
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError("EventRuleService.CyclingRaceRuleDetail", ex);
            }
            return response;
        }
    }
}
