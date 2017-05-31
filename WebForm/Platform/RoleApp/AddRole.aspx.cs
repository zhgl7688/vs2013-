using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.RoleApp
{
    public partial class AddRole : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string name = Request.Form["Name"];
                string note = Request.Form["Note"];
                string useMember = Request.Form["UseMember"];
                FoWoSoft.Data.Model.Role role = new FoWoSoft.Data.Model.Role();
                using (System.Transactions.TransactionScope trans = new System.Transactions.TransactionScope())
                {
                    role.ID = Guid.NewGuid();
                    role.Name = name.Trim();
                    if (!useMember.IsNullOrEmpty())
                    {
                        role.UseMember = useMember;
                        FoWoSoft.Platform.UsersRole busersRole = new FoWoSoft.Platform.UsersRole();
                        var users = new FoWoSoft.Platform.Organize().GetAllUsers(useMember);
                        foreach (var user in users)
                        {
                            FoWoSoft.Data.Model.UsersRole ur = new FoWoSoft.Data.Model.UsersRole();
                            ur.IsDefault = true;
                            ur.MemberID = user.ID;
                            ur.RoleID = role.ID;
                            busersRole.Add(ur);
                        }
                    }
                    if (!note.IsNullOrEmpty())
                    {
                        role.Note = note.Trim();
                    }
                    new FoWoSoft.Platform.Role().Add(role);

                    //添加一个根应用
                    FoWoSoft.Data.Model.RoleApp roleApp = new FoWoSoft.Data.Model.RoleApp();
                    roleApp.ID = Guid.NewGuid();
                    roleApp.ParentID = Guid.Empty;
                    roleApp.RoleID = role.ID;
                    roleApp.Sort = 1;
                    roleApp.Title = "管理目录";
                    new FoWoSoft.Platform.RoleApp().Add(roleApp);
                    trans.Complete();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('添加成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();", true);
                }

            }
        }
    }
}