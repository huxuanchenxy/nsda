using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using nsda.Services.Contract.member;

namespace nsda.unittest
{
    [TestClass]
    public class MemberTempServiceTest:BaseTest
    {
        private IMemberTempService service = AutofacContainer.Resolve<IMemberTempService>();

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
