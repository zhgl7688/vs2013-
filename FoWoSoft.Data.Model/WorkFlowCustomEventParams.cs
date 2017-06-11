using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Model
{
    /// <summary>
    /// 调用流程事件时的参数实体
    /// </summary>
    [Serializable]
    public struct WorkFlowCustomEventParams
    {

        public Guid FlowID { get; set; }

        public Guid StepID { get; set; }

        public Guid GroupID { get; set; }

        public Guid TaskID { get; set; }

        public string InstanceID { get; set; }
        public WorkFlowCustomEventParams set(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
        {
            FlowID = execute.FlowID;
            GroupID = execute.GroupID;
            StepID = execute.StepID;
            TaskID = execute.TaskID;
            InstanceID = execute.InstanceID;
            return this;
        }

        
    }
}
