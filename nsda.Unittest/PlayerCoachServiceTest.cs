using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;

namespace nsda.unittest
{
    [TestClass]
    public class PlayerCoachServiceTest:BaseTest
    {
        private IPlayerCoachService service = AutofacContainer.Resolve<IPlayerCoachService>();
        //绑定学生 或 教练
        [TestMethod]
        public void Insert()
        {
            string msg = string.Empty;
            service.Insert(new Model.dto.request.PlayerCoachRequest {
                 IsCoach=false,
                 IsPositive=true,
                 MemberId= 13,
                 ObjMemberId= 10,
                 StartDate="2018-01"
            },out msg);
        }

        [TestMethod]
        public void Edit()
        {
            string msg = string.Empty;
            service.Edit(new Model.dto.request.PlayerCoachRequest
            {
                IsCoach = false,
                IsPositive = true,
                MemberId = 13,
                ObjMemberId = 10,
                StartDate = "2017-01",
                Id=4
            }, out msg);
        }

        [TestMethod]
        public void Delete()
        {
            string msg = string.Empty;
            service.Delete(4,13, out msg);
        }

        [TestMethod]
        public void Check()
        {
            string msg = string.Empty;
            service.Check(4, true, 10, out msg);
        }

        [TestMethod]
        public void Player_CoachDetail()
        {
            service.Player_CoachDetail(13);
        }

        [TestMethod]
        public void Coach_PlayerList()
        {
            service.Coach_PlayerList(new Model.dto.request.PlayerCoachQueryRequest {
                 MemberId=3,
                  PageIndex=1,
                  PageSize=10
            });
        }

        [TestMethod]
        public void Player_CoachList()
        {
            service.Player_CoachList(new Model.dto.request.PlayerCoachQueryRequest
            {
                MemberId = 13,
                 PageIndex=1,
                 PageSize=10
            });
        }
    }
}
