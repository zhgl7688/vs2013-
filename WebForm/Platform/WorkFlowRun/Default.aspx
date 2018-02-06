﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForm.Platform.WorkFlowRun.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <%
        string flowid = Request.QueryString["flowid"];//2ea49481-3ee2-4805-ab85-53604d696976
        string instanceid = Request.QueryString["instanceid"];
        string taskid = Request.QueryString["taskid"];
        string stepid = Request.QueryString["stepid"];
        string groupid = Request.QueryString["groupid"];
        string display = Request.QueryString["display"] ?? "0";

        FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        FoWoSoft.Platform.WorkFlowTask btask = new FoWoSoft.Platform.WorkFlowTask();
        FoWoSoft.Platform.WorkFlowButtons bworkFlowButtons = new FoWoSoft.Platform.WorkFlowButtons();
        FoWoSoft.Platform.AppLibrary bappLibrary = new FoWoSoft.Platform.AppLibrary();
        Guid flowID;
        if (!flowid.IsGuid(out flowID))
        {
            Response.Write("流程ID为空!");
            Response.End();
        }
        var wfInstalled = bworkFlow.GetWorkFlowRunModel(flowID);
        if (wfInstalled == null)
        {
            Response.Write("未找到流程运行时!");
            Response.End();
        }
        else if (wfInstalled.Status == 3)
        {
            Response.Write("该流程已被卸载,不能发起新的流程实例!");
            Response.End();
        }
        else if (wfInstalled.Status == 4)
        {
            Response.Write("该流程已被删除,不能发起新的实例!");
            Response.End();
        }
        Guid stepID;
        if (!stepid.IsGuid(out stepID))
        {
            stepID = wfInstalled.FirstStepID;//307552c3-547d-445b-9c6b-e42737568b10
        }

        FoWoSoft.Data.Model.WorkFlowInstalledSub.Step currentStep = wfInstalled.Steps.ToList().Find(p => p.ID == stepID);

        if (currentStep == null)
        {
            Response.Write("未找到流程步骤!");
            Response.End();
        }
        int isArchives = currentStep.Archives;//是否要归档
        string query = string.Format("flowid={0}&instanceid={1}&taskid={2}&stepid={3}&groupid={4}&appid={5}&tabid={6}",
            flowID, instanceid, taskid, stepID, groupid, Request.QueryString["appid"], Request.QueryString["tabid"]);

        //是否是抄送任务
        bool isCopyFor = false;

        //更新打开时间
        Guid taskgid;

        //如果是执行，并且任务ID为GUID,则更新打开时间和状态，检查当前任务是否可以执行。
        if ("0" == display && taskid.IsGuid(out taskgid))
        {

            btask.UpdateOpenTime(taskgid, FoWoSoft.Utility.DateTimeNew.Now, true);

            var task = btask.Get(taskgid);
            if (task != null)
            {
                if ("1" != display && task.Status.In(2, 3, 4, 5))
                {
                    Response.Write("<script type='text/javascript'>alert('该任务已处理,请刷新您的待办列表!');top.mainTab.closeTab();</script>");
                    Response.End();
                }
                else if ("1" != display && task.ReceiveID != FoWoSoft.Platform.Users.CurrentUserID)
                {
                    Response.Write("<script type='text/javascript'>alert('您不能处理当前任务,请刷新您的待办列表!');top.mainTab.closeTab();</script>");
                    Response.End();
                }
                isCopyFor = task.Type == 5;
            }
        }

        var form = currentStep.Forms.First();//目前只显示一个表单
        var appLibrary = bappLibrary.Get(form.ID, true);
        string src = appLibrary == null ? "" : WebForm.Common.Tools.BaseUrl + appLibrary.Address;
        bool isCustomeForm = appLibrary != null && appLibrary.Code.IsNullOrEmpty();//是否是自定义表单

        int debugType = wfInstalled.Debug;
        wfInstalled.DebugUsers.RemoveAll(p => p == null);
        bool isDebug = (debugType == 1 || debugType == 2) && wfInstalled.DebugUsers.Exists(p => p.ID == FoWoSoft.Platform.Users.CurrentUserID);
        bool isSign = currentStep.SignatureType == 1 || currentStep.SignatureType == 2;//是否有意见
        int signType = currentStep.SignatureType;
        int siSignCenter = 0;
        if ((stepID == Guid.Parse("3daf19f5-ce5e-4773-a783-581500722498")) ||
            (stepID == Guid.Parse("b1f08f44-4692-4307-82fa-32c6026201a3")) ||
            (stepID == Guid.Parse("88B44E40-E9EB-44F9-9F2B-18B0AAE70A5A")) ||
            (stepID == Guid.Parse("72578ab0-b803-4f0b-b0c0-1faf3c99ea7e"))
            )
        {
            siSignCenter = 1;
        }



    %>
    <script>
        var isSignCenter = '<%=siSignCenter%>';
      
       
    </script>
    <form id="mainform" name="mainform" method="post" target="submiter">
        <%if ("0" == display)
            { //display为0表示执行，1表示查看 %>
        <div class="toolbar" style="margin-top: 0; border-top: none 0; position: fixed; top: 0; left: 0; right: 0; margin-left: auto; z-index: 999; width: 100%; margin-right: auto; height: 26px;">
            <div>
                <%
                    //如果是抄送，只显示完成按钮
                    List<FoWoSoft.Data.Model.WorkFlowInstalledSub.StepSet.Button> buttons = new List<FoWoSoft.Data.Model.WorkFlowInstalledSub.StepSet.Button>();
                    if (isCopyFor)
                    {
                        buttons.Add(new FoWoSoft.Data.Model.WorkFlowInstalledSub.StepSet.Button()
                        {
                            ID = "954EFFA8-03B8-461A-AAA8-8727D090DCB9",
                            Note = "完成",
                            Sort = 0
                        });
                    }
                    else
                    {
                        foreach (var button in currentStep.Buttons)
                        {
                            buttons.Add(button);
                        }
                    }

                    foreach (var button in buttons)
                    {
                        if (button == null)
                        {
                            continue;
                        }
                        Guid buttonID;
                        if (button.ID.IsGuid(out buttonID))
                        {
                            var button1 = bworkFlowButtons.Get(buttonID, true);
                            if (button1 == null)
                            {
                                continue;
                            }
                            var lkk = "yes111";
                            var funName = string.Concat("fun_", button1.ID.ToString("N"), "()");
                            if (siSignCenter == 1 && button1.Title == "退回")
                            {
                                button1.Title = "否决";
                                lkk = "no111";
                            }

                %>

                <a href="#" id="<%=lkk  %>" onclick="<%=funName %>;return false;" title="<%=!button.Note.IsNullOrEmpty() ? button.Note : button1.Note.IsNullOrEmpty() ? "" : button1.Note.Replace("\"", "'") %>">
                    <span style="background: url(../../<%=button1.Ico%>) no-repeat left center;"><%=button1.Title %></span>
                </a>
                <script type="text/javascript">
                    function <%=funName + "{" + button1.Script + "}" %>
</script>
                <%}
                    else if (string.Compare(button.ID, "other_splitline", true) == 0)
                    { //显示其它特定按钮如分隔线| %>
                <span class="toolbarsplit">&nbsp;</span>
                <%}%>
                <%}%>
            </div>
        </div>

        <input type="hidden" name="instanceid" id="instanceid" value="" />
        <input type="hidden" name="params" id="params" value="" />
        <%if (isDebug && debugType == 1)
            {%>
        <br />
        <br />
        <iframe name="submiter" style="width: 99%; height: 200px; border: 1px solid #666; margin-left: 4px; overflow: auto;"></iframe>
        <%}
            else
            {%>
        <iframe name="submiter" style="width: 99%; height: 1px; margin: 0; display: none;"></iframe>
        <%}%>
        <%} %>

        <!--表单主体-->
        <div style="<%=isCustomeForm?"": "margin:8px;"%>" id="form_body_div">
            <%
                if (!src.IsNullOrEmpty())
                {
                    if (!isCustomeForm)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        System.IO.TextWriter tw = new System.IO.StringWriter(sb);
                        Guid g;
                        if (Guid.TryParse(instanceid, out g))
                        {
                            if (new FoWoSoft.Platform.TempTestMeet().Get(g).UserID == "直播会议")
                                src = "/Platform/WorkFlowFormDesigner/Forms/secord.aspx";
                        }


                        Server.Execute(src, tw);
                        Response.Write(sb.ToString().RemovePageTag());
                    }
                    else
                    {
                        Response.Write("<input type=\"hidden\" id=\"Form_TitleField\" name=\"Form_TitleField\" value=\"customformtitle\" />");
                        Response.Write("<input type=\"hidden\" id=\"customformtitle\" name=\"customformtitle\" value=\"\" />");
                        Response.Write("<input type=\"hidden\" id=\"Form_AutoSaveData\" name=\"Form_AutoSaveData\" value=\"0\" />");
                        //Response.Write("<input type=\"hidden\" id=\"instanceid\" name=\"instanceid\" value=\"\" />");
                        src = src.IndexOf('?') >= 0 ? src + Request.Url.Query.Substring(1) : src + Request.Url.Query;
                        Response.Write("<iframe src=\"" + (src.StartsWith("/") ? src : "/" + src) + "\" id=\"customeformiframe\" style=\"border:none 0;padding:0;width:100%;margin-top:28px;\"></iframe>");
                    }
                }
            %>
        </div>
        <!--表单主体-->

        <!--意见处理栏-->
        <%
            if (isSign && "0" == display && !isCopyFor)
            {
                var displayOk = true;
                string commentsOptions = new FoWoSoft.Platform.WorkFlowComment().GetOptionsStringByUserID(FoWoSoft.Platform.Users.CurrentUserID);
                if (stepID == Guid.Parse("3daf19f5-ce5e-4773-a783-581500722498") ||
          (stepID == Guid.Parse("b1f08f44-4692-4307-82fa-32c6026201a3"))//||
                                                                        // (stepID == Guid.Parse("72578ab0-b803-4f0b-b0c0-1faf3c99ea7e")) 
          )
                {
                    commentsOptions += string.Format(" <option value=\"{0}\">{0}</option>", "加签");
                }
                else if (stepID == Guid.Parse("45E6F561-1BA6-4ED0-8281-7AE905E0804C"))
                {
                    displayOk = false;
                    commentsOptions = string.Format(" <option value=\"{0}\" select=\"\">{0}</option>", "同意");
                }

        %>
        <div style="height: 12px; margin: 16px 8px 8px 8px; border-bottom: 1px dashed #ccc;"></div>
        <div style="height: 30px; margin: 15px auto 8px auto; text-align: left; width: 96%;">
            处理意见：
            <% if (displayOk){ %>
            <select class="myselect" id="mycomment"  style="margin-right: 6px; width: 100px;" onchange="$('#comment').val(this.value);"><option value=""></option>
                <%=commentsOptions %>


            </select>
            <%}else{ %>
             <input   id="mycomment" value="同意"  type="hidden"/>   
            <%} %>
            &nbsp;<input type="text" class="mytext" id="comment" name="comment" value="" style="width: 70%; margin-right: 6px;" />
            <%if (signType == 2)
                {%>
            <input type="hidden" value="" id="issign" name="issign" />
            <input type="button" class="mybutton" id="signbutton" onclick="sign();" value="&nbsp;&nbsp;签&nbsp;&nbsp;章&nbsp;&nbsp;" />
            <%
                string signFile = string.Concat(Server.MapPath("../../Files/UserSigns/"), FoWoSoft.Platform.Users.CurrentUserID, ".gif");
                string signSrc = string.Concat("../../Files/UserSigns/", FoWoSoft.Platform.Users.CurrentUserID, ".gif");
                if (!System.IO.File.Exists(signFile))
                {
                    System.Drawing.Bitmap img = new FoWoSoft.Platform.WorkFlow().CreateSignImage(FoWoSoft.Platform.Users.CurrentUserName);
                    if (img != null)
                    {
                        img.Save(signFile, System.Drawing.Imaging.ImageFormat.Gif);
                    }
                }
            %>
            <img alt="" src="<%=signSrc %>" id="signimg" style="vertical-align: middle; display: none;" />
            <%}%>
        </div>
        <%}%>
        <!--意见处理栏-->

        <!--历史意见显示-->
        <div id="form_commentlist_div">
            <%
                if (currentStep.OpinionDisplay == 1)//如果步骤设置为要显示意见
                {
                    Server.Execute("ShowComment.aspx");
                }
            %>
        </div>
        <!--历史意见显示-->

        <!--归档内容-->
        <textarea id="form_body_div_textarea" name="form_body_div_textarea" style="display: none;"></textarea>
        <textarea id="form_commentlist_div_textarea" name="form_commentlist_div_textarea" style="display: none;"></textarea>
        <!--归档内容-->
    </form>

    <script type="text/javascript">
        var isDebug = '<%=isDebug%>' == 'True' && '1' == '<%=debugType%>';
        var isSign = '<%=isSign%>' == 'True';
        var signType = '<%=signType%>';
        var iframeid = '<%=Request.QueryString["tabid"]%>';
        var isShow = "0" != "<%=display%>";//是否是查看模式
        var appid = '<%=Request.QueryString["appid"]%>';
        var query = '<%=query%>';
        var isSystemDetermine = '<%=currentStep.Behavior.FlowType==0?"1":"0"%>';//当前步骤的后续流转类型是否是系统判断
        var instanceid ='<%=instanceid%>';
        var isCustomeForm = '<%=isCustomeForm?"1":"0"%>';
        $(function () {
            if ("1" == "<%=isArchives%>") {
                $("#form_body_div_textarea").val($("#form_body_div").html());
                $("#form_commentlist_div_textarea").val($("#form_commentlist_div").html());
            }
            if ("1" == isCustomeForm) {
                $("#customeformiframe").height($(window).height() - 32);
            }
        });
        //$('#mycomment').change(function () {
        //    // alert($(this).children('option:selected').val()); 
        //    var s = $(this).children('option:selected').val();
        //    if (s == "同意" || s == "加签") {
        //        $('#yes111').attr("disabled", false);
        //        $('#no111').attr("disabled", true);
        //    } else
        //        if (s == "不同意") {
        //            $('#yes111').attr("disabled", true);
        //            $('#no111').attr("disabled", false);
        //        }
        //});
    </script>
    <script type="text/javascript" src="Scripts/common.js"></script>
</body>
</html>
