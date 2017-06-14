using FoWoSoft.Data.Model.WorkFlowExecute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowRun
{
    public partial class Execute2 : System.Web.UI.Page
    {
             FoWoSoft.Platform.WorkFlowOperation workFlowOperstion;
            FoWoSoft.Platform.WorkFlowOperationData data = new FoWoSoft.Platform.WorkFlowOperationData();
            protected void Page_Load(object sender, EventArgs e)
            {
                WebForm.Common.Tools.CheckLogin(false);
                data.params1 = Request.Form["params"];

                if (data.params1.IsNullOrEmpty())
                {
                    Response.Write("参数为空!");
                    Response.End();
                }
                data.instanceid = Request.QueryString["instanceid"] ?? Request.Form["instanceid"];

                data.flowid = Request.QueryString["flowid"];

                data.comment = Request.Form["comment"];


                data.stepid = Request.QueryString["stepid"];
                data.title = Request.Form[Request.Form["Form_TitleField"]];
                data.isSystemDetermine = Request.QueryString["isSystemDetermine"];
                data.commentHtml = Request.Form["form_commentlist_div_textarea"];
                data.formHtml = Request.Form["form_body_div_textarea"];
                data.GroupID = Request.QueryString["groupid"];
                data.IsSign = Request.Form["issign"];
                data.TaskID = Request.QueryString["taskid"];
                data.CurrentUser = FoWoSoft.Platform.Users.CurrentUser;
                workFlowOperstion = new FoWoSoft.Platform.WorkFlowOperation(data);

                if (!workFlowOperstion.Check)
                {
                    Response.Write(workFlowOperstion.Msg);
                    Response.End();
                }


                Response.Write("执行参数：" + data.params1 + "<br/>");

                //操作roomis审核
                if (data.params1.ToLower().Contains("completed"))
                    new WebForm.Common.Meet().Roomis(data.instanceid, WebForm.Common.RoomisOperation.put_approve);

                var reslut = workFlowOperstion.result;
                Response.Write(workFlowOperstion.Msg);
                Response.Write(string.Format("处理流程步骤结果：{0}<br/>", reslut.IsSuccess ? "成功" : "失败"));
                Response.Write(string.Format("调试信息：{0}", reslut.DebugMessages));
                Response.Write("<script type=\"text/javascript\">alert('" + reslut.Messages + "');top.mainDialog.close();</script>");

                if (reslut.IsSuccess)
                {
                    DisplayHtml(reslut);
                }

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
        }
}