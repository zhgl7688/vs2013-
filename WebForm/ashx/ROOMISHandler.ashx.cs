using FoWoSoft.Data.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
        public void ProcessRequest(HttpContext context)
        {
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
            if (context.Request["MeetId"] == null) { context.Response.Write("MeetId不能为空"); context.Response.End(); }
            string sql = @"DELETE MeetInfo   WHERE MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@MeetId",  context.Request["MeetId"]),
				 };
            var result = new DBHelper().Execute(sql, parameters);
            context.Response.Write(result);
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
            updateUser(meetInfo, result);
            var meetinfo = meetInfoService.GetByTemp1(meetInfo.temp1);
            if (meetinfo != null)
            {
                meetInfo.temp3 = meetinfo.temp3;
                ModifyTempTestMeet(meetInfo);
            }

            return result;
        }
        public void Add(HttpContext context)
        {
            context.Response.Write(Create(CheckRequest(context)));
        }

        public int Create(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            //判断是否会议有重复
            if (meetInfoService.MeetInfoRepeat(meetInfo.temp1)) return -1;

            var result = meetInfoService.Create(meetInfo);

            updateUser(meetInfo, result);

            CreateNewTempTestMeet(meetInfo);
            return result;
        }

        public void CreateNewTempTestMeet(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            Guid testMeetid = Guid.NewGuid();
            new DBHelper().Execute("update meetInfo set temp3='" + testMeetid.ToString().ToUpper() + "' where temp1=" + meetInfo.temp1);
            string sql = @"INSERT INTO TempTestMeet( ID , Title , Date2 ,Reason ,college )
VALUES  (   @ID , @Title , @Date2 ,@Reason ,@college   )";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID",  testMeetid),
				new SqlParameter("@Title",meetInfo.MeetName+"视频会议" ),
				new SqlParameter("@Date2",meetInfo.MeetTimes),
				new SqlParameter("@Reason", "校内"),
				new SqlParameter("@college",  meetInfo.MeetId),
				 };
            var user = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            var result = new DBHelper().Execute(sql, parameters);
            sql = @"INSERT INTO WorkFlowTask(ID, PrevID, PrevStepID, FlowID, StepID, StepName, InstanceID, GroupID, Type, Title, SenderID, SenderName, SenderTime, ReceiveID, ReceiveName, ReceiveTime,     Status,  Sort )
VALUES  (   @ID ,'00000000-0000-0000-0000-000000000000' ,'00000000-0000-0000-0000-000000000000' ,'2EA49481-3EE2-4805-AB85-53604D696976','307552C3-547D-445B-9C6B-E42737568B10','发起视频会议',
@testMeetId, @GroupID ,'0',@Title,@SenderID,@SenderName,GETDATE(), @SenderID,@SenderName,GETDATE(),  0 ,1)";
            parameters = new SqlParameter[]{
				new SqlParameter("@ID",   Guid.NewGuid()),
				new SqlParameter("@testMeetId",testMeetid ),
				new SqlParameter("@GroupID",Guid.NewGuid() ),
				new SqlParameter("@Title", meetInfo.MeetName+"视频会议" ),
				new SqlParameter("@SenderID", user.ID),
				new SqlParameter("@SenderName", user.Name)  };
            result = new DBHelper().Execute(sql, parameters);
        }

        public void ModifyTempTestMeet(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            string sql = @"update TempTestMeet set Date2=@Date2,college=@college where id=@id ";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@Date2",meetInfo.MeetTimes),
				new SqlParameter("@college",  meetInfo.MeetId),
                new SqlParameter("@id",  meetInfo.temp3),
				 };
            var user = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            var result = new DBHelper().Execute(sql, parameters);

        }
        private static void updateUser(FoWoSoft.Data.Model.MeetInfo meetInfo, int result)
        {
            if (Convert.ToInt16(result) > 0)
            {
                Task.Factory.StartNew(() => new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId));
                Task.Factory.StartNew(() => new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId));

            }
        }

        private FoWoSoft.Data.Model.MeetInfo CheckRequest(HttpContext context)
        {
            if (context.Request["ApplicatId"] == null) { context.Response.Write("ApplicatId不能为空"); context.Response.End(); }
            if (context.Request["MeetTimes"] == null) { context.Response.Write("MeetTimes不能为空"); context.Response.End(); }
            if (context.Request["MeetName"] == null) { context.Response.Write("MeetName不能为空"); context.Response.End(); }
            if (context.Request["AdminId"] == null) { context.Response.Write("AdminId不能为空"); context.Response.End(); }
            if (context.Request["MeetId"] == null) { context.Response.Write("MeetId不能为空"); context.Response.End(); }
            if (context.Request["temp1"] == null) { context.Response.Write("temp1不能为空"); context.Response.End(); }

            var meetInfo = new FoWoSoft.Data.Model.MeetInfo
            {
                ApplicatId = context.Request["ApplicatId"],
                MeetTimes = context.Request["MeetTimes"],
                MeetId = context.Request["MeetId"],
                MeetName = context.Request["MeetName"],
                AdminId = context.Request["AdminId"],
                temp1 = context.Request["temp1"],
                temp2 = context.Request["temp2"] ?? "",
            };
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