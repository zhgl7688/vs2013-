using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm
{
    public partial class Login1 : WebForm.Common.BasePage
    {
        protected string Script = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                check();
            }
        }

        protected override bool CheckApp()
        {
            return true;
        }

        protected override bool CheckUrl(bool isEnd = true)
        {
            return true;
        }

        protected override bool CheckLogin(bool isRedirect = true)
        {
            return true;
        }

        private void check()
        {
            string isVcodeSessionKey = FoWoSoft.Utility.Keys.SessionKeys.IsValidateCode.ToString();
            string vcodeSessionKey = FoWoSoft.Utility.Keys.SessionKeys.ValidateCode.ToString();

            string account = Request.Form["Account"];
            string password = Request.Form["Password"];
            string force = Request.Form["Force"];
            string vcode = Request.Form["VCode"];
            bool isSessionLost = "1" == Request.QueryString["session"];//是否是超时后再登录

            if (Session[isVcodeSessionKey] != null
                && "1" == Session[isVcodeSessionKey].ToString()
                && (Session[vcodeSessionKey] == null
                || string.Compare(Session[vcodeSessionKey].ToString(), vcode.Trim(), true) != 0))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "error", "alert('验证码错误!');", true);
            }
            else if (account.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                Session[isVcodeSessionKey] = "1";
                FoWoSoft.Platform.Log.Add("用户登录失败", string.Concat("用户:", account, "登录失败，帐号或密码为空"), FoWoSoft.Platform.Log.Types.用户登录);
                Script = "alert('帐号或密码不能为空!');";
            }
            else
            {
                FoWoSoft.Platform.Users busers = new FoWoSoft.Platform.Users();
                var user = busers.GetByAccount(account.Trim());
                if (user == null || string.Compare(user.Password, busers.GetUserEncryptionPassword(user.ID.ToString(), password.Trim()), false) != 0)
                {
                    Session[isVcodeSessionKey] = "1";
                    FoWoSoft.Platform.Log.Add("用户登录失败", string.Concat("用户:", account, "登录失败，帐号或密码错误"), FoWoSoft.Platform.Log.Types.用户登录);
                    Script = "alert('帐号或密码错误!');";
                }
                else if (user.Status == 1)
                {
                    Session[isVcodeSessionKey] = "1";
                    FoWoSoft.Platform.Log.Add("用户登录失败", string.Concat("用户:", account, "登录失败，帐号已被冻结"), FoWoSoft.Platform.Log.Types.用户登录);
                    Script = "alert('帐号已被冻结!');";
                }
                else
                {
                    FoWoSoft.Platform.OnlineUsers bou = new FoWoSoft.Platform.OnlineUsers();
                    var onUser = bou.Get(user.ID);
                    if (onUser != null && "1" != force)
                    {
                        string ip = onUser.IP;
                        Session.Remove(isVcodeSessionKey);
                        Script = "if(confirm('当前帐号已经在" + ip + "登录,您要强行登录吗?')){$('#Account').val('" + account + "');$('#Password').val('" + password + "');$('#Force').val('1');$('#form1').submit();}";
                    }
                    else
                    {
                        Guid uniqueID = Guid.NewGuid();
                        Session[FoWoSoft.Utility.Keys.SessionKeys.UserID.ToString()] = user.ID;
                        Session[FoWoSoft.Utility.Keys.SessionKeys.UserUniqueID.ToString()] = uniqueID;
                        bou.Add(user, uniqueID);
                        Session.Remove(isVcodeSessionKey);
                        FoWoSoft.Platform.Log.Add("用户登录成功", string.Concat("用户:", user.Name, "(", user.ID, ")登录成功"), FoWoSoft.Platform.Log.Types.用户登录);
                        if (isSessionLost)
                        {
                            Script = "alert('登录成功!');new RoadUI.Window().close();";
                        } 
                        else
                        {
                            Script = "top.location='" + Common.Tools.BaseUrl + "Default.aspx';";
                        }
                    }
                }
            }
        }
    }
}