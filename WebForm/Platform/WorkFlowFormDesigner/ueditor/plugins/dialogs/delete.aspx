<%@ Page Language="C#" %>
<% 
    WebForm.Common.Tools.CheckLogin();
    string id = Request.QueryString["id"];
    if(!id.IsGuid())
    {
        Response.Write("参数错误!");
        Response.End();
    }
    FoWoSoft.Platform.WorkFlowForm WFF = new FoWoSoft.Platform.WorkFlowForm();
    
    var wff = WFF.Get(id.ToGuid());
    
    if(wff==null)
    {
        Response.Write("参数错误!");
        Response.End();
    }
    wff.Status = 2;
    WFF.Update(wff);

    //FoWoSoft.Platform.AppLibrary APP = new FoWoSoft.Platform.AppLibrary();
    //var app = APP.GetByCode(id);
    //if(app!=null)
    //{
    //    APP.Delete(app.ID);
    //}

    FoWoSoft.Platform.Log.Add("删除了流程表单", wff.Serialize(), FoWoSoft.Platform.Log.Types.流程相关);

    Response.Write("1");
    Response.End();
%>