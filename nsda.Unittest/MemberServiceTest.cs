using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;
using nsda.Utilities;
using nsda.Model.dto;
using nsda.Model.dto.response;
using System.Xml;
using nsda.Services.Contract.member;
using nsda.Services.admin;

namespace nsda.unittest
{
    /// <summary>
    /// 会员管理测试
    /// </summary>
    [TestClass]
    public class MemberServiceTest:BaseTest
    {
        private IMemberService service = AutofacContainer.Resolve<IMemberService>();
        [TestMethod]
        public void MemberList()
        {
            var request = new Model.dto.request.MemberQueryRequest
            {
                PageIndex = 1,
                PageSize = 10,
                MemberStatus = 0
            };
            var list = service.List(request);
            var res = new ResultDto<MemberResponse>
            {
                page = 1,
                total = request.Total,
                records = request.Records,
                rows = list
            };

            Console.WriteLine(JsonUtils.Serialize(res));
        }
    }
}
