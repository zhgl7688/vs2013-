﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
            string orgina = context.Request.QueryString["orgina"];
            string guid = context.Request.QueryString["guid"];
            if (guid!=null){
                context.Response.ContentType = "text/json";
                var ss =JsonConvert.SerializeObject( new FoWoSoft.Platform.Guid_id().GetAll());

                context.Response.Write (ss);
            }else
            if (title != null)
            {
                var ss = new WebForm.EduWebService().GetUser(title);
                if (ss != null)
                    context.Response.Write("{\"BMBH\":" + ss.BMBH + ",\"BMMC\":\"" + ss.BMMC + "\"}");
                else context.Response.Write(string.Format("没有{0}账号信息", title));
            }
            else if (all != null)
            {
                if (all=="ok")
                {
new WebForm.Common.UserService().CreateAllUser();
                    context.Response.Write("all");
                }
                    
                else
                new WebForm.Common.UserService().CreateUser(all);
            }
            else if (orgina != null)

            {
                Guid first = Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA");
                var rist = new FoWoSoft.Platform.Organize().GetChilds(first)[0];
                new EduWebService().organizeResize1("04F12BEB-D99D-43DF-AC9A-3042957D6BDA", first, 1);
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