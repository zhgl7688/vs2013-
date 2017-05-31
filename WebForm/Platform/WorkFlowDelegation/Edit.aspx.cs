using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowDelegation
{
    public partial class Edit : Common.BasePage
    {
        protected bool isOneSelf = false;
        protected string FlowOptions = string.Empty;
        protected FoWoSoft.Platform.WorkFlowDelegation bworkFlowDelegation = new FoWoSoft.Platform.WorkFlowDelegation();
        protected FoWoSoft.Data.Model.WorkFlowDelegation workFlowDelegation = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];

            string UserID = string.Empty;
            string ToUserID = string.Empty;
            string StartTime = string.Empty;
            string EndTime = string.Empty;
            string FlowID = string.Empty;
            string Note = string.Empty;

            isOneSelf = "1" == Request.QueryString["isoneself"];

            Guid delegationID;
            if (id.IsGuid(out delegationID))
            {
                workFlowDelegation = bworkFlowDelegation.Get(delegationID);
                if (workFlowDelegation != null)
                {
                    FlowID = workFlowDelegation.FlowID.ToString();
                }
            }
            string oldXML = workFlowDelegation.Serialize();

            if (IsPostBack)
            {
                UserID = Request.Form["UserID"];
                ToUserID = Request.Form["ToUserID"];
                StartTime = Request.Form["StartTime"];
                EndTime = Request.Form["EndTime"];
                FlowID = Request.Form["FlowID"];
                Note = Request.Form["Note"];

                bool isAdd = !id.IsGuid();
                if (workFlowDelegation == null)
                {
                    workFlowDelegation = new FoWoSoft.Data.Model.WorkFlowDelegation();
                    workFlowDelegation.ID = Guid.NewGuid();
                }
                workFlowDelegation.UserID = isOneSelf ? FoWoSoft.Platform.Users.CurrentUserID : FoWoSoft.Platform.Users.RemovePrefix(UserID).ToGuid();
                workFlowDelegation.EndTime = EndTime.ToDateTime();
                if (FlowID.IsGuid())
                {
                    workFlowDelegation.FlowID = FlowID.ToGuid();
                }
                workFlowDelegation.Note = Note.IsNullOrEmpty() ? null : Note;
                workFlowDelegation.StartTime = StartTime.ToDateTime();
                workFlowDelegation.ToUserID = FoWoSoft.Platform.Users.RemovePrefix(ToUserID).ToGuid();
                workFlowDelegation.WriteTime = FoWoSoft.Utility.DateTimeNew.Now;

                if (isAdd)
                {
                    bworkFlowDelegation.Add(workFlowDelegation);
                    FoWoSoft.Platform.Log.Add("添加了工作委托", workFlowDelegation.Serialize(), FoWoSoft.Platform.Log.Types.流程相关);
                }
                else
                {
                    bworkFlowDelegation.Update(workFlowDelegation);
                    FoWoSoft.Platform.Log.Add("修改了工作委托", "", FoWoSoft.Platform.Log.Types.流程相关, oldXML, workFlowDelegation.Serialize());
                }
                bworkFlowDelegation.RefreshCache();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('保存成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();", true);
            }
            FlowOptions = new FoWoSoft.Platform.WorkFlow().GetOptions(FlowID);
            if (workFlowDelegation == null) workFlowDelegation = new FoWoSoft.Data.Model.WorkFlowDelegation();
        }
    }
}