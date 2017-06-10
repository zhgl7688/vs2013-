<%@ Page Language="C#" %>

<%
    string FlowID = Request.QueryString["flowid"];
    string StepID = Request.QueryString["stepid"];//
    string GroupID = Request.QueryString["groupid"];
    string TaskID = Request.QueryString["taskid"];
    string InstanceID = Request.QueryString["instanceid"];
    string DisplayModel = Request.QueryString["display"] ?? "0";

    // string DBConnID = "06075250-30dc-4d32-bf97-e922cb30fac8";
    // string DBTable = "TempTestMeet";
    // string DBTablePK = "ID";
    // string DBTableTitle = "Title";
    if (InstanceID.IsNullOrEmpty()) { InstanceID = Request.QueryString["instanceid1"]; }
    FoWoSoft.Platform.Dictionary BDictionary = new FoWoSoft.Platform.Dictionary();
    FoWoSoft.Platform.WorkFlow BWorkFlow = new FoWoSoft.Platform.WorkFlow();
    FoWoSoft.Platform.WorkFlowTask BWorkFlowTask = new FoWoSoft.Platform.WorkFlowTask();
    var fieldStatus = BWorkFlow.GetFieldStatusAndCheck(FlowID, StepID);
    // LitJson.JsonData initData = BWorkFlow.GetFormData(DBConnID, DBTable, DBTablePK, InstanceID, fieldStatus);
    // string TaskTitle = BWorkFlow.GetFromFieldData(initData, DBTable, DBTableTitle);

    #region 根据用户信息查获会议信息
    FoWoSoft.Data.Model.MeetInfo meet = null;
    string seconId = "";
    if (StepID == null || StepID == new FoWoSoft.Platform.WorkFlow().GetWorkFlowRunModel(FlowID).FirstStepID.ToString())
    {
       if(InstanceID!=null) meet = new FoWoSoft.Platform.MeetInfo().GetByTemp3(InstanceID);
        if (meet!=null) meet.AdminId = new FoWoSoft.Platform.Users().GetByAccount(meet.AdminId).ID.ToString();
       
    }
    #endregion

    Guid g;
    string Title = "";
    DateTime Date1 = DateTime.Now;
    DateTime Date2 = DateTime.Now;
    string college = meet != null ? meet.MeetId : "";

    string abroad = "";
    string ID = "";
    string inland = "";
    string Reason = "国内异地";
    string Type = "";
    var test1 = "";
    var test = "";
    var UserID = "论坛";
    if (Guid.TryParse(InstanceID, out g))
    {
        var tempMeet = new FoWoSoft.Platform.TempTestMeet().Get(g);
        Title = tempMeet.Title;
        if (tempMeet.Date1 != null) Date1 = (DateTime)tempMeet.Date1;
        if (tempMeet.Date2 != null) Date2 = (DateTime)tempMeet.Date2;

        college = tempMeet.college;
        abroad = tempMeet.abroad;
        ID = tempMeet.ID.ToString();
        inland = tempMeet.inland;
        Reason = tempMeet.Reason;
        Type = tempMeet.Type;
        test1 = tempMeet.test1;
        test = tempMeet.test;
        UserID = tempMeet.UserID;
        if (Reason == "校内") {fieldStatus["Date2"] = 1;
        fieldStatus["Reason"] = 1;
        }
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
    var ReasonArray = ["校内", "国内异地", "国外"];
    var UserIDArray = ["论坛", "报告", "讲座", "答辩", "例会", "其他"];
    var meetinfo = null;
    <%if (meet != null)
      {  %>
    meetinfo = {
        MeetTimes: '<%=meet.MeetTimes%>', MeetId: '<%=meet.MeetId%>', AdminId: '<%=meet.AdminId%>'
       };
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
<p style="text-align: center;margin-top:50px"><span style="font-size: 24px;"><strong></strong></span></p>
<p style="text-align: center;"><span style="font-size: 24px;"><strong><span style="font-size: 18px;">视 频 会 议 </span></strong></span></p>
<p><span style="font-size: 24px;"></span></p>
<table class="flowformtable" data-sort="sortDisabled" cellspacing="1" cellpadding="0">
    <tbody>
        <tr class="firstRow">
            <td  class="tableleft" valign="top">活动名称：<br />
            </td>
            <td width="300" valign="top">
                <input id="TempTestMeet.Title" "<%=fieldStatus["Title"]==0?" ":" disabled='disabled'" %>" type1="flow_text" name="TempTestMeet.Title" value="<%=Title%>" style="width: 90%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
            <td class="tableDate" valign="top">会议调试时间：<br />
            </td>
            <td colspan="1" rowspan="1" width="255" valign="top">
                <input type1="flow_datetime" id="TempTestMeet.Date1"  "<%=fieldStatus["Date1"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.Date1" value="<%=Date1%>" format="yyyy-MM-dd HH:mm" defaultvalue="" istime="1" daybefor="0" dayafter="1" currentmonth="0" isflow="1" class="mycalendar" title="" type="text" /></td>
        </tr>
        <tr>
            <td class="tableleft" valign="top">主办单位：<br />
            </td>
            <td width="300" valign="top">
                <input id="TempTestMeet.test1" type1="flow_text"  "<%=fieldStatus["test1"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.test1" value="<%=test1%>" style="width: 90%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
            <td class="tableDate" valign="top">正式会议时间：<br />
            </td>
            <td colspan="1" rowspan="1" width="255" valign="top">
                <input type1="flow_datetime" id="TempTestMeet.Date2"  "<%=fieldStatus["Date2"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.Date2" value="<%=Date2%>" format="yyyy-MM-dd HH:mm" defaultvalue="" istime="1" daybefor="0" dayafter="1" currentmonth="0" isflow="1" class="mycalendar" title="" type="text" /></td>
        </tr>
        <tr>
            <td class="tableleft" valign="top">简述：<br />
            </td>
            <td rowspan="1" colspan="3" valign="top">
                <textarea isflow="1" type1="flow_textarea"   "<%=fieldStatus["test"]==0?" ":" disabled='1'"  %>"  id="TempTestMeet.test" name="TempTestMeet.test" class="mytext" style="width: 80%; height: 100px"><%=test %></textarea></td>
        </tr>
        
        <tr>
            <td colspan="1" rowspan="1" style="word-break: break-all;" valign="top">会议类型</td>
            <td colspan="3" rowspan="1" valign="top">
                <select class="mycombox" id="TempTestMeet.UserID"  "<%=fieldStatus["UserID"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.UserID" datasource="1" listmode="0"  isflow="1" type1="flow_combox"  >
                    <option value="论坛" >论坛</option>
                    <option value="报告">报告</option>
                    <option value="讲座" >讲座</option>
                     <option value="答辩" >答辩</option>
                     <option value="例会" >例会</option>
                     <option value="其他" >其他</option>
                </select> </td>
        </tr>
        <tr>
            <td  class="tableleft" valign="top">对端会场情况（可多选）<br />
            </td>
            <td rowspan="1" colspan="3" valign="top">
                <input name="TempTestMeet.Type" id="TempTestMeet.Type_0" value="校内视频会议室"  "<%=fieldStatus["Type"]==0?" ":" disabled='1'"  %>"  style="vertical-align: middle;" isflow="1" type1="flow_checkbox" type="checkbox" "<%=Type.IndexOf("校内视频会议室")>-1?" checked='checked'":"" %>"/>
                <label for="TempTestMeet.Type_0" style="vertical-align: middle; margin-right: 3px;">校内视频会议室</label>
                <input name="TempTestMeet.Type" id="TempTestMeet.Type_1"  "<%=fieldStatus["Type"]==0?" ":" disabled='1'"  %>"  value="标准H.323\SIP会议终端（如：polycom视频会议终端）" style="vertical-align: middle;" isflow="1" type1="flow_checkbox" type="checkbox"  "<%=Type.Contains("标准H.323\\SIP会议终端（如：polycom视频会议终端）")?" checked='checked'":"" %>" />
                <label for="TempTestMeet.Type_1" style="vertical-align: middle; margin-right: 3px;">标准H.323\SIP会议终端（如：polycom视频会议终端）</label>
                <input name="TempTestMeet.Type" id="TempTestMeet.Type_2"  "<%=fieldStatus["Type"]==0?" ":" disabled='1'"  %>"  value="PC或移动终端" style="vertical-align: middle;" isflow="1" type1="flow_checkbox" type="checkbox" "<%=Type.IndexOf("PC或移动终端")>-1?" checked='checked'":"" %>"/>
                <label for="TempTestMeet.Type_2" style="vertical-align: middle; margin-right: 3px;">PC或移动终端</label></td>
        </tr>
        <tr>
            <td class="tableleft" valign="top">对端会场位置</td>
            <td colspan="3" rowspan="1" valign="top">
                <select class="mycombox" style="width:200px" id="TempTestMeet.Reason"  "<%=fieldStatus["Reason"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.Reason" datasource="1" listmode="0"  isflow="1" type1="flow_combox" onchange="onchange_27a9bf064a4b1410f3e5c9a4eaa6b983 (this);" >
                      <option value="校内">校内</option>
                    <option value="国内异地">国内异地</option>
                    <option value="国外" >国外</option>
                </select><script type="text/javascript">function onchange_27a9bf064a4b1410f3e5c9a4eaa6b983(obj) {

    if (obj.value == '校内') {
       if (meetinfo == null) {
            alert("没有申请会场信息!"); return;
        } else {
            document.getElementById('TempTestMeet.Date2').value = meetinfo.MeetTimes;
            document.getElementById('TempTestMeet.Date2').readOnly = true;
            $('#TempTestMeet.Date2').unbind();
        }
    } else {
        document.getElementById('TempTestMeet.Date2').disabled = false;
    }
    var index = ReasonArray.indexOf(obj.value);
     for (var i = 0; i < 3; i++) {
        if (index == i) document.getElementById('Reason' + i).style = "";
        else document.getElementById('Reason' + i).style.display = "none";
    }


}</script></td>
        </tr>
        <tr id="Reason0" style="display:none" >
            <td class= "tableleft" valign="top">
                <br />
            </td>
            <td rowspan="1" colspan="3" style="word-break: break-all;" valign="top">校内：
                <select class="mycombox" id="TempTestMeet.college"   disabled="disabled"  name="TempTestMeet.college" datasource="0" listmode="0" isflow="1" type1="flow_combox">
                    <%=WebForm.Common.Meet.GetMeetOptions(college)%>  </select>
                <%--<select class="mycombox" id="TempTestMeet.college"  "<%=fieldStatus["college"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.college" datasource="0" listmode="0" isflow="1" type1="flow_combox">
                   <%=WebForm.Common.Meet.GetMeetOptions(college)%>  </select>--%>
             <%--  <input  id="TempTestMeet.college" type1="flow_text"  "<%=fieldStatus["college"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.college" value="<%=college%>" style="width: 80%" valuetype="0" isflow="1" class="mytext" title="" type="text" /> 
 --%>
            </td>
        </tr>
        <tr id="Reason1" style="display:none">
            <td class= "tableleft"  valign="top">
                <br />
            </td>
            <td colspan="3" rowspan="1" style="word-break: break-all;" valign="top">国内异地 
                <input  id="TempTestMeet.inland" type1="flow_text"  "<%=fieldStatus["Title"]==0?" ":" disabled='1'"  %>"  name="TempTestMeet.inland" value="<%=inland%>" style="width: 80%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
        </tr>
        <tr id="Reason2" style="display:none">
            <td class= "tableleft"  valign="top">
                <br />
            </td>
            <td colspan="3" rowspan="1" style="word-break: break-all;" valign="top">国外：<input id="TempTestMeet.abroad"  "<%=fieldStatus["abroad"]==0?" ":" disabled='1'"  %>"  type1="flow_text" name="TempTestMeet.abroad" value="<%=abroad%>" style="width: 80%" valuetype="0" isflow="1" class="mytext" title="" type="text" /></td>
        </tr>
    </tbody>
</table>
<p>
    <br />
</p>
<script>
    var reasonindex = ReasonArray.indexOf("<%=Reason%>");
    var abroad = document.getElementById('TempTestMeet.Reason');
    abroad.options[reasonindex].setAttribute("selected", "selected");
    document.getElementById('Reason' + reasonindex).style = "";
    //TempTestMeet.UserID
    var reasonindex = UserIDArray.indexOf("<%=UserID%>");
    console.log("<%=UserID%>");
    var UserID = document.getElementById('TempTestMeet.UserID');
    UserID.options[reasonindex].setAttribute("selected", "selected");
    //$.get("/ashx/getmeethandler.ashx", function (result) {
    //   var rJson = JSON.parse(result);
    //   console.log(rJson);
    //   var optionStr = "";
    //    for (var i = 0; i < rJson.length; i++) {
    //        optionStr += "<option value=\"" + rJson[i].id + "\">" + rJson[i].displayName + "</option>";
    //    }
    //    console.log(document.getElementById('TempTestMeet.college'));
    //   // document.getElementById('TempTestMeet.college').innerHTML = "";
    //    document.getElementById('TempTestMeet.college').applyElement = optionStr;
    //});
</script>
 <% if (DisplayModel == "0")
    { %>
<%}
    else
    { %>
 
<script>
  //  alert($('#TempTestMeet.Title').value());
    var inputs = document.getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].disabled = true;

    }
    var selects = document.getElementsByTagName('select');
    for (var i = 0; i < selects.length; i++) {
        selects[i].disabled = true;

    }
    document.getElementById("TempTestMeet.test").disabled = true;

</script>

<%} %>