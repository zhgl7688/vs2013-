using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm
{
    /// <summary>
    /// Test1 的摘要说明
    /// </summary>
    public class Test1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string title = context.Request.QueryString["title"];
            string all = context.Request.QueryString["all"];
            if (title != null)
            {
 var ss = new WebForm.EduWebService().GetUser(title);
            if (ss!=null)
            context.Response.Write("{\"count\":" + ss.BMBH + ",\"data\":\"" + ss.BMMC + "\"}");
           else context.Response.Write(string.Format("没有{0}账号信息",title));
            }
            else if (all!=null)
            {
                new WebForm.Common.UserService().CreateUser(all);
            }

           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}