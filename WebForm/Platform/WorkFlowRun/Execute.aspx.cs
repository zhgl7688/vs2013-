using FoWoSoft.Data.Model;
using FoWoSoft.Data.Model.WorkFlowExecute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowRun
{
    public partial class Execute1 : System.Web.UI.Page
    {
        FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        FoWoSoft.Platform.WorkFlowTask btask = new FoWoSoft.Platform.WorkFlowTask();
        FoWoSoft.Platform.WorkFlowDelegation bworkFlowDelegation = new FoWoSoft.Platform.WorkFlowDelegation();
        FoWoSoft.Platform.Organize borganize = new FoWoSoft.Platform.Organize();
        FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
        protected void Page_Load(object sender, EventArgs e)
        {
            WebForm.Common.Tools.CheckLogin(false);
            string params1 = Request.Form["params"];
            if (params1.IsNullOrEmpty())
            {
                Response.Write("参数为空!");
                Response.End();
            }
            string instanceid = Request.QueryString["instanceid"] ?? Request.Form["instanceid"];

            string flowid = Request.QueryString["flowid"];
            var wfInstalled = bworkFlow.GetWorkFlowRunModel(flowid);
            if (wfInstalled == null)
            {
                Response.Write("未找到流程运行时实体,请确认流程是否已安装!");
                Response.End();
            }
            string comment = Request.Form["comment"];


            string stepid = Request.QueryString["stepid"];

            var opationJSON = LitJson.JsonMapper.ToObject(params1);

            //流程标题
            string title = Request.Form[Request.Form["Form_TitleField"]];
            var execute = CreateExecute(instanceid, flowid, wfInstalled, comment, stepid, opationJSON, title);
            //操作roomis审核
            if (execute.ExecuteType == EnumType.ExecuteType.Completed)
                new WebForm.Common.Meet().Roomis(execute.InstanceID, WebForm.Common.RoomisOperation.put_approve);

            var eventParams = WorkFlowCustomEventParamsSet(execute);


            //保存业务数据 "1" != Request.QueryString["isSystemDetermine"]:当前步骤流转类型如果是系统判断，则先保存数据，在这里就不需要保存数据了。
            if ((execute.ExecuteType == (EnumType.ExecuteType.Save | EnumType.ExecuteType.Completed)) ||
                "1" != Request.QueryString["isSystemDetermine"])
            {
                instanceid = bworkFlow.SaveFromData(instanceid, eventParams);
                if (execute.InstanceID.IsNullOrEmpty())
                {
                    execute.InstanceID = instanceid;
                    eventParams.InstanceID = instanceid;
                }
            }

            Response.Write("执行参数：" + params1 + "<br/>");

            var steps = wfInstalled.Steps.Where(p => p.ID == execute.StepID);
            eventParams = firstevent(execute.ExecuteType, eventParams, steps);//提交前，退回前事件处理

            //处理委托
            execDelegation(execute);
            //处理流程
            var reslut = btask.Execute(execute);
            Response.Write(string.Format("处理流程步骤结果：{0}<br/>", reslut.IsSuccess ? "成功" : "失败"));
            Response.Write(string.Format("调试信息：{0}", reslut.DebugMessages));
            setLogs(params1, wfInstalled.Name, reslut);

            eventParams = NextEvent(execute, eventParams, steps);//提交后，退回后事件处理

            //归档
            WorkFlowArchives(wfInstalled, title, execute);

            Response.Write("<script type=\"text/javascript\">alert('" + reslut.Messages + "');top.mainDialog.close();</script>");

            if (reslut.IsSuccess)
            {
               
                RoomisSend(execute, reslut);

                DisplayHtml(reslut);
            }

        }

        private void RoomisSend(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute, Result reslut)
        {
            // 操作roomis审核
            // if (execute.ExecuteType == EnumType.ExecuteType.Completed)
            //new WebForm.Common.Meet().Roomis(execute.StepID.ToString(), WebForm.Common.RoomisOperation.put_step);
            // new WebForm.Common.Meet().Roomis(execute.InstanceID, WebForm.Common.RoomisOperation.put_approve);
            //else
            //{

 FoWoSoft.Platform.Log.Add(string.Format("发送流程({0})", execute.InstanceID.ToString()), "roomis", FoWoSoft.Platform.Log.Types.其它分类);

            new WebForm.Common.Meet().Roomis(execute);
            //   }

        }

        public WorkFlowCustomEventParams WorkFlowCustomEventParamsSet(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
        {
            var eventParams = new FoWoSoft.Data.Model.WorkFlowCustomEventParams()
            {
                FlowID = execute.FlowID,
                GroupID = execute.GroupID,
                StepID = execute.StepID,
                TaskID = execute.TaskID,
                InstanceID = execute.InstanceID,
            };

            return eventParams;
        }
        private void DisplayHtml(Result reslut)
        {
            //判断是打开任务还是关闭窗口
            var nextTasks = reslut.NextTasks.Where(p => p.Status.In(0, 1) && p.ReceiveID == FoWoSoft.Platform.Users.CurrentUserID);
            var nextTask = nextTasks.Count() > 0 ? nextTasks.First() : null;
            if (nextTask != null)
            {
                string url = string.Format("Default.aspx?flowid={0}&stepid={1}&taskid={2}&groupid={3}&instanceid={4}&appid={5}&tabid={6}",
                    nextTask.FlowID, nextTask.StepID, nextTask.ID, nextTask.GroupID, nextTask.InstanceID,
                    Request.QueryString["appid"], Request.QueryString["tabid"]
                    );
                Response.Write("<script type=\"text/javascript\">window.parent.location = '" + url + "';</script>");
            }
            else
            {
                Response.Write("<script type=\"text/javascript\">top.mainTab.closeTab();</script>");
            }
        }

        private Dictionary<Guid, List<FoWoSoft.Data.Model.Users>> GetExecuteSetps(LitJson.JsonData stepsjson, EnumType.ExecuteType executeType)
        {
            Dictionary<Guid, List<FoWoSoft.Data.Model.Users>> steps = new Dictionary<Guid, List<FoWoSoft.Data.Model.Users>>();

            if (stepsjson.IsArray)
            {
                foreach (LitJson.JsonData step in stepsjson)
                {
                    string id = step["id"].ToString();
                    string member = step["member"].ToString();
                    Guid gid;
                    if (id.IsGuid(out gid))
                    {
                        switch (executeType)
                        {
                            case EnumType.ExecuteType.Submit:
                                steps.Add(gid, borganize.GetAllUsers(member));
                                break;
                            case EnumType.ExecuteType.Back:
                                steps.Add(gid, new List<FoWoSoft.Data.Model.Users>());
                                break;
                            case EnumType.ExecuteType.Save:
                                break;
                            case EnumType.ExecuteType.Completed:

                                break;
                            case EnumType.ExecuteType.Redirect:
                                break;
                        }
                    }
                    if (executeType == EnumType.ExecuteType.Redirect)
                    {
                        steps.Add(Guid.Empty, borganize.GetAllUsers(member));
                    }
                }
            }
            return steps;
        }

        private FoWoSoft.Data.Model.WorkFlowExecute.Execute CreateExecute(string instanceid, string flowid, FoWoSoft.Data.Model.WorkFlowInstalled wfInstalled, string comment, string stepid, LitJson.JsonData opationJSON, string title)
        {
            var execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();
            execute.Comment = comment.IsNullOrEmpty() ? "" : comment.Trim();
            execute.FlowID = flowid.ToGuid();
            execute.GroupID = Request.QueryString["groupid"].ToGuid();
            execute.InstanceID = instanceid;
            execute.IsSign = "1" == Request.Form["issign"];
            execute.Note = "";
            execute.Sender = FoWoSoft.Platform.Users.CurrentUser;
            execute.StepID = stepid.IsGuid() ? stepid.ToGuid() : wfInstalled.FirstStepID;
            execute.TaskID = Request.QueryString["taskid"].ToGuid();
            execute.Title = title ?? "";
            execute.ExecuteType = (EnumType.ExecuteType)Enum.Parse(typeof(EnumType.ExecuteType), opationJSON["type"].ToString(), true);
            execute.Steps = GetExecuteSetps(opationJSON["steps"], execute.ExecuteType);
            return execute;
        }

        private FoWoSoft.Data.Model.WorkFlowCustomEventParams NextEvent(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute, FoWoSoft.Data.Model.WorkFlowCustomEventParams eventParams, IEnumerable<FoWoSoft.Data.Model.WorkFlowInstalledSub.Step> steps)
        {
            foreach (var step in steps)
            {
                //步骤提交后事件
                if (!step.Event.SubmitAfter.IsNullOrEmpty() &&
                    (execute.ExecuteType == (EnumType.ExecuteType.Submit | EnumType.ExecuteType.Completed)))
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.SubmitAfter.Trim(), eventParams);
                    Response.Write(string.Format("执行步骤提交后事件：({0}) 返回值：{1}<br/>", step.Event.SubmitAfter.Trim(), obj.ToString()));
                }
                //步骤退回后事件
                if (!step.Event.BackAfter.IsNullOrEmpty() && execute.ExecuteType == EnumType.ExecuteType.Back)
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.BackAfter.Trim(), eventParams);
                    Response.Write(string.Format("执行步骤退回后事件：({0}) 返回值：{1}<br/>", step.Event.BackAfter.Trim(), obj.ToString()));
                }
            }
            return eventParams;
        }

        private FoWoSoft.Data.Model.WorkFlowCustomEventParams firstevent(EnumType.ExecuteType executeType, FoWoSoft.Data.Model.WorkFlowCustomEventParams eventParams, IEnumerable<FoWoSoft.Data.Model.WorkFlowInstalledSub.Step> steps)
        {
            foreach (var step in steps)
            {
                //步骤提交前事件
                if (!step.Event.SubmitBefore.IsNullOrEmpty() &&
                    (executeType == (EnumType.ExecuteType.Submit | EnumType.ExecuteType.Completed)))
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.SubmitBefore.Trim(), eventParams);
                    Response.Write(string.Format("执行步骤提交前事件：({0}) 返回值：{1}<br/>", step.Event.SubmitBefore.Trim(), obj.ToString()));
                }
                //步骤退回前事件
                if (!step.Event.BackBefore.IsNullOrEmpty() && executeType == EnumType.ExecuteType.Back)
                {
                    object obj = btask.ExecuteFlowCustomEvent(step.Event.BackBefore.Trim(), eventParams);
                    Response.Write(string.Format("执行步骤退回前事件：({0}) 返回值：{1}<br/>", step.Event.BackBefore.Trim(), obj.ToString()));
                }
            }
            return eventParams;
        }

        private void execDelegation(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
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
        //归档
        public void WorkFlowArchives(FoWoSoft.Data.Model.WorkFlowInstalled wfInstalled, string title, FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
        {
            if (execute.ExecuteType == (EnumType.ExecuteType.Submit | EnumType.ExecuteType.Completed))
            {
                var currentsteps = wfInstalled.Steps.Where(p => p.ID == execute.StepID);
                if (currentsteps.Count() > 0 && currentsteps.First().Archives == 1)
                {
                    string flowName, stepName;
                    string commentHtml = Request.Form["form_commentlist_div_textarea"];
                    stepName = bworkFlow.GetStepName(execute.StepID, execute.FlowID, out flowName, true);
                    string archivesContents = new FoWoSoft.Platform.WorkFlowForm().GetArchivesString(Request.Form["form_body_div_textarea"], commentHtml);
                    FoWoSoft.Data.Model.WorkFlowArchives wfr = new FoWoSoft.Data.Model.WorkFlowArchives();
                    wfr.Comments = commentHtml;
                    wfr.Contents = archivesContents;
                    wfr.FlowID = execute.FlowID;
                    wfr.FlowName = flowName;
                    wfr.GroupID = execute.GroupID;
                    wfr.ID = Guid.NewGuid();
                    wfr.InstanceID = execute.InstanceID;
                    wfr.StepID = execute.StepID;
                    wfr.StepName = stepName;
                    wfr.Title = title.IsNullOrEmpty() ? flowName + "-" + stepName : title;
                    wfr.TaskID = execute.TaskID;
                    wfr.WriteTime = FoWoSoft.Utility.DateTimeNew.Now;
                    new FoWoSoft.Platform.WorkFlowArchives().Add(wfr);
                }
            }
        }


    }
}