<%@ Page Language="C#"%>
<%
	string FlowID = Request.QueryString["flowid"];
	string StepID = Request.QueryString["stepid"];
	string GroupID = Request.QueryString["groupid"];
	string TaskID = Request.QueryString["taskid"];
	string InstanceID = Request.QueryString["instanceid"];
	string DisplayModel = Request.QueryString["display"] ?? "0";
	string DBConnID = "06075250-30dc-4d32-bf97-e922cb30fac8";
	string DBTable = "TempTest";
	string DBTablePK = "ID";
	string DBTableTitle = "Title";
if(InstanceID.IsNullOrEmpty()){InstanceID = Request.QueryString["instanceid1"];}
FoWoSoft.Platform.Dictionary BDictionary = new FoWoSoft.Platform.Dictionary();
	FoWoSoft.Platform.WorkFlow BWorkFlow = new FoWoSoft.Platform.WorkFlow();
	FoWoSoft.Platform.WorkFlowTask BWorkFlowTask = new FoWoSoft.Platform.WorkFlowTask();
	string fieldStatus = BWorkFlow.GetFieldStatus(FlowID, StepID);
	LitJson.JsonData initData = BWorkFlow.GetFormData(DBConnID, DBTable, DBTablePK, InstanceID, fieldStatus);
	string TaskTitle = BWorkFlow.GetFromFieldData(initData, DBTable, DBTableTitle);
%>
<link href="Scripts/Forms/flowform.css" rel="stylesheet" type="text/css" />
<script src="Scripts/Forms/common.js" type="text/javascript" ></script>
<input type="hidden" id="Form_ValidateAlertType" name="Form_ValidateAlertType" value="1" />
<input type="hidden" id="Form_TitleField" name="Form_TitleField" value="TempTest.Title" />
<input type="hidden" id="Form_DBConnID" name="Form_DBConnID" value="06075250-30dc-4d32-bf97-e922cb30fac8" />
<input type="hidden" id="Form_DBTable" name="Form_DBTable" value="TempTest" />
<input type="hidden" id="Form_DBTablePk" name="Form_DBTablePk" value="ID" />
<input type="hidden" id="Form_DBTableTitle" name="Form_DBTableTitle" value="Title" />
<input type="hidden" id="Form_AutoSaveData" name="Form_AutoSaveData" value="1" />
<script type="text/javascript">
	var initData = <%=BWorkFlow.GetFormDataJsonString(initData)%>;
	var fieldStatus = "1"=="<%=Request.QueryString["isreadonly"]%>"? {} : <%=fieldStatus%>;
	var displayModel = '<%=DisplayModel%>';
    $(window).load(function (){
       
		formrun.initData(initData, "TempTest", fieldStatus, displayModel);
		 
	});
</script>
<p><br/></p><p> </p><p style="text-align: center;"><strong><span style="font-size: 24px;">视 频 会 议<br/></span></strong></p><p> </p><table class="flowformtable" data-sort="sortDisabled" cellspacing="1" cellpadding="0"><tbody><tr class="firstRow"><td style="word-break: break-all;" width="23" valign="top"><span style="font-size:16px;font-family:宋体">活动名称</span>：</td><td colspan="3" width="941" valign="top"><input id="TempTest.Title" type1="flow_text" name="TempTest.Title" value="<%=FoWoSoft.Platform.Users.CurrentUserName%>的请假申请" style="width:80%" isflow="1" class="mytext" title="" type="text"/><script type="text/javascript">function onchange_31414dcceba792ae7555cf547c5f2469(srcObj){alert("adfasdfddd2222222sadf");}</script></td></tr><tr><td style="word-break: break-all;" width="122" valign="top"><span style="font-size:16px;font-family:宋体">测试时间</span>：<br/></td><td style="ms-word-break: break-all;" rowspan="1" colspan="3" valign="top"><input type1="flow_datetime" id="TempTest.Date1" name="TempTest.Date1" value="" defaultvalue="" istime="0" daybefor="0" dayafter="0" currentmonth="0" isflow="1" class="mycalendar" title="" type="text"/></td></tr><tr><td style="word-break: break-all;" width="133" valign="top"><span style="font-size:16px;font-family:宋体">正式会议时间</span>：<br/></td><td style="word-break: break-all;" rowspan="1" colspan="3" valign="top"><input type1="flow_datetime" id="TempTest.Date2" name="TempTest.Date2" value="" defaultvalue="" istime="0" daybefor="0" dayafter="1" currentmonth="0" isflow="1" class="mycalendar" title="" type="text"/></td></tr></tbody></table><p> </p><p><br/></p>
