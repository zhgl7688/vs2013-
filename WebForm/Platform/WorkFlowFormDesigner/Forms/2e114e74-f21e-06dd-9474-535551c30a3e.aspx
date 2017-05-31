<%@ Page Language="C#"%>
<%
	string FlowID = Request.QueryString["flowid"];
	string StepID = Request.QueryString["stepid"];
	string GroupID = Request.QueryString["groupid"];
	string TaskID = Request.QueryString["taskid"];
	string InstanceID = Request.QueryString["instanceid"];
	string DisplayModel = Request.QueryString["display"] ?? "0";
	string DBConnID = "06075250-30dc-4d32-bf97-e922cb30fac8";
	string DBTable = "TempTestMeet";
	string DBTablePK = "";
	string DBTableTitle = "Title";
if(InstanceID.IsNullOrEmpty()){InstanceID = Request.QueryString["instanceid1"];}	RoadFlow.Platform.Dictionary BDictionary = new RoadFlow.Platform.Dictionary();
	RoadFlow.Platform.WorkFlow BWorkFlow = new RoadFlow.Platform.WorkFlow();
	RoadFlow.Platform.WorkFlowTask BWorkFlowTask = new RoadFlow.Platform.WorkFlowTask();
	string fieldStatus = BWorkFlow.GetFieldStatus(FlowID, StepID);
	LitJson.JsonData initData = BWorkFlow.GetFormData(DBConnID, DBTable, DBTablePK, InstanceID, fieldStatus);
	string TaskTitle = BWorkFlow.GetFromFieldData(initData, DBTable, DBTableTitle);
%>
<link href="Scripts/Forms/flowform.css" rel="stylesheet" type="text/css" />
<script src="Scripts/Forms/common.js" type="text/javascript" ></script>
<input type="hidden" id="Form_ValidateAlertType" name="Form_ValidateAlertType" value="1" />
<input type="hidden" id="Form_TitleField" name="Form_TitleField" value="TempTestMeet.Title" />
<input type="hidden" id="Form_DBConnID" name="Form_DBConnID" value="06075250-30dc-4d32-bf97-e922cb30fac8" />
<input type="hidden" id="Form_DBTable" name="Form_DBTable" value="TempTestMeet" />
<input type="hidden" id="Form_DBTablePk" name="Form_DBTablePk" value="" />
<input type="hidden" id="Form_DBTableTitle" name="Form_DBTableTitle" value="Title" />
<input type="hidden" id="Form_AutoSaveData" name="Form_AutoSaveData" value="1" />
<script type="text/javascript">
	var initData = <%=BWorkFlow.GetFormDataJsonString(initData)%>;
	var fieldStatus = "1"=="<%=Request.QueryString["isreadonly"]%>"? {} : <%=fieldStatus%>;
	var displayModel = '<%=DisplayModel%>';
	$(window).load(function (){
		formrun.initData(initData, "TempTestMeet", fieldStatus, displayModel);
	});
</script>
<p> </p><p> </p><p style="text-align: center;"><strong><span style="font-size: 20px;">报 告 请 示</span></strong></p><p>  </p><table class="flowformtable" cellspacing="1" cellpadding="0"><tbody><tr class="firstRow"><td style="-ms-word-break: break-all;" width="79" valign="top" height="16">标题：<br/></td><td width="1038" valign="top" height="16"><input id="TempTestMeet.Title" type1="flow_text" name="TempTestMeet.Title" value="<%=RoadFlow.Platform.Users.CurrentUserName%>的报告请示" style="width:70%" maxlength="200" valuetype="0" isflow="1" class="mytext" title="" type="text"/></td></tr><tr><td style="-ms-word-break: break-all;" width="79" valign="top" height="9">内容：<br/></td><td width="1038" valign="top" height="9"><textarea isflow="1" type1="flow_textarea" id="TempTestMeet.test" name="TempTestMeet.test" class="mytext" style="width:70%;height:100px" maxlength="1000"></textarea></td></tr></tbody></table><p> </p>