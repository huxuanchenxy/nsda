using nsda.Model.dto.request;
using nsda.Model.dto.response;
using nsda.Model.enums;
using nsda.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Services.Contract.eventmanage
{
    public interface IEventService: IDependency
    {
        //发起比赛
        bool Insert(EventRequest request, out string msg);
        //编辑赛事
        bool Edit(EventRequest request, out string msg);
        //修改组别信息
        bool EditGroup(EventGroupRequest request, out string msg);
        // 赛事详情
        EventResponse Detail(int id);
        //设定赛事级别
        bool SettingLevel(int id, EventLevelEm eventLevel, int sysUserId, out string msg);
        //审核赛事
        bool Check(int id, bool isAgree, int sysUserId, out string msg);
        //赛事列表查询
        List<PlayerOrRefereeEventResponse> PlayerOrRefereeEvent(PlayerOrRefereeEventQueryRequest request);
        List<EventResponse> EventList(EventQueryRequest request);
        List<EventResponse> AdminEventList(EventAdminQueryRequest request);
        //选手 裁判 通过赛事查询选择条件
        List<EventConditionResponse> EventCondition();
        //裁判注册时 可报名赛事
        List<EventSelectResponse> RefereeRegisterEvent();
        //赛事组别信息
        List<EventGroupResponse> SelectEventGroup(int eventId,int memberId);
        //修改赛事状态
        bool EditEventStatus(int eventId,EventStatusEm eventStatus,int memberId, out string msg);
        //赛事组别列表
        List<EventGroupResponse> ListEventGroup(int eventId, int memberId);
        // 赛事组别详情
        EventGroupResponse EventGroupDetail(int eventGroupId);
        List<string> EventDate(int eventId);
        List<string> EventYYYYDate(int eventId);
    }
}
