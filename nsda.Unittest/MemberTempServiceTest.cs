using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using nsda.Services.Contract.member;

namespace nsda.unittest
{
    [TestClass]
    public class MemberTempServiceTest : BaseTest
    {
        private IMemberTempService service = AutofacContainer.Resolve<IMemberTempService>();

        [TestMethod]
        public void InsertTempPlayer()
        {
            string msg = string.Empty;
            service.InsertTempPlayer(new System.Collections.Generic.List<Model.dto.request.TempPlayerRequest> {
                new Model.dto.request.TempPlayerRequest {
                     ContactMobile="18701729689",
                       EventGroupId=3,
                       EventId=2,
                         Name="上官",

                               SchoolId=1


                },
                new Model.dto.request.TempPlayerRequest {
                     ContactMobile="1870172968219",
                       EventGroupId=3,
                       EventId=2,
                         Name="上官1",

                               SchoolId=1

                }
            }, 12, out msg);
        }

        [TestMethod]
        public void BindTempPlayer()
        {
            string msg = string.Empty;
            service.BindTempPlayer(new Model.dto.request.BindTempPlayerRequest
            {
                ContactMobile = "18701729689",
                GroupNum = "nsda1000002",
                MemberId = 13
            }, out msg);
        }

        [TestMethod]
        public void Callback()
        {
            service.Callback(16, 3);
        }

        [TestMethod]
        public void InsertTempReferee()
        {
            string msg = string.Empty;
            service.InsertTempReferee(new Model.dto.request.TempRefereeRequest
            {
                ContactMobile = "18701729689",
                EventId = 2,
                GroupId = 3,
                Name = "上官"
            }, 12, out msg);
        }

        [TestMethod]
        public void BindTempReferee()
        {
            string msg = string.Empty;
            service.BindTempReferee(new Model.dto.request.BindTempRefereeRequest
            {
                MemberId = 10,
                ContactMobile = "18701729689",
                TempRefereeNum = "nsda1000001"
            }, out msg);

        }

        [TestMethod]
        public void ListPlayer()
        {
            service.ListPlayer(new Model.dto.request.TempMemberQueryRequest
            {
                PageIndex = 1,
                PageSize = 10
            });
        }
        [TestMethod]
        public void ListReferee()
        {
            service.ListReferee(new Model.dto.request.TempMemberQueryRequest
            {
                PageIndex = 1,
                PageSize = 10
            });
        }

    }
}
