using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Model.dto.request;
using nsda.Models;
using nsda.Services.admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsda.unittest
{
    [TestClass]
    public class LeavingMsgTest:BaseTest
    {
        private ILeavingMsgService service = AutofacContainer.Resolve<ILeavingMsgService>();

        [TestMethod]
        public void Insert()
        {
            string msg = string.Empty;
            var request = new LeavingMsgRequest
            {
                Email = "test@163.com",
                Message = "test",
                Mobile = "182********",
                Name = "shangguanlei"
            };
            service.Insert(request, out msg);
        }

        [TestMethod]
        public void Process()
        {
            string msg = string.Empty;
            service.Process(1,1, out msg);
        }


        [TestMethod]
        public void Delete()
        {
            string msg = string.Empty;
            service.Delete(1,1, out msg);
        }

        [TestMethod]
        public void Detail()
        {
            service.Detail(1);
        }


        [TestMethod]
        public void List()
        {
            LeavingMsgQueryRequest request = new LeavingMsgQueryRequest {
                 PageIndex=1,
                 PageSize=10
            };
            var list=service.List(request);
        }
    }
}
