using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoWoOATest
{
    [TestClass]
    public class MeetTest
    {
        [TestMethod]
        public void TestRoomisCompleted()
        {
            FoWoSoft.Data.Model.WorkFlowExecute.Execute execute=new FoWoSoft.Data.Model.WorkFlowExecute.Execute ();
            execute.InstanceID = "e7801f76-6879-4d9c-adf9-f2571ca66e8e";
            execute.ExecuteType = FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Completed;
            execute.Sender = new FoWoSoft.Data.Model.Users { Account = "20121102" };
            var result =new WebForm.Common.Meet().Roomis(execute);
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TestRoomisBack()
        {
            FoWoSoft.Data.Model.WorkFlowExecute.Execute execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();
            execute.InstanceID = "e7801f76-6879-4d9c-adf9-f2571ca66e8e";
            execute.StepID = Guid.Parse("3DAF19F5-CE5E-4773-A783-581500722498");
            execute.ExecuteType = FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Back;
            execute.Sender = new FoWoSoft.Data.Model.Users { Account = "20121102" };
            var result = new WebForm.Common.Meet().Roomis(execute);
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TestRoomisSubmit()
        {
            FoWoSoft.Data.Model.WorkFlowExecute.Execute execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();
            execute.InstanceID = "e4c91e62-e84a-4623-a266-67f4c394c1ed";
            execute.StepID = Guid.Parse("3DAF19F5-CE5E-4773-A783-581500722498");
            execute.ExecuteType = FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Submit;
            execute.Sender = new FoWoSoft.Data.Model.Users { Account = "20121102" };
            var result = new WebForm.Common.Meet().Roomis(execute);
            Assert.AreEqual(1, result);
        }
    }
}
