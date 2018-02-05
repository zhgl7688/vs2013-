using FoWoSoft.Data.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebForm.EduModels;

namespace WebForm.ashx
{
    /// <summary>
    /// ROOMISHandler 的摘要说明
    /// 申请人职工号：ApplicatId
    //时间 MeetTimes
    //会议室 MeetId
    //会议室名称 MeetName
    //管理员职工号：AdminId
    /// </summary>
    public class ROOMISHandler : IHttpHandler
    {
        FoWoSoft.Platform.MeetInfo meetInfoService = new FoWoSoft.Platform.MeetInfo();
        HttpContext context;
        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            var model = context.Request["model"].ToString();
            switch (model)
            {
                case "add": Add(context); break;
                case "modify": Modify(context); break;
                case "del": Delete(context); break;
                case "get": Get(context); break;
                default:
                    break;
            }
            // context.Response.ContentType = "text/plain";

        }

        private void Get(HttpContext context)
        {
            new FoWoSoft.Platform.MeetInfo().Get(context.Request["Account"]);
        }

        private void Delete(HttpContext context)
        {
            if (context.Request["temp1"] == null) { context.Response.Write("temp1不能为空"); context.Response.End(); }
            var result = delbytemp1(context.Request["temp1"]);
            context.Response.Write(result);
        }
        public int delbytemp1(string temp1)
        {
            return meetInfoService.DeleteByMeetId(temp1);
        }

        private void Modify(HttpContext context)
        {
            var meetInfo = CheckRequest(context);
            var result = RoomisModify(meetInfo);
            context.Response.Write(result);
        }
        public int RoomisModify(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {

            var result = meetInfoService.ModifyByTemp1(meetInfo);
            if (result > 0)
            {
                var meetinfot = meetInfoService.GetByTemp1(meetInfo.temp1);
                if (meetinfot != null)
                {
                    var tempTestMeet = Common.Tools.MeetInfoToTempTestMeet(meetInfo, meetinfot.temp3);
                    new TempTestMeet().RoomisModify(tempTestMeet);
                }
            }
            return result;
        }
        public void Add(HttpContext context)
        {
            context.Response.Write(Create(CheckRequest(context)));
        }

        public int Create(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            if (meetInfoService.MeetInfoRepeat(meetInfo.temp1)) return -1;

            var result = meetInfoService.Create(meetInfo);
            if (result > 0)
                CreateNewTempTestMeet(meetInfo);

            return result;

        }

        public void CreateNewTempTestMeet(FoWoSoft.Data.Model.MeetInfo meetInfot)
        {
            MeetInfoModel meetInfo = meetInfot as MeetInfoModel;
            string testMeetid;
            meetInfoService.RoomisUpdate(meetInfo, out testMeetid);
            var tempmeet = Common.Tools.MeetInfoToTempTestMeet(meetInfo, testMeetid);
            new FoWoSoft.Platform.TempTestMeet().RoomisAdd(tempmeet);

            var aplicatUser = new FoWoSoft.Platform.Users().GetByAccount(meetInfo.ApplicatId);
            var adminUser = new FoWoSoft.Platform.Users().GetByAccount(meetInfo.AdminId);
            var task = new FoWoSoft.Data.Model.WorkFlowTask
            {
                InstanceID = testMeetid,
                Title = meetInfo.temp2,

                SenderID = aplicatUser.ID,
                SenderName = aplicatUser.Name,
                ReceiveID = adminUser.ID,
                ReceiveName = adminUser.Name
            };
            new FoWoSoft.Platform.WorkFlowTask().RoomisCreate(task);

        }


        private FoWoSoft.Data.Model.MeetInfo CheckRequest(HttpContext context)
        {
            string[] contextStrs = new string[] { "ApplicatId", "MeetTimes", "MeetName", "AdminId", "MeetId", "temp1" };

            for (int i = 0; i < contextStrs.Length; i++)
            {
                if (context.Request[contextStrs[i]] == null)
                {
                    context.Response.Write(contextStrs[i] + "不能为空");
                    context.Response.End();
                }

            }
            var ApplicatIds = context.Request["ApplicatId"].Split('|');

            var meetInfo = new EduModels.MeetInfoModel
            {

                MeetTimes = context.Request["MeetTimes"],
                MeetId = context.Request["MeetId"],
                MeetName = context.Request["MeetName"],
                AdminId = context.Request["AdminId"],
                temp1 = context.Request["temp1"],
                temp2 = context.Request["temp2"] ?? "",
                Date1 = context.Request["Date1"] == null ? DateTime.Now : Convert.ToDateTime(context.Request["Date1"]),
                test1 = context.Request["test1"] ?? "",
                test = context.Request["test"] ?? "",
                typeid = context.Request["typeid"] ?? "",
                type = context.Request["type"] ?? "",
                Reason = context.Request["Reason"] ?? "",
                inland = context.Request["inland"] ?? "",
                abroad = context.Request["abroad"] ?? "",
            };
            meetInfo.ApplicatId = ApplicatIds[0];
            meetInfo.Phone = ApplicatIds.Length > 1 ? ApplicatIds[1] : "";
            meetInfo.Address = ApplicatIds.Length > 2 ? ApplicatIds[2] : "";
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            if (adminUser == null) { context.Response.Write("管理员不存在！"); context.Response.End(); };
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            if (aplicatUser == null) { context.Response.Write("申请人不存在！"); context.Response.End(); };
            return meetInfo;
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