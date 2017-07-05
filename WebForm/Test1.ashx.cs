using System;
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
                var guids = new FoWoSoft.Platform.Guid_id().GetAll();
                var ss = JsonConvert.SerializeObject(guids);

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
        //public string get_gs_xml(string contractno, string AddtimeBegin, string AddtimeEnd)
        //{

        //    var Entities = new gsEntities1().gs_xml_hyjy.OrderByDescending(m => m.Addtime).ThenBy(m => m.contractno).
        //           Select(l => new Models_gs_xml_hyjy() { contractno = l.contractno,
        //               AlterPlanCode = l.AlterPlanCode,
        //               AlterLevel = l.AlterLevel, 
        //               PegAndSite = l.PegAndSite, 
        //               AlterItemName = l.AlterItemName,
        //               AlterEstimatAmount = l.AlterEstimatAmount, 
        //               Addtime = l.Addtime,
        //               Addtime1=l.Addtime.tostring("yyyy-MM-dd")
        //           });


        //    if (contractno != null)
        //    {
        //        Entities = Entities.Where(o => o.contractno == contractno);
        //    }

        //    if (AddtimeBegin != null)
        //    {
        //        DateTime Begin = GMT2Local(AddtimeBegin);
        //        Entities = Entities.Where(o => o.Addtime >= Begin);
        //    }

        //    if (AddtimeEnd != null)
        //    {
        //        DateTime End = GMT2Local(AddtimeEnd);
        //        Entities = Entities.Where(o => o.Addtime < End);
        //    }
            
        //    foreach (var item in Entities.ToList())
        //    {
        //        item.Addtime = Convert.ToDateTime(item.Addtime.ToShortDateString().ToString());
        //    }
        //    return XmlSerializer(Entities.ToList());
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}