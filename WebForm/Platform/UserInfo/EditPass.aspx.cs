using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.UserInfo
{
    public partial class EditPass : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string oldpass = Request.Form["oldpass"];
                string newpass = Request.Form["newpass"];

                FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
                var user = FoWoSoft.Platform.Users.CurrentUser;
                if (user != null)
                {
                    if (string.Compare(user.Password, busers.GetUserEncryptionPassword(user.ID.ToString(), oldpass.Trim()), false) != 0)
                    {
                        FoWoSoft.Platform.Log.Add("修改密码失败", string.Concat("用户：", user.Name, "(", user.ID, ")修改密码失败,旧密码错误!"), FoWoSoft.Platform.Log.Types.用户登录);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('旧密码错误!');", true);
                    }
                    else
                    {
                        busers.UpdatePassword(newpass.Trim(), user.ID);
                        FoWoSoft.Platform.Log.Add("修改密码成功", string.Concat("用户：", user.Name, "(", user.ID, ")修改密码成功!"), FoWoSoft.Platform.Log.Types.用户登录);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('密码修改成功!');new RoadUI.Window().close();", true);
                    }
                }
            }
        }
    }
}