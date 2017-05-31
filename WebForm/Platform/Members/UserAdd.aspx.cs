using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.Members
{
    public partial class UserAdd : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Account.Attributes.Add("validate_url", "CheckAccount.ashx");
            FoWoSoft.Platform.Organize borganize = new FoWoSoft.Platform.Organize();
            FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();

            string id = Request.QueryString["id"];

            string name = string.Empty;
            string account = string.Empty;
            string status = string.Empty;
            string note = string.Empty;
            Guid parentID;

            if (IsPostBack && id.IsGuid(out parentID))
            {
                name = Request.Form["Name"];
                account = Request.Form["Account"];
                status = Request.Form["Status"];
                note = Request.Form["Note"];

                Guid userID = Guid.NewGuid();
                string userXML = string.Empty;
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                {
                    //添加人员
                    FoWoSoft.Data.Model.Users user = new FoWoSoft.Data.Model.Users();
                    user.Account = account.Trim();
                    user.Name = name.Trim();
                    user.Note = note.IsNullOrEmpty() ? null : note;
                    user.Password = busers.GetUserEncryptionPassword(userID.ToString(), busers.GetInitPassword());
                    user.Sort = 1;
                    user.Status = status.IsInt() ? status.ToInt() : 0;
                    user.ID = userID;
                    busers.Add(user);

                    //添加关系
                    FoWoSoft.Data.Model.UsersRelation userRelation = new FoWoSoft.Data.Model.UsersRelation();
                    userRelation.IsMain = 1;
                    userRelation.OrganizeID = parentID;
                    userRelation.Sort = new FoWoSoft.Platform.UsersRelation().GetMaxSort(parentID);
                    userRelation.UserID = userID;
                    new FoWoSoft.Platform.UsersRelation().Add(userRelation);

                    //更新父级[ChildsLength]字段
                    borganize.UpdateChildsLength(parentID);

                    //更新角色
                    new FoWoSoft.Platform.UsersRole().UpdateByUserID(userID);

                    userXML = user.Serialize();
                    scope.Complete();
                }

                FoWoSoft.Platform.Log.Add("添加了人员", userXML, FoWoSoft.Platform.Log.Types.组织机构);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('添加成功!');parent.frames[0].reLoad('" + id + "');window.location=window.location;", true);
            }
            this.StatusRadios.Text = borganize.GetStatusRadio("Status", "0", "validate=\"radio\"");
        }
    }
}