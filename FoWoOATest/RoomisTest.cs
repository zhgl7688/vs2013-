using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoWoOATest
{
    [TestClass]
    public class RoomisTest
    {
        [TestMethod]
        public void RoomisSendStepTest()
        {
            string instance = "615ED859-0E39-453C-BA62-6653C743819D";
          new WebForm.Common.Meet().Roomis(instance, WebForm.Common.RoomisOperation.put_step);
            //  WebForm.Common.Meet.put1("72", "api/booking/events/{0}/approve","{ \"approver\":\"20121102\",\"status\":\"PENDING\",\"remarks\":\"待处理\"}");
        }
        [TestMethod]
        public void RoosTest()
        {
            string instance = "1eb6d23d-71a0-46e0-8507-a35660a1f51d";
            new WebForm.Common.Meet().Roomis(instance, WebForm.Common.RoomisOperation.put_approve);

        }
        [TestMethod]
        public void CheckBackTest()
        {
            FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType type = FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Back;
                Guid stepID=Guid.Parse("3DAF19F5-CE5E-4773-A783-581500722498");
                Guid stepID1=Guid.Parse("72578AB0-B803-4F0B-B0C0-1FAF3C99EA7E");
                Guid stepID2=Guid.Parse("B1F08F44-4692-4307-82FA-32C6026201A3");
                Guid stepID3=Guid.Parse("88B44E40-E9EB-44F9-9F2B-18B0AAE70A5A");
              
            Assert.IsTrue( WebForm.Common.Tools.CheckBack(type,stepID));
            Assert.IsTrue( WebForm.Common.Tools.CheckBack(type,stepID1));
            Assert.IsTrue( WebForm.Common.Tools.CheckBack(type,stepID2));
            Assert.IsTrue( WebForm.Common.Tools.CheckBack(type,stepID3));
           
               
        }
    }
}
