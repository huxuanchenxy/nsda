using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.Contract.admin;
using Autofac;

namespace nsda.unittest
{
    [TestClass]
    public class PayCallBackServiceTest: BaseTest
    {
        private IPayCallBackService service = AutofacContainer.Resolve<IPayCallBackService>();
        [TestMethod]
        public void Callback()
        {
            service.Callback(1, "testpaylog");
        }
    }
}
