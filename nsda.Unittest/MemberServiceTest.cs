using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Services.member;
using Autofac;

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
        public void TestMethod1()
        {
        }
    }
}
