using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm
{
    public partial class Test : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new WebForm.Common.UserService().CreateAllUser();
        }

        protected override bool CheckApp()
        {
            return true;//return base.CheckApp();
        }
        protected override bool CheckUrl(bool isEnd = true)
        {
            return true; //base.CheckUrl(isEnd);
        }
        protected override bool CheckLogin(bool isRedirect = true)
        {
            return true; //base.CheckLogin(isRedirect);
        }
    }
}