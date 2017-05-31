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
          
            CheckRequest(context);
 var meetInfo = new FoWoSoft.Data.Model.MeetInfo
            {
                     ApplicatId= context.Request["ApplicatId"],
                      MeetTimes= context.Request["MeetTimes"],
                       MeetId=context.Request["MeetId"],
                        MeetName= context.Request["MeetName"],
                         AdminId= context.Request["AdminId"],
            };
            string sql = @"UPDATE MeetInfo SET ApplicatId=@ApplicatId,  MeetTimes=@MeetTimes,
                          MeetName=@MeetName, AdminId=@AdminId WHERE MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ApplicatId",meetInfo.ApplicatId ),
				new SqlParameter("@MeetTimes",meetInfo.MeetTimes),
				new SqlParameter("@MeetName",meetInfo.MeetName),
				new SqlParameter("@AdminId",meetInfo.AdminId),
				new SqlParameter("@MeetId",  meetInfo.MeetId),
				 };
            var result = new DBHelper().Execute(sql, parameters);
            context.Response.Write(result);
            updateUser(meetInfo, result);
        }

        public void Add(HttpContext context)
        {
            CheckRequest(context);
            var meetInfo = new FoWoSoft.Data.Model.MeetInfo
            {
                ApplicatId = context.Request["ApplicatId"],
                MeetTimes = context.Request["MeetTimes"],
                MeetId = context.Request["MeetId"],
                MeetName = context.Request["MeetName"],
                AdminId = context.Request["AdminId"],
            };
         int result=   Create(meetInfo);
            context.Response.Write(result);
        }

        public int Create(  FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            //判断是否会议有重复
            if (MeetInfoRepeat(meetInfo.MeetId))
            {
              return(-1); 
            }

            string sql = @"INSERT INTO MeetInfo( ApplicatId ,  MeetTimes ,  MeetId ,   MeetName ,  AdminId    )
                      VALUES  ( @ApplicatId,@MeetTimes,@MeetId ,@MeetName,@AdminId   )";
            SqlParameter[] parameters = new SqlParameter[]{
					new SqlParameter("@ApplicatId",meetInfo.ApplicatId ),
				new SqlParameter("@MeetTimes",meetInfo.MeetTimes),
				new SqlParameter("@MeetName",meetInfo.MeetName),
				new SqlParameter("@AdminId",meetInfo.AdminId),
				new SqlParameter("@MeetId",  meetInfo.MeetId),
				 };
            int result = new DBHelper().Execute(sql, parameters);
          
            updateUser(meetInfo, result);

            CreateNewTempTestMeet(meetInfo);
            return result;
        }

        public void CreateNewTempTestMeet(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            Guid testMeetid = Guid.NewGuid();
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


        private static void updateUser(FoWoSoft.Data.Model.MeetInfo meetInfo , int result)
        {
            if (Convert.ToInt16(result) > 0)
            {
                Task.Factory.StartNew(() => new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId));
                Task.Factory.StartNew(() => new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId));

            }
        }
        private bool MeetInfoRepeat(string meetId)
        {
            string sql = @"select count(*)from MeetInfo where MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
				 	new SqlParameter("@MeetId",  meetId),
				 };
            var result = new DBHelper().ExecuteScalar(sql, parameters);
            return Convert.ToInt16(result) > 0 ? true : false;
        }
        private static void CheckRequest(HttpContext context)
        {
            if (context.Request["ApplicatId"] == null) { context.Response.Write("ApplicatId不能为空"); context.Response.End(); }
            if (context.Request["MeetTimes"] == null) { context.Response.Write("MeetTimes不能为空"); context.Response.End(); }
            if (context.Request["MeetName"] == null) { context.Response.Write("MeetName不能为空"); context.Response.End(); }
            if (context.Request["AdminId"] == null) { context.Response.Write("AdminId不能为空"); context.Response.End(); }
            if (context.Request["MeetId"] == null) { context.Response.Write("MeetId不能为空"); context.Response.End(); }
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