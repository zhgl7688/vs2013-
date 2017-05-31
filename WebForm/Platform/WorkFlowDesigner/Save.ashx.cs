using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebForm.Platform.WorkFlowDesigner
{
    /// <summary>
    /// Save 的摘要说明
    /// </summary>
    public class Save : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string json = context.Request.Form["json"];
            string msg = new FoWoSoft.Platform.WorkFlow().SaveFlow(json);
            FoWoSoft.Platform.Log.Add("保存了流程", json + "(" + msg + ")", FoWoSoft.Platform.Log.Types.流程相关);
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