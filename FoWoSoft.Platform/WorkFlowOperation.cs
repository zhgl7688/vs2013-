using FoWoSoft.Data.Model;
using FoWoSoft.Data.Model.WorkFlowExecute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Platform
{
   public class WorkFlowOperation
    {
        FoWoSoft.Data.Model.WorkFlowExecute.Execute execute;
        FoWoSoft.Data.Model.WorkFlowInstalled wfInstalled;
        WorkFlowCustomEventParams eventParams = new WorkFlowCustomEventParams();
        WorkFlowOperationData data;
        LitJson.JsonData opationJSON;
        IEnumerable<FoWoSoft.Data.Model.WorkFlowInstalledSub.Step> currentStep;
        public FoWoSoft.Data.Model.WorkFlowExecute.Result result { get; set; }

        Organize borganize = new Organize();
        WorkFlow bworkFlow = new WorkFlow();
        WorkFlowTask btask = new WorkFlowTask();
        WorkFlowDelegation bworkFlowDelegation = new WorkFlowDelegation();
        Users busers = new Users();



        public string Msg { get; set; }
        public bool Check { get; set; }
        public WorkFlowOperation(WorkFlowOperationData data)
        {
            this.data = data;
            wfInstalledcheck();
            opationJSON = LitJson.JsonMapper.ToObject(data.params1);
            CreateExecute();
            WorkFlowCustomEventParamsSet();
            jump();
        }
        private void jump()
        {
            switch (execute.ExecuteType)
            {
                case EnumType.ExecuteType.Submit:
                    submit();
                    break;
                case EnumType.ExecuteType.Save:
                    save();
                    break;
                case EnumType.ExecuteType.Back:
                    back();
                    break;
                case EnumType.ExecuteType.Completed:
                    completed();
                    break;
                case EnumType.ExecuteType.Redirect:
                    redirect();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">返回的信息</param>
        /// <returns>msg默认null,通过为true</returns>
        private void wfInstalledcheck()
        {
            wfInstalled = bworkFlow.GetWorkFlowRunModel(data.flowid);
            if (wfInstalled == null)
            {
                Msg = "未找到流程运行时实体,请确认流程是否已安装!";
                Check = false;
                currentStep = wfInstalled.Steps.Where(p => p.ID == data.stepid.ToGuid());
            }
            Check = true;
        }
        public void submit()
        {
            if (opationJSON["steps"].IsArray)
            {
                foreach (LitJson.JsonData step in opationJSON["steps"])
                {
                    Guid gid;
                    if (step["id"].ToString().IsGuid(out gid)) execute.Steps.Add(gid, borganize.GetAllUsers(step["member"].ToString()));
                }
            }
            submitBeforeEvent();
            //处理委托
            execDelegation();
            //处理流程
          //  result = btask.Execute(execute, btask.executeSubmit);


            submitAfterEvent();

            WorkFlowArchives();
        }

        private void submitAfterEvent()
        {
            foreach (var step in currentStep)
            {
                //步骤提交后事件
                if (!step.Event.SubmitAfter.IsNullOrEmpty())
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.SubmitAfter.Trim(), eventParams);
                    Msg += (string.Format("执行步骤提交后事件：({0}) 返回值：{1}<br/>", step.Event.SubmitAfter.Trim(), obj.ToString()));
                }

            }
        }




        public void save()
        {
            GetInstanceID();
            // setLogs(data.params1, wfInstalled.Name, reslut);
            //处理委托
            execDelegation();
            //处理流程
           // var reslut = btask.Execute(execute, btask.executeSave);
         
        }
        public void back()
        {

            if (opationJSON["steps"].IsArray)
            {
                foreach (LitJson.JsonData step in opationJSON["steps"])
                {
                    Guid gid;
                    if (step["id"].ToString().IsGuid(out gid)) execute.Steps.Add(gid, new List<FoWoSoft.Data.Model.Users>());
                }
            }
            foreach (var step in currentStep)
            {
                //步骤退回前事件
                if (!step.Event.BackBefore.IsNullOrEmpty())
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.BackBefore.Trim(), eventParams);
                    Msg += string.Format("执行步骤退回前事件：({0}) 返回值：{1}<br/>", step.Event.BackBefore.Trim(), obj.ToString());
                }
            }
            //处理委托
            execDelegation();
            //处理流程
           // var reslut = btask.Execute(execute, btask.executeBack);
          
            foreach (var step in currentStep)
            {
                //步骤退回后事件
                if (!step.Event.BackAfter.IsNullOrEmpty())
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.BackAfter.Trim(), eventParams);
                    Msg += (string.Format("执行步骤退回后事件：({0}) 返回值：{1}<br/>", step.Event.BackAfter.Trim(), obj.ToString()));
                }
            }

        }
        public void completed()
        {
            GetInstanceID();
            submitBeforeEvent();
            //处理委托
            execDelegation();
            //处理流程
           // var reslut = btask.Execute(execute, btask.executeSubmit);
            

            submitAfterEvent();


            WorkFlowArchives();

        }
        public void redirect()
        {
            if (opationJSON["steps"].IsArray)
            {
                foreach (LitJson.JsonData step in opationJSON["steps"])
                {
                    execute.Steps.Add(Guid.Empty, borganize.GetAllUsers(step["member"].ToString()));
                }
            }
            submitBeforeEvent();
            //处理委托
            execDelegation();
            //处理流程
          //  var reslut = btask.Execute(execute, btask.executeRedirect);
           



        }
        private void submitBeforeEvent()
        {
            foreach (var step in currentStep)
            {
                //步骤提交前事件
                if (!step.Event.SubmitBefore.IsNullOrEmpty())
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.SubmitBefore.Trim(), eventParams);
                    Msg = string.Format("执行步骤提交前事件：({0}) 返回值：{1}<br/>", step.Event.SubmitBefore.Trim(), obj.ToString());
                }
            }

        }
        private void setMsg(Result reslut)
        {

        }
        private void execDelegation()
        {
            foreach (var executeStep in execute.Steps)
            {
                for (int i = 0; i < executeStep.Value.Count; i++)
                {
                    Guid newUserID = bworkFlowDelegation.GetFlowDelegationByUserID(execute.FlowID, executeStep.Value[i].ID);
                    if (newUserID != Guid.Empty && newUserID != executeStep.Value[i].ID)
                    {
                        executeStep.Value[i] = busers.Get(newUserID);
                    }
                }
            }
        }

        private void GetInstanceID()
        {
            if ("1" != data.isSystemDetermine)
            {
                string instanceid = bworkFlow.SaveFromData(data.instanceid, eventParams);
                if (execute.InstanceID.IsNullOrEmpty())
                {
                    execute.InstanceID = eventParams.InstanceID = instanceid;
                }
            }
        }

        private static void setLogs(string params1, string name, Result reslut)
        {
            string logContent = string.Format("处理参数：{0}<br/>处理结果：{1}<br/>调试信息：{2}<br/>返回信息：{3}",
                params1,
                reslut.IsSuccess ? "成功" : "失败",
                reslut.DebugMessages,
                reslut.Messages
                );

            FoWoSoft.Platform.Log.Add(string.Format("处理了流程({0})", name), logContent, FoWoSoft.Platform.Log.Types.流程相关);
        }
        public void WorkFlowCustomEventParamsSet()
        {
            eventParams = new FoWoSoft.Data.Model.WorkFlowCustomEventParams()
            {
                FlowID = execute.FlowID,
                GroupID = execute.GroupID,
                StepID = execute.StepID,
                TaskID = execute.TaskID,
                InstanceID = execute.InstanceID,
            };
        }
        private void CreateExecute()
        {
            var execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();
            execute.Comment = data.comment.IsNullOrEmpty() ? "" : data.comment.Trim();
            execute.FlowID = data.flowid.ToGuid();
            execute.GroupID = data.GroupID.ToGuid();
            execute.InstanceID = data.instanceid;
            execute.IsSign = "1" == data.IsSign;
            execute.Note = "";
            execute.Sender = data.CurrentUser;
            execute.StepID = data.stepid.IsGuid() ? data.stepid.ToGuid() : wfInstalled.FirstStepID;
            execute.TaskID = data.TaskID.ToGuid();
            execute.Title = data.title ?? "";
            execute.ExecuteType = (EnumType.ExecuteType)Enum.Parse(typeof(EnumType.ExecuteType), opationJSON["type"].ToString(), true);
            execute.Steps = new Dictionary<Guid, List<Data.Model.Users>>();

        }


        //归档
        public void WorkFlowArchives()
        {

            var currentsteps = wfInstalled.Steps.Where(p => p.ID == execute.StepID);
            if (currentsteps.Count() > 0 && currentsteps.First().Archives == 1)
            {
                string flowName, stepName;
                stepName = bworkFlow.GetStepName(execute.StepID, execute.FlowID, out flowName, true);
                string archivesContents = new FoWoSoft.Platform.WorkFlowForm().GetArchivesString(data.formHtml, data.commentHtml);
                FoWoSoft.Data.Model.WorkFlowArchives wfr = new FoWoSoft.Data.Model.WorkFlowArchives();
                wfr.Comments = data.commentHtml;
                wfr.Contents = archivesContents;
                wfr.FlowID = execute.FlowID;
                wfr.FlowName = flowName;
                wfr.GroupID = execute.GroupID;
                wfr.ID = Guid.NewGuid();
                wfr.InstanceID = execute.InstanceID;
                wfr.StepID = execute.StepID;
                wfr.StepName = stepName;
                wfr.Title = data.title.IsNullOrEmpty() ? flowName + "-" + stepName : data.title;
                wfr.TaskID = execute.TaskID;
                wfr.WriteTime = FoWoSoft.Utility.DateTimeNew.Now;
                new FoWoSoft.Platform.WorkFlowArchives().Add(wfr);
            }

        }

    }
    public class WorkFlowOperationData
    {

        public string params1 { get; set; }

        public string instanceid { get; set; }

        public string flowid { get; set; }

        public string comment { get; set; }

        public string stepid { get; set; }

        public string title { get; set; }

        public string isSystemDetermine { get; set; }

        public string commentHtml { get; set; }

        public string formHtml { get; set; }

        public string GroupID { get; set; }

        public string IsSign { get; set; }

        public string TaskID { get; set; }

        public Data.Model.Users CurrentUser { get; set; }
    }
}
