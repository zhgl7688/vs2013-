﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Platform.WorkFlowTasks
{
    public partial class CompletedList : Common.BasePage
    {
        protected FoWoSoft.Platform.WorkFlowTask bworkFlowTask = new FoWoSoft.Platform.WorkFlowTask();
        protected FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        protected List<FoWoSoft.Data.Model.WorkFlowTask> taskList = new List<FoWoSoft.Data.Model.WorkFlowTask>();
        protected string query = string.Empty;
        protected void Page_Load(object sender1, EventArgs e)
        {
            string title = "";
            string flowid = "";
            string sender = "";
            string date1 = "";
            string date2 = "";

            if (IsPostBack)
            {
                title = Request.Form["Title1"];
                flowid = Request.Form["FlowID"];
                sender = Request.Form["SenderID"];
                date1 = Request.Form["Date1"];
                date2 = Request.Form["Date2"];
            }
            else
            {
                title = Request.QueryString["title"];
                flowid = Request.QueryString["flowid"];
                sender = Request.QueryString["sender"];
                date1 = Request.QueryString["date1"];
                date2 = Request.QueryString["date2"];
            }

            string query2 = string.Format("&appid={0}&tabid={1}&title={2}&flowid={3}&sender={4}&date1={5}&date2={6}",
                Request.QueryString["appid"], Request.QueryString["tabid"], title.UrlEncode(), flowid, sender, date1, date2
                );

            query = string.Format("{0}&pagesize={1}&pagenumber={2}",
                query2,
                Request.QueryString["pagesize"], Request.QueryString["pagenumber"]
                );

            string pager;
           var taskList1 = bworkFlowTask.GetTasks(FoWoSoft.Platform.Users.CurrentUserID,
               out pager, query2, title, flowid, sender, date1, date2, 1);
            foreach (var item in taskList1)
            {

                if (taskList.Exists(s=>s.InstanceID==item.InstanceID)) continue;
                taskList.Add(item);
            }
            this.Pager.Text = pager;
            this.flowOptions.Text = bworkFlow.GetOptions(flowid);

        }
    }
}