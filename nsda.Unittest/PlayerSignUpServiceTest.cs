using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;

namespace nsda.unittest
{
    /// <summary>
    /// 选手报名管理
    /// </summary>
    [TestClass]
    public class PlayerSignUpServiceTest:BaseTest
    {
        private IMemberService service = AutofacContainer.Resolve<IMemberService>();
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
