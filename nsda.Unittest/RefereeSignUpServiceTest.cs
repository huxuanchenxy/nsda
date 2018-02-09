using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using nsda.Services.Contract.referee;

namespace nsda.unittest
{
    [TestClass]
    public class RefereeSignUpServiceTest:BaseTest
    {
        private IRefereeSignUpService service = AutofacContainer.Resolve<IRefereeSignUpService>();
        [TestMethod]
        public void Apply()
        {
            string msg = string.Empty;
            service.Apply(2, 11, out msg);
        }

        [TestMethod]
        public void Check()
        {
            string msg = string.Empty;
            service.Check(2, Model.enums.CheckRefereeEnum.通过, 11, out msg);
        }
    }
}
