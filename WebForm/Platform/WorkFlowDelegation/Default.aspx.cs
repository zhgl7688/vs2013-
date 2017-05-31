using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowDelegation
{
    public partial class Default : Common.BasePage
    {
        protected FoWoSoft.Platform.Organize borganize = new FoWoSoft.Platform.Organize();
        protected FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
        protected FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        protected IEnumerable<FoWoSoft.Data.Model.WorkFlowDelegation> workFlowDelegationList;
        protected string Query1 = string.Empty;
        protected bool isoneself = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            isoneself = "1" == Request.QueryString["isoneself"];
            if (isoneself)
            {
                this.S_UserID.Disabled = true;
                this.S_UserID.Value = FoWoSoft.Platform.Users.PREFIX + FoWoSoft.Platform.Users.CurrentUserID.ToString();
            }
            FoWoSoft.Platform.WorkFlowDelegation bworkFlowDelegation = new FoWoSoft.Platform.WorkFlowDelegation();
            FoWoSoft.Platform.Organize borganize = new FoWoSoft.Platform.Organize();
            FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
            FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
            
            string startTime = string.Empty;
            string endTime = string.Empty;
            string suserid = string.Empty;
            string Query1 = string.Format("&appid={0}&tabid={1}&isoneself={2}", Request.QueryString["appid"], 
                Request.QueryString["tabid"], Request.QueryString["isoneself"]);
            if (IsPostBack)
            {
                if (!Request.Form["DeleteBut"].IsNullOrEmpty())
                {
                    string ids = Request.Form["checkbox_app"];
                    foreach (string id in ids.Split(','))
                    {
                        Guid bid;
                        if (!id.IsGuid(out bid))
                        {
                            continue;
                        }
                        var comment = bworkFlowDelegation.Get(bid);
                        if (comment != null)
                        {
                            bworkFlowDelegation.Delete(bid);
                            FoWoSoft.Platform.Log.Add("删除了流程意见", comment.Serialize(), FoWoSoft.Platform.Log.Types.流程相关);
                        }
                    }
                    bworkFlowDelegation.RefreshCache();
                }
                startTime = Request.Form["S_StartTime"];
                endTime = Request.Form["S_EndTime"];
                suserid = Request.Form["S_UserID"];
            }
            else
            {
                startTime = Request.QueryString["S_StartTime"];
                endTime = Request.QueryString["S_EndTime"];
                suserid = Request.QueryString["S_UserID"];
            }
            Query1 += "&S_StartTime=" + startTime + "&S_EndTime=" + endTime + "&S_UserID=" + suserid;
            string pager;
            bool isOneSelf = "1" == Request.QueryString["isoneself"];
            
            if (isOneSelf)
            {
                workFlowDelegationList = bworkFlowDelegation.GetPagerData(out pager, Query1, FoWoSoft.Platform.Users.CurrentUserID.ToString(), startTime, endTime);
            }
            else
            {
                workFlowDelegationList = bworkFlowDelegation.GetPagerData(out pager, Query1, FoWoSoft.Platform.Users.RemovePrefix(suserid), startTime, endTime);
            }
            this.Pager.Text = pager;
        }
    }
}