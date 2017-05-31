using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowDesigner
{
    public partial class Set_Flow : Common.BasePage
    {
        protected FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        protected FoWoSoft.Platform.DBConnection bdbConn = new FoWoSoft.Platform.DBConnection();
        protected string base_TypesOptions = string.Empty;
        protected string link_DBConnOptions = string.Empty;
        protected bool isAdd = false;
        protected string flowID = string.Empty;
        protected string defaultManager = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base_TypesOptions = bworkFlow.GetTypeOptions();
            link_DBConnOptions = bdbConn.GetAllOptions();
            isAdd = "1" == Request.QueryString["isadd"];
            flowID = Request.QueryString["flowid"].IsGuid() && !isAdd ? Request.QueryString["flowid"] : Guid.NewGuid().ToString();
            defaultManager = FoWoSoft.Platform.Users.PREFIX + FoWoSoft.Platform.Users.CurrentUserID.ToString();
        }
    }
}