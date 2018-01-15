using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Model.dto.request;
using nsda.Services.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.unittest
{
    [TestClass]
    public class LoginLogServiceTest:BaseTest
    {
        private ILoginLogService service = AutofacContainer.Resolve<ILoginLogService>();

        [TestMethod]
        public void Insert()
        {
            var request = new LoginLogRequest {
                Account = "shangguanlei@163.com",
                DataType = Model.enums.DataTypeEm.选手,
                LoginResult = "OK"
            };
            service.Insert(request);
        }

        [TestMethod]
        public void List()
        {
            var request = new LoginLogQueryRequest
            {
                PageSize = 1,
                PageIndex = 1
            };
           var list= service.List(request);
           Assert.Equals(request.Records, 3);
        }
    }
}
