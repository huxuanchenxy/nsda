using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using nsda.Services.Contract.member;

namespace nsda.unittest
{
    [TestClass]
    public class MemberPointsServiceTest:BaseTest
    {
        private IMemberPointsService service = AutofacContainer.Resolve<IMemberPointsService>();

        [TestMethod]
        public void PointsDetail()
        {
            service.Detail(1);
        }

        [TestMethod]
        public void PlayerPointsRecord()
        {
            service.PlayerPointsRecord(new Model.dto.request.PlayerPointsRecordQueryRequest {
                 MemberId=1,
                 PageIndex=1,
                 PageSize=10
            });
        }
    }
}
