<%@ Page Language="C#" %>

<%
    string FlowID = Request.QueryString["flowid"];
    string StepID = Request.QueryString["stepid"];
    string GroupID = Request.QueryString["groupid"];
    string TaskID = Request.QueryString["taskid"];
    string InstanceID = Request.QueryString["instanceid"];
    string DisplayModel = Request.QueryString["display"] ?? "0";
    string DBConnID = "06075250-30dc-4d32-bf97-e922cb30fac8";
    string DBTable = "TempTest_Purchase";
    string DBTablePK = "ID";
    string DBTableTitle = "Title";
    if (InstanceID.IsNullOrEmpty()) { InstanceID = Request.QueryString["instanceid1"]; } RoadFlow.Platform.Dictionary BDictionary = new RoadFlow.Platform.Dictionary();
    RoadFlow.Platform.WorkFlow BWorkFlow = new RoadFlow.Platform.WorkFlow();
    RoadFlow.Platform.WorkFlowTask BWorkFlowTask = new RoadFlow.Platform.WorkFlowTask();
    string fieldStatus = BWorkFlow.GetFieldStatus(FlowID, StepID);
    LitJson.JsonData initData = BWorkFlow.GetFormData(DBConnID, DBTable, DBTablePK, InstanceID, fieldStatus);
    string TaskTitle = BWorkFlow.GetFromFieldData(initData, DBTable, DBTableTitle);
%>
<link href="Scripts/Forms/flowform.css" rel="stylesheet" type="text/css" />
<script src="Scripts/Forms/common.js" type="text/javascript"></script>
<input type="hidden" id="Form_ValidateAlertType" name="Form_ValidateAlertType" value="2" />
<input type="hidden" id="Form_TitleField" name="Form_TitleField" value="TempTest_Purchase.Title" />
<input type="hidden" id="Form_DBConnID" name="Form_DBConnID" value="06075250-30dc-4d32-bf97-e922cb30fac8" />
<input type="hidden" id="Form_DBTable" name="Form_DBTable" value="TempTest_Purchase" />
<input type="hidden" id="Form_DBTablePk" name="Form_DBTablePk" value="ID" />
<input type="hidden" id="Form_DBTableTitle" name="Form_DBTableTitle" value="Title" />
<input type="hidden" id="Form_AutoSaveData" name="Form_AutoSaveData" value="1" />
<script type="text/javascript">
    var initData = <%=BWorkFlow.GetFormDataJsonString(initData)%>;
    var fieldStatus = "1"=="<%=Request.QueryString["isreadonly"]%>"? {} : <%=fieldStatus%>;
    var displayModel = '<%=DisplayModel%>';
    $(window).load(function (){
        formrun.initData(initData, "TempTest_Purchase", fieldStatus, displayModel);
    });
</script>
<p>
    <br />
</p>
<p>
    <br />
</p>
<p style="text-align: center;"><span style="font-size: 20px;"><strong>物资采购申请</strong></span><br />
</p>
<table class="flowformtable" uetable="null" data-sort="sortDisabled" cellpadding="0" cellspacing="1">
    <tbody>
        <tr class="firstRow">
            <td style="-ms-word-break: break-all;" valign="top" width="102">申请人：<br />
            </td>
            <td style="word-break: break-all;" valign="top" width="434">
                <input rootid="" unit="0" workgroup="0" user="1" station="0" dept="0" title="" class="mymember" isflow="1" type1="flow_org" id="TempTest_Purchase.UserID" name="TempTest_Purchase.UserID" value="u_<%=new RoadFlow.Platform.WorkFlowTask().GetFirstSnderID(FlowID.ToGuid(), GroupID.ToGuid(), true)%>" more="0" type="text" /></td>
            <td style="-ms-word-break: break-all;" valign="top" width="97">部门：</td>
            <td style="-ms-word-break: break-all;" valign="top" width="439">
                <input rootid="" unit="0" workgroup="0" user="0" station="0" dept="1" title="" class="mymember" isflow="1" type1="flow_org" id="TempTest_Purchase.UserDept" name="TempTest_Purchase.UserDept" value="<%=new RoadFlow.Platform.WorkFlowTask().GetFirstSnderDeptID(FlowID.ToGuid(), GroupID.ToGuid())%>" more="0" type="text" /></td>
        </tr>
        <tr>
            <td style="-ms-word-break: break-all;" valign="top" width="102">申请日期：<br />
            </td>
            <td style="-ms-word-break: break-all;" valign="top" width="434">
                <input title="" class="mycalendar" isflow="1" type1="flow_datetime" id="TempTest_Purchase.SqDateTime" name="TempTest_Purchase.SqDateTime" value="<%=RoadFlow.Utility.DateTimeNew.ShortDate%>" defaultvalue="<%=RoadFlow.Utility.DateTimeNew.ShortDate%>" istime="0" daybefor="0" dayafter="0" currentmonth="0" type="text" /></td>
            <td style="-ms-word-break: break-all;" valign="top" width="97">备注：</td>
            <td style="-ms-word-break: break-all;" valign="top" width="439">
                <input title="" class="mytext" isflow="1" id="TempTest_Purchase.Note" type1="flow_text" name="TempTest_Purchase.Note" value="" style="width: 80%" valuetype="0" type="text" /></td>
        </tr>
        <tr>
            <td style="word-break: break-all;" colspan="4" valign="top">申请明细：  </td>
        </tr>
        <tr>
            <td style="-ms-word-break: break-all;" colspan="4" valign="top">
                <table class="flowformsubtable" style="width: 99%; margin: 0 auto;" issubflowtable="1" id="subtable_TempTest_PurchaseList_ID_ID_PurchaseID" cellpadding="0" cellspacing="1">
                    <thead>
                        <tr>
                            <th>名称<input name="flowsubtable_id" value="TempTest_PurchaseList_ID_ID_PurchaseID" type="hidden" /><input name="flowsubtable_TempTest_PurchaseList_ID_ID_PurchaseID_secondtable" value="TempTest_PurchaseList" type="hidden" /><input name="flowsubtable_TempTest_PurchaseList_ID_ID_PurchaseID_primarytablefiled" value="ID" type="hidden" /><input name="flowsubtable_TempTest_PurchaseList_ID_ID_PurchaseID_secondtableprimarykey" value="ID" type="hidden" /><input name="flowsubtable_TempTest_PurchaseList_ID_ID_PurchaseID_secondtablerelationfield" value="PurchaseID" type="hidden" /></th>
                            <th>型号</th>
                            <th>单位</th>
                            <th>数量</th>
                            <th>日期</th>
                            <th>类型</th>
                            <th>备注</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr type1="listtr">
                            <td colname="TempTest_PurchaseList_Name" iscount="0">
                                <input name="hidden_guid_TempTest_PurchaseList_ID_ID_PurchaseID" value="51092f18fda58ec35e200ce287f0d2af" type="hidden" /><input name="flowsubid" value="TempTest_PurchaseList_ID_ID_PurchaseID" type="hidden" /><input class="mytext" issubflow="1" type1="subflow_text" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Name" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Name" colname="TempTest_PurchaseList_Name" style="width: 100px" value="" valuetype="0" type="text" /></td>
                            <td colname="TempTest_PurchaseList_Model" iscount="0">
                                <input class="mytext" issubflow="1" type1="subflow_text" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Model" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Model" colname="TempTest_PurchaseList_Model" style="width: 100px" value="" valuetype="0" type="text" /></td>
                            <td colname="TempTest_PurchaseList_Unit" iscount="0">
                                <input class="mytext" issubflow="1" type1="subflow_text" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Unit" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Unit" colname="TempTest_PurchaseList_Unit" style="width: 100px" value="" valuetype="0" type="text" /></td>
                            <td colname="TempTest_PurchaseList_Quantity" iscount="1">
                                <input class="mytext" issubflow="1" type1="subflow_text" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Quantity" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Quantity" colname="TempTest_PurchaseList_Quantity" style="width: 100px" value="" iscount="1" onblur="formrun.subtableCount('TempTest_PurchaseList_ID_ID_PurchaseID','TempTest_PurchaseList_Quantity','countspan_TempTest_PurchaseList_ID_ID_PurchaseID_TempTest_PurchaseList_Quantity');" valuetype="1" type="text" /></td>
                            <td colname="TempTest_PurchaseList_Date" iscount="0">
                                <input class="mycalendar" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Date" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Date" issubflow="1" type1="subflow_datetime" value="" colname="TempTest_PurchaseList_Date" style="width: 80px" type="text" /></td>
                            <td colname="TempTest_PurchaseList_Type" iscount="0">
                                <select class="myselect" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Type" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Type" issubflow="1" type1="subflow_select" colname="TempTest_PurchaseList_Type">
                                    <option value="办公用品">办公用品</option>
                                    <option value="办公家具">办公家具</option>
                                    <option value="耗材">耗材</option>
                                </select></td>
                            <td colname="TempTest_PurchaseList_Note" iscount="0">
                                <input class="mytext" issubflow="1" type1="subflow_text" name="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Note" id="TempTest_PurchaseList_ID_ID_PurchaseID_51092f18fda58ec35e200ce287f0d2af_TempTest_PurchaseList_Note" colname="TempTest_PurchaseList_Note" style="width: 100px" value="" valuetype="0" type="text" /></td>
                            <td>
                                <input class="mybutton" style="margin-right: 4px;" value="增加" onclick="formrun.subtableNewRow(this);" type="button" /><input class="mybutton" value="删除" onclick="    formrun.subtableDeleteRow(this);" type="button" /></td>
                        </tr>
                        <tr type1="counttr">
                            <td colspan="11" style="padding-right: 20px; text-align: right;" align="right"><span style="margin-right: 10px;">数量合计：<label id="countspan_TempTest_PurchaseList_ID_ID_PurchaseID_TempTest_PurchaseList_Quantity">0</label></span></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
<p>
    <br />
</p>
