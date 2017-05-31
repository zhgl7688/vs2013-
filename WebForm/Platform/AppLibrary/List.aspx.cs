using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.AppLibrary
{
    public partial class List : Common.BasePage
    {
        protected List<FoWoSoft.Data.Model.AppLibrary> AppList = new List<FoWoSoft.Data.Model.AppLibrary>();
        protected string Query = string.Empty;
        protected string Query1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pager;
            string appid = Request.QueryString["appid"];
            string tabid = Request.QueryString["tabid"];
            string typeid = Request.QueryString["typeid"];
            string title1 = Request.QueryString["title1"];
            string address = Request.QueryString["address"];
            FoWoSoft.Platform.AppLibrary bapp = new FoWoSoft.Platform.AppLibrary();
            if (IsPostBack)
            {
                title1 = Request.Form["Title1"];
                address = Request.Form["Address"];
                //删除
                if (!Request.Form["Button1"].IsNullOrEmpty())
                {
                    string deleteID = Request.Form["checkbox_app"];
                    System.Text.StringBuilder delxml = new System.Text.StringBuilder();
                    foreach (string id in deleteID.Split(','))
                    {
                        Guid gid;
                        if (id.IsGuid(out gid))
                        {
                            var app = bapp.Get(gid);
                            if (app != null)
                            {
                                delxml.Append(app.Serialize());
                                bapp.Delete(gid);
                            }
                        }
                    }
                    FoWoSoft.Platform.Log.Add("删除了一批应用程序库", delxml.ToString(), FoWoSoft.Platform.Log.Types.角色应用);
                }
            }

            FoWoSoft.Platform.Dictionary bdict = new FoWoSoft.Platform.Dictionary();
           
            string typeidstring = typeid.IsGuid() ? bapp.GetAllChildsIDString(typeid.ToGuid()) : "";
            Query = string.Format("&appid={0}&tabid={1}&title1={2}&typeid={3}&address={4}",
                        Request.QueryString["appid"],
                        Request.QueryString["tabid"],
                        title1.UrlEncode(), typeid, address.UrlEncode()
                        );
            Query1 = string.Format("{0}&pagesize={1}&pagenumber={2}", Query, Request.QueryString["pagesize"], Request.QueryString["pagenumber"]);

            AppList = bapp.GetPagerData(out pager, Query, title1, typeidstring, address);
            this.Pager.Text = pager;
        }

    }
}