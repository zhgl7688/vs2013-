using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.UserApp
{
    public partial class Tree : Common.BasePage
    {
        protected string RoleOptions = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = Request.QueryString["id"];
            FoWoSoft.Platform.Role brole = new FoWoSoft.Platform.Role();
            var roles = new FoWoSoft.Platform.UsersRole().GetByUserID(userid.ToGuid());
            List<FoWoSoft.Data.Model.Role> roleList = new List<FoWoSoft.Data.Model.Role>();
            foreach (var role in roles)
            {
                var role1 = brole.Get(role.RoleID);
                if (role1 == null)
                {
                    continue;
                }
                roleList.Add(role1);
            }

            RoleOptions = new FoWoSoft.Platform.Role().GetRoleOptions(Request.QueryString["roleid"], "", roleList);

        }
    }
}