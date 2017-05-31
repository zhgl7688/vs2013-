using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.Dictionary
{
    public partial class Add : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                FoWoSoft.Data.Model.Dictionary dict = new FoWoSoft.Data.Model.Dictionary();
                FoWoSoft.Platform.Dictionary bdict = new FoWoSoft.Platform.Dictionary();
                string id = Request.QueryString["id"];
                if (!id.IsGuid())
                {
                    var dictRoot = bdict.GetRoot();
                    id = dictRoot != null ? dictRoot.ID.ToString() : "";
                }
                if (!id.IsGuid())
                {
                    throw new Exception("未找到父级");
                }


                string title = Request.Form["Title1"];
                string code = Request.Form["Code"];
                string values = Request.Form["Values"];
                string note = Request.Form["Note"];
                string other = Request.Form["Other"];

                dict.ID = Guid.NewGuid();
                dict.Code = code.IsNullOrEmpty() ? null : code.Trim();
                dict.Note = note.IsNullOrEmpty() ? null : note.Trim();
                dict.Other = other.IsNullOrEmpty() ? null : other.Trim();
                dict.ParentID = id.ToGuid();
                dict.Sort = bdict.GetMaxSort(id.ToGuid());
                dict.Title = title.Trim();
                dict.Value = values.IsNullOrEmpty() ? null : values.Trim();

                bdict.Add(dict);
                bdict.RefreshCache();
                FoWoSoft.Platform.Log.Add("添加了数据字典项", dict.Serialize(), FoWoSoft.Platform.Log.Types.数据字典);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ok", "alert('添加成功!');parent.frames[0].reLoad('" + id + "');", true);

            }
        }
    }
}