using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebForm.Tests
{
    [TestClass()]
    public class EduWebServiceTests
    {
        [TestMethod()]
        public void GetUseinfoTest()
        {
            var webservice = new EduWebService();
            //var id = "20121102";
            var id = "20171001";
           // var result = webservice.GetUseinfo(id);
            var mobile = webservice.GetMobile(id);
            //Assert.AreEqual("", result.Rows[0]["mobile"]);
            Assert.IsNull(mobile);
            //Assert.Fail();
        }

        [TestMethod()]
        public void SetUseinfoTest()
        {
            var webservice = new EduWebService();
            var id = "20121102";
            var mobile = "";
            var result = webservice.SetUseinfo(id,mobile);
            Assert.AreEqual("添加成功", result.Rows[0][0]);
        }
    }
}