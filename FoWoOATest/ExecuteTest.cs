using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoWoOATest
{
    [TestClass]
    public class ExecuteTest
    {
        [TestMethod]
        public void TestbackEnd()
        {
            FoWoSoft.Data.Model.WorkFlowExecute.Execute execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute
            {
                InstanceID = "e141b428-e586-41b0-8a5b-a4d5843e65d0",
                ExecuteType  = FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Back,
              StepID  = Guid.Parse("3DAF19F5-CE5E-4773-A783-581500722498"),

            };
            new WebForm.Platform.WorkFlowRun.Execute().backEnd(execute);

        }
    }
}
