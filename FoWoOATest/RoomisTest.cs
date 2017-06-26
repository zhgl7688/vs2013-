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
    }
}
