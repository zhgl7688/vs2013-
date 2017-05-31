using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.RoleApp
{
    public partial class Sort : Common.BasePage
    {
        protected List<FoWoSoft.Data.Model.RoleApp> RoleAppList = new List<FoWoSoft.Data.Model.RoleApp>();
        protected void Page_Load(object sender, EventArgs e)
        {
            FoWoSoft.Platform.RoleApp broleApp = new FoWoSoft.Platform.RoleApp();
            string id = Request.QueryString["id"];
            var roleApp = broleApp.Get(id.ToGuid());
            //RoleAppList = broleApp.GetChild(roleApp.ParentID);

            if (IsPostBack)
            {
                string srots = Request.Form["sortapp"];
                if (srots.IsNullOrEmpty())
                {
                    return;
                }
                string[] sortArray = srots.Split(new char[] { ',' });
                for (int i = 0; i < sortArray.Length; i++)
                {
                    Guid guid;
                    if (!sortArray[i].IsGuid(out guid))
                    {
                        continue;
                    }
                    broleApp.UpdateSort(guid, i + 1);
                }
                broleApp.ClearAllDataTableCache();
                string rid = roleApp.ParentID.ToString();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "parent.frames[0].reLoad('" + rid + "');", true);
                
            }
            RoleAppList = broleApp.GetChild(roleApp.ParentID);
        }
    }
}