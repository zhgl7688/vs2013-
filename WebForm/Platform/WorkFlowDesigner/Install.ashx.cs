using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebForm.Platform.WorkFlowDesigner
{
    /// <summary>
    /// Install 的摘要说明
    /// </summary>
    public class Install : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string json = context.Request.Form["json"];
            string msg = new FoWoSoft.Platform.WorkFlow().InstallFlow(json, false);
            FoWoSoft.Platform.Log.Add("安装了流程", json + "(" + msg + ")", FoWoSoft.Platform.Log.Types.流程相关);
            context.Response.Write(msg);
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