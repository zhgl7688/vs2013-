using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm
{
    public partial class Default : WebForm.Common.BasePage
    {
        EduWebService eduWebService = new EduWebService();
        protected int RoleLength;
        protected string DefaultRoleID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //  eduWebService.organizeResize(Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA"));
            //var ss = eduWebService.GetAll("ALL");
            //eduWebService.AddOrganize(ss);

            string loginMsg = string.Empty;
            if (!Common.Tools.CheckLogin(out loginMsg))
            {
                UserCheck();
                // Response.Redirect("Login.aspx");
                //  return;
            }



            #region 得到用户角色相关的信息

            FoWoSoft.Platform.UsersRole buserRole = new FoWoSoft.Platform.UsersRole();
            FoWoSoft.Platform.Role brole = new FoWoSoft.Platform.Role();
            var roles = buserRole.GetByUserID(FoWoSoft.Platform.Users.CurrentUserID);
            RoleLength = roles.Count;
            DefaultRoleID = string.Empty;
            string rolesOptions = string.Empty;
            if (roles.Count > 0)
            {
                var mainRole = roles.Find(p => p.IsDefault);
                DefaultRoleID = mainRole != null ? mainRole.RoleID.ToString() : roles.First().RoleID.ToString();
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

                rolesOptions = brole.GetRoleOptions("", "", roleList);
            }
            #endregion

            this.UserName.Text = CurrentUserName;
            this.OrganizeName.Text = CurrentUserOrganizeName;
            this.CurrentTime.Text = FoWoSoft.Utility.DateTimeNew.Now.ToDateWeekString();
            this.RoleOptions.Text = rolesOptions;

        }

        private void UserCheck()
        {
            if (Request.QueryString["userid"] == null)
                Response.Redirect("http://caslogin.ecnu.edu.cn/login.aspx?url=" + Request.Url.ToString() + "");

            string userid = Request.QueryString["userid"].ToString();//获取userid
            string token = Request.QueryString["token"].ToString();//获取token
            Session["userid"] = userid;
            Session["token"] = token;
            var ecnuws = new cn.edu.ecnu.datawebservice.ECNUWebService();
            DataSet tokends = ecnuws.CASLOGIN_Check(userid, token); //调用webservice接口，详细接口信息查看相关webservice接口文档
            if (tokends == null)
                Response.Redirect("http://caslogin.ecnu.edu.cn/login.aspx?url=" + Request.Url.ToString() + "&error=webservice出错, 请从网站登录页面登录");

            if (tokends.Tables[0].Rows[0][0].ToString().Contains("OK:"))
            {
                //此处为认证部分结束，由业务系统自定义代码，跳转其他页面
                //判断本地是否有用户
                var user = new WebForm.Common.UserService().CreateNewUser(userid);

                //保存用户信息
                FoWoSoft.Platform.OnlineUsers bou = new FoWoSoft.Platform.OnlineUsers();
                Guid uniqueID = Guid.NewGuid();
                Session[FoWoSoft.Utility.Keys.SessionKeys.UserID.ToString()] = user.ID;
                Session[FoWoSoft.Utility.Keys.SessionKeys.UserUniqueID.ToString()] = uniqueID;
                bou.Add(user, uniqueID);

            }
            else  //认证出错后继续跳转到统一身份认证页面加上ERROR参数
            {
                string url = Request.Url.ToString();
                Response.Redirect("http://caslogin.ecnu.edu.cn/login.aspx?url=" + url + "&error=" + tokends.Tables[0].Rows[0][0].ToString() + ",请从网站登录页面登录");
            }


        }



        protected override bool CheckUrl(bool isEnd = true)
        {
            return true;
        }

        protected override bool CheckLogin(bool isRedirect = true)
        {
            return true;
        }

    }
}