using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm.ashx
{
    /// <summary>
    /// TaskFlowHandler 的摘要说明
    /// </summary>
    public class TaskFlowHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            // context.Response.Write("Hello World");
            var taskgid = context.Request["id"];
            var Model = context.Request["Model"];
            switch (Model)
            {
                case "status": GetStatus(context, taskgid); break;
                case "DelStatus": GetDelStatus(context, taskgid); break;
                case "DelTask": DelTask(context, taskgid); break;
                case "TaskCount": TaskCount(context); break; 
                case "SetUser": SetUser(context); break; 
                default:
                    break;
            }


        }

        private void SetUser(HttpContext context)
        {
            var userID =context.Request["userID"];
            var mobile = context.Request["mobile"];
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(mobile)) return;
            new EduWebService().SetUseinfo(userID, mobile);
        }

        private void TaskCount(HttpContext context)
        {
            var userID =Guid.Parse( context.Request["userID"]);
             string pager;
             var taskList = new FoWoSoft.Platform.WorkFlowTask().GetTasks(userID,  out pager);
         var  result=  taskList.Count>0 ?  "有"+taskList.Count.ToString()+"条待办":""  ;
       
            context.Response.Write(result);
        }

        private static void GetStatus(HttpContext context, string taskgid)
        {
            FoWoSoft.Platform.WorkFlowTask btask = new FoWoSoft.Platform.WorkFlowTask();
            var task = btask.Get(Guid.Parse(taskgid));
            if (task != null)
            {
                if (task.Status.In(2, 3, 4, 5))
                {
                    context.Response.Write("1");

                }
                else { context.Response.Write("0"); }
            }
        }
        private static void GetDelStatus(HttpContext context, string taskgid)
        {
            FoWoSoft.Platform.WorkFlowTask btask = new FoWoSoft.Platform.WorkFlowTask();
            var task = btask.Get(Guid.Parse(taskgid));
            if (task != null)
            {
                if (task.PrevID == Guid.Empty && task.Status.In(0,1))
                {
                    context.Response.Write("0");
                }
                else   
                {
                    context.Response.Write("1");

                }


            }
            else
            {
                context.Response.Write("0");
            }
        }
        private static void DelTask(HttpContext context, string taskgid)
        {
            FoWoSoft.Platform.WorkFlowTask btask = new FoWoSoft.Platform.WorkFlowTask();
            var task = btask.Get(Guid.Parse(taskgid));
            if (task != null)
            {
                btask.Delete(task.ID);
                new WebForm.Common.Meet().Roomis(task.InstanceID, WebForm.Common.RoomisOperation.put_reject);
 
            
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