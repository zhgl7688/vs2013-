using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowDesigner
{
    public partial class Open_List : Common.BasePage
    {
        protected FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
        protected FoWoSoft.Platform.Organize borg = new FoWoSoft.Platform.Organize();
        protected FoWoSoft.Platform.WorkFlow bwf = new FoWoSoft.Platform.WorkFlow();
        protected IEnumerable<FoWoSoft.Data.Model.WorkFlow> flows;
        protected string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            type = Request.QueryString["typeid"];
            string name = string.Empty;
            if (IsPostBack)
            {
                name = Request.Form["flow_name"];
            }

            flows = bwf.GetAll().Where(p => p.Status != 4);
            if (!name.IsNullOrEmpty())
            {
                flows = flows.Where(p => p.Name.IndexOf(name) >= 0);
            }
        }
    }
}