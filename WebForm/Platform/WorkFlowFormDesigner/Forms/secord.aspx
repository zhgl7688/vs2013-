<%@ Page Language="C#" %>

<%
    string FlowID = Request.QueryString["flowid"];
    string StepID = Request.QueryString["stepid"];
    string GroupID = Request.QueryString["groupid"];
    string TaskID = Request.QueryString["taskid"];
    string InstanceID = Request.QueryString["instanceid"];
    string DisplayModel = Request.QueryString["display"] ?? "0";
    if (InstanceID.IsNullOrEmpty()) { InstanceID = Request.QueryString["instanceid1"]; }
    FoWoSoft.Platform.WorkFlow BWorkFlow = new FoWoSoft.Platform.WorkFlow();
    FoWoSoft.Platform.WorkFlowTask BWorkFlowTask = new FoWoSoft.Platform.WorkFlowTask();
    var fieldStatus = BWorkFlow.GetFieldStatusAndCheck(FlowID, StepID);
    Guid g;
    if (!Guid.TryParse(InstanceID, out g))
    {
        return;
    }

    FoWoSoft.Data.Model.TempTestMeet tempMeet = null;
    FoWoSoft.Data.Model.MeetInfo meetInfo = null;
    string displayPhoneAddress = "";

    tempMeet = new FoWoSoft.Platform.TempTestMeet().Get(g);
     var Date1 = tempMeet.Date1 != null ? (DateTime)tempMeet.Date1 : DateTime.Now;
    var Date2 = tempMeet.Date2 != null ? (DateTime)tempMeet.Date2 : DateTime.Now;
     var ID = tempMeet.ID.ToString();

 
    meetInfo = new FoWoSoft.Platform.MeetInfo().GetByTemp3(InstanceID);
    var account = new FoWoSoft.Platform.Users().GetByAccount(meetInfo.AdminId);
    meetInfo.AdminId = account.ID.ToString();

    if (!string.IsNullOrEmpty(tempMeet.DeptID))
    {
        displayPhoneAddress = "<span style='margin-left:15px;color:red'>备注：</span>" + "预约人：" + account.Name + ";<span style='margin-left:10px;  font:bold;'>预约人号码：</span>" + tempMeet.DeptID;
    }
    if (!string.IsNullOrEmpty(tempMeet.DeptName))
    {
        displayPhoneAddress += "<br/><span style='margin-left:38px;  font:bold;'>会场地址：</span>" + tempMeet.DeptName + "";
    }


%>

<link href="Scripts/Forms/flowform.css" rel="stylesheet" type="text/css" />
<%--<script src="Scripts/Forms/common.js" type="text/javascript"></script>--%>
<input type="hidden" id="Form_ValidateAlertType" name="Form_ValidateAlertType" value="1" />
<input type="hidden" id="Form_TitleField" name="Form_TitleField" value="TempTestMeet.Title" />
<input type="hidden" id="Form_DBConnID" name="Form_DBConnID" value="06075250-30dc-4d32-bf97-e922cb30fac8" />
<input type="hidden" id="Form_DBTable" name="Form_DBTable" value="TempTestMeet" />
<input type="hidden" id="Form_DBTablePk" name="Form_DBTablePk" value="ID" />
<input type="hidden" id="Form_DBTableTitle" name="Form_DBTableTitle" value="Title" />
<input type="hidden" id="Form_AutoSaveData" name="Form_AutoSaveData" value="1" />
<script>
    var meetinfo;
    var memberId;
    var currentStep = -1;
     <%if (StepID == "04dbab0d-e751-4579-8007-8f9f59683a24")
    {
        var currentTask = BWorkFlowTask.Get(Guid.Parse(TaskID));
        %>
    currentStep = 4;
    memberId ="<%=currentTask.SenderID%>"
        <%}%>
     <%if (StepID == "65bb6ee3-826c-4337-8dbd-2229c95f6f29")
    {
        var currentTask = BWorkFlowTask.Get(Guid.Parse(TaskID));
        %>

    currentStep = 3;
    memberId ="<%=currentTask.SenderID%>"
        <%}%>
 
</script>
<style>
    .tableleft {
        width: 130px;
    }

    .tableDate {
        width: 100px;
        text-align: center;
    }
</style>
<p style="text-align: center; margin-top: 50px"><span style="font-size: 24px;"><strong></strong></span></p>
<p style="text-align: center;"><span style="font-size: 24px;"><strong><span style="font-size: 18px;">直  播 会 议 </span></strong></span></p>
<p><span style="font-size: 24px;"></span></p>
<table class="flowformtable" data-sort="sortDisabled" cellspacing="1" cellpadding="0">
    <tbody>
        <tr class="firstRow">
            <td class="tableleft" valign="top">活动名称：   </td>
            <td width="300" valign="top">
                <input id="TempTestMeet.Title"  type1="flow_text"  disabled="disabled"  name="TempTestMeet.Title" value="<%=tempMeet.Title%>" style="width: 90%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
            <td class="tableDate" valign="top">会议调试时间： 
            </td>
            <td colspan="1" rowspan="1" width="255" valign="top">
                <input type1="flow_datetime" id="TempTestMeet.Date1"  disabled="disabled"  name="TempTestMeet.Date1" value="<%=Date1%>" format="yyyy-MM-dd HH:mm" defaultvalue="" istime="1" daybefor="0" dayafter="1" currentmonth="0" isflow="1" class="mycalendar" title="" type="text" /></td>
        </tr>
        <tr>
            <td class="tableleft" valign="top">主办单位：  </td>
            <td width="300" valign="top">
                <input id="TempTestMeet.test1" type1="flow_text"  disabled="disabled"   name="TempTestMeet.test1" value="<%=tempMeet.test1%>" style="width: 90%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
            <td class="tableDate" valign="top">正式会议时间：<br />
            </td>
            <td colspan="1" rowspan="1" width="255" valign="top">
                <input type1="flow_datetime" id="TempTestMeet.Date2"  disabled="disabled"   name="TempTestMeet.Date2" value="<%=Date2%>" format="yyyy-MM-dd HH:mm" defaultvalue="" istime="1" daybefor="0" dayafter="1" currentmonth="0" isflow="1" class="mycalendar" title="" type="text" /></td>
        </tr>
        <tr>
            <td class="tableleft" valign="top">简述：<br />
            </td>
            <td rowspan="1" colspan="3" valign="top">
                <textarea isflow="1" type1="flow_textarea"  disabled="disabled"   id="TempTestMeet.test" name="TempTestMeet.test" class="mytext" style="width: 80%; height: 100px"><%= tempMeet.test %></textarea></td>
        </tr>

         <tr>
            <td colspan="1" rowspan="1" style="word-break: break-all;" valign="top">预约类型</td>
            <td colspan="3" rowspan="1" valign="top">
                <input id="TempTestMeet.UserID" name="TempTestMeet.UserID"  disabled="disabled"  value="<%=tempMeet.UserID %>"  style="width: 50%" valuetype="0" isflow="1" class="mytext" title="" type="text">
            </td>
        </tr> 
        <tr>
            <td class="tableleft" valign="top">直播平台:   </td>
            <td rowspan="1" colspan="3" valign="top">
                 <input id="TempTestMeet.Type"   name="TempTestMeet.UserID"  disabled="disabled"  value="<%=tempMeet.Type %>" style="width: 50%" valuetype="0" isflow="1" class="mytext" title="" type="text" >
              </td>
        </tr>
        <% if (tempMeet.Type ==Enum.GetName(typeof(FoWoSoft.Data.Model.LivePlatform), FoWoSoft.Data.Model.LivePlatform.华师大直播平台))
            { %>
        <tr>
            <td class="tableleft" valign="top">直播范围:</td>
            <td colspan="3" rowspan="1" valign="top">
                  <input id="TempTestMeet.Reason"  name="TempTestMeet.Reason"  disabled="disabled"  value="<%=tempMeet.Reason%>" style="width: 50%" valuetype="0" isflow="1" class="mytext" title="" type="text" >
             </td>
        </tr>
      <tr>
            <td class="tableleft" valign="top">是否分会场转播:</td>
            <td colspan="3" rowspan="1" valign="top">
                 <input id="TempTestMeet.UserID_text"  name="TempTestMeet.UserID_text"  disabled="disabled"  value="<%=tempMeet.UserID_text%>" style="width: 50%" valuetype="0" isflow="1" class="mytext" title="" type="text" >
             </td>
        </tr>
        <%}
            else
            { %>
        <tr class="platformType2">
            <td class="tableleft" valign="top">入场设备信息:   </td>
             <td rowspan="1" colspan="3" >
                <input id="TempTestMeet.inland" type1="flow_text"  disabled="disabled"  name="TempTestMeet.inland" value="<%= tempMeet.inland%>" style="width: 80%" valuetype="0" isflow="1" class="mytext" title="" type="text" />
            </td>
        </tr>
        <tr class="platformType2">
            <td class="tableleft" valign="top">Mac地址:  </td>
             <td colspan="3" rowspan="1" >
                <input id="TempTestMeet.abroad"  disabled="disabled"    type1="flow_text" name="TempTestMeet.abroad" value="<%=tempMeet.abroad%>" style="width: 80%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>

        </tr>
        <tr class="platformType2" >
            <td class="tableleft" valign="top">其他需求：  </td>
            <td colspan="3" rowspan="1"  >
                   <textarea isflow="1" type1="flow_textarea" disabled="disabled"   id="TempTestMeet.test2_text" name="TempTestMeet.test2_text" class="mytext" style=" width: 80%; height: 100px; text-align:left"><%=tempMeet.test2_text.Trim()%></textarea></td>
       </tr>
        <%} %>
    </tbody>
</table>

<br />
<%=displayPhoneAddress%><br />

<script>
  
     document.getElementById("TempTestMeet.Date1").disabled = true;
    document.getElementById("TempTestMeet.Date2").disabled = true;
  

    
</script>
 

 