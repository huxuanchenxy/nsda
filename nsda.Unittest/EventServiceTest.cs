using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.Contract.eventmanage;
using Autofac;

namespace nsda.unittest
{
    [TestClass]
    public class EventServiceTest : BaseTest
    {
        private IEventService service = AutofacContainer.Resolve<IEventService>();

        [TestMethod]
        public void Insert()
        {
            string msg = string.Empty;
            service.Insert(new Model.dto.request.EventRequest
            {
                Address = "上海杨浦区",
                CityId = 1,
                CountryId = 1,
                IsInter = false,
                EndEventDate = DateTime.Now.AddDays(7),
                StartEventDate = DateTime.Now.AddDays(6),
                EndRefundDate = DateTime.Now,
                EndSignDate = DateTime.Now.AddDays(5),
                EventGroup = new System.Collections.Generic.List<Model.dto.request.EventGroupRequest> {
                    new Model.dto.request.EventGroupRequest {
                      TeamNumber=2,
                      Name="新手组" 
                    },
                    new Model.dto.request.EventGroupRequest {
                        TeamNumber=2,
                        Name="公开组"
                    }
                },
                EventType = Model.enums.EventTypeEm.辩论,
                EventTypeName = Model.enums.EventTypeNameEm.公共论坛式辩论,
                Maxnumber = 100,
                MemberId = 12,
                ProvinceId = 1,
                Name = "nsda新版测试",
                Remark = "ceshieryi",
                Signfee = 300
            }, out msg);
        }

        //赛事审核
        [TestMethod]
        public void Check()
        {
            string msg = string.Empty;
            service.Check(2, true,  1, out msg);
        }

        [TestMethod]
        public void SetLevel()
        {
            string msg = string.Empty;
            service.SettingLevel(2, Model.enums.EventLevelEm.D, 1, out msg);
        }
    }
}
