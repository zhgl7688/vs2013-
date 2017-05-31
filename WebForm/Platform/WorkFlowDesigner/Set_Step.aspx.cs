using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowDesigner
{
    public partial class Set_Step : Common.BasePage
    {
        protected FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        protected FoWoSoft.Platform.DBConnection bdbConn = new FoWoSoft.Platform.DBConnection();
        protected FoWoSoft.Platform.WorkFlowButtons bworkFlowButtons = new FoWoSoft.Platform.WorkFlowButtons();
        protected FoWoSoft.Platform.AppLibrary bappLibrary = new FoWoSoft.Platform.AppLibrary();

        protected string appLibraryTypes = string.Empty;
        protected string stepID = string.Empty;
        protected string stepX = string.Empty;
        protected string stepY = string.Empty;
        protected string stepWidth = string.Empty;
        protected string stepHeight = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            appLibraryTypes = bappLibrary.GetTypeOptions();
            stepID = Request.QueryString["id"];
            stepX = Request.QueryString["x"];
            stepY = Request.QueryString["y"];
            stepWidth = Request.QueryString["width"];
            stepHeight = Request.QueryString["height"];
        }
    }
}