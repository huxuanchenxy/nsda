using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;
using nsda.Services.Contract.member;

namespace nsda.unittest
{
    /// <summary>
    /// 选手报名管理
    /// </summary>
    [TestClass]
    public class PlayerSignUpServiceTest:BaseTest
    {
        private IPlayerSignUpService service = AutofacContainer.Resolve<IPlayerSignUpService>();

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
