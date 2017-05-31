using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.UserApp
{
    public partial class AddApp : Common.BasePage
    {
        protected string AppTypesOptions = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            FoWoSoft.Platform.AppLibrary bappLibrary = new FoWoSoft.Platform.AppLibrary();
            FoWoSoft.Platform.UsersApp busersApp = new FoWoSoft.Platform.UsersApp();
            FoWoSoft.Platform.RoleApp broleApp = new FoWoSoft.Platform.RoleApp();
            FoWoSoft.Data.Model.UsersApp usersApp = null;

            string id = Request.QueryString["id"];
            string userID = Request.QueryString["userid"];
            string roleID = Request.QueryString["roleid"];

            if (IsPostBack && id.IsGuid() && userID.IsGuid())
            {
                usersApp = busersApp.Get(id.ToGuid());
                if (!Request.Form["Save"].IsNullOrEmpty())
                {
                    string name = Request.Form["Name"];
                    string type = Request.Form["Type"];
                    string appid = Request.Form["AppID"];
                    string params1 = Request.Form["Params"];
                    string ico = Request.Form["Ico"];

                    FoWoSoft.Data.Model.UsersApp usersApp1 = new FoWoSoft.Data.Model.UsersApp();

                    usersApp1.ID = Guid.NewGuid();
                    usersApp1.ParentID = id.ToGuid();
                    usersApp1.Title = name.Trim();
                    usersApp1.Sort = broleApp.GetMaxSort(id.ToGuid());
                    usersApp1.UserID = userID.ToGuid();
                    usersApp1.RoleID = roleID.IsGuid() ? roleID.ToGuid() : Guid.Empty;
                    if (appid.IsGuid())
                    {
                        usersApp1.AppID = appid.ToGuid();
                    }
                    else
                    {
                        usersApp1.AppID = null;
                    }
                    usersApp1.Params = params1.IsNullOrEmpty() ? null : params1.Trim();
                    if (!ico.IsNullOrEmpty())
                    {
                        usersApp1.Ico = ico;
                    }

                    busersApp.Add(usersApp1);
                    busersApp.ClearCache();
                    FoWoSoft.Platform.Log.Add("添加了个人应用", busersApp.Serialize(), FoWoSoft.Platform.Log.Types.角色应用);
                    string refreshID = id;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('添加成功!'); parent.frames[0].reLoad('" + refreshID + "')", true);
                }
            }

            AppTypesOptions = bappLibrary.GetTypeOptions();
        }
    }
}