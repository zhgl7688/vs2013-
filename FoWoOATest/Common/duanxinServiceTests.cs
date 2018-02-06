using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebForm.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebForm.Common.Tests
{
    [TestClass()]
    public class duanxinServiceTests
    {
        [TestMethod()]
        public void smsSendTest()
        {
            DuanxinService ds = new DuanxinService();
            ds.smsSend("13407522936", "短信测试");
            //Assert.Fail();
        }
    }
}