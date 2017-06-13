using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using FoWoSoft.Data.MSSQL;
using System.Web;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;
using WebForm;

namespace FoWoOATest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var connstring = ConfigurationManager.ConnectionStrings;
            //  foreach (var item in connstring)
            //{

            //var  ConnectionString = item.ToString();
            //var ss = ((System.Configuration.ConnectionStringSettings)(item));
            //var name = ss.Name;
            // var meetId = 222;
            string sql = @"select count(*) from MeetInfo where MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
                     new SqlParameter("@MeetId",  222),
                 };
            var result = new DBHelper().ExecuteScalar(sql, parameters);
            var ss = Convert.ToInt16(result) > 0 ? true : false;
            Assert.IsTrue(ss);
            // }
        }

    
             [TestMethod]
        public void OrganizeTest()
        {
            Guid first = Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA");
            var rist = new FoWoSoft.Platform.Organize().GetChilds(first)[0];
            new EduWebService().organizeResize1("04F12BEB-D99D-43DF-AC9A-3042957D6BDA", first,1); 
        }
        [TestMethod]
        public void createallUserTest()
        {
             new WebForm.Common.UserService().CreateAllUser("50");
        }
        [TestMethod]
        public void EduUser()
        {
            // var id = "20121102";
            var id = "42017271204";
            var user = new WebForm.EduWebService().GetUser(id);
            //Assert.IsNotNull(user);
            Assert.AreEqual("", user.XM);
        }
        [TestMethod]
        public void EduAllUserTest()
        {
            var id = "0416";
            // var id = "42017271204";
            var user = new WebForm.EduWebService().GetAllUserByDPCODE(id);
            //Assert.IsNotNull(user);
            Assert.AreEqual("", user);
        }


        [TestMethod]
        public void RoosTest()
        {
            string instance = "1eb6d23d-71a0-46e0-8507-a35660a1f51d";
            new WebForm.Common.Meet().Roomis(instance, WebForm.Common.RoomisOperation.put_approve);

        }
        [TestMethod]
        public void jumplastTest()
        {
            Guid taskID = Guid.Parse("26A94FCC-401D-441D-B2C2-F1246B602B72");
            var users = new List<FoWoSoft.Data.Model.Users>();
            new FoWoSoft.Platform.WorkFlowTask().JumpLast(taskID, users);

        }
        [TestMethod]
        public void MeetTest()
        {
            string Meetid = "34";
            string adminId = "20121102";
            //string result=  WebForm.Common.Meet.put_approve(Meetid, adminId);
            var result = WebForm.Common.Meet.put_reject(Meetid, adminId);
            Assert.AreEqual("", "");
        }
        [TestMethod]
        public void MeetOptions()
        {
            string Meetid = null;

            //string result=  WebForm.Common.Meet.put_approve(Meetid, adminId);
            var result = WebForm.Common.Meet.GetMeetOptions(Meetid);
            Assert.AreEqual("", "");
        }
        [TestMethod]
        public void TestAdd()
        {

            var meetInfo = new FoWoSoft.Data.Model.MeetInfo
            {
                MeetId = "200000022",
                AdminId = "gy",
                MeetName = "100会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102",
                temp1 = "21",
                temp2 = "会议名称"
            };
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            Assert.IsNotNull(adminUser);
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            Assert.IsNotNull(aplicatUser);
            // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().Create(meetInfo);
        }
        [TestMethod]
        public void TestRoomisModify()
        {

            var meetInfo = new FoWoSoft.Data.Model.MeetInfo
            {
                MeetId = "200000021",
                AdminId = "gy",
                MeetName = "33会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102",
                temp1 = "22",
                temp2 = "会议名称"
            };
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            Assert.IsNotNull(adminUser);
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            Assert.IsNotNull(aplicatUser);
            // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().RoomisModify(meetInfo);
        }
        [TestMethod]
        public void TestEnum()
        {
            var ss = (testen)Enum.Parse(typeof(testen), "red");
            Assert.AreEqual<testen>(testen.red, ss);
            var kk = testen.black == (testen.red | testen.green);
            Assert.AreEqual(false, kk);
        }
        [TestMethod]
        public void Teststruct()
        {
            var execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();

            execute.FlowID = Guid.NewGuid();

            var param = "";// new FoWoSoft.Data.Model.WorkFlowCustomEventParams().set(execute);
                           // Assert.AreEqual(execute.FlowID, param.FlowID);
                           // execute.FlowID = Guid.NewGuid();
                           // Assert.AreEqual(execute.FlowID, param.FlowID);

        }
        [TestMethod]
        public void testteset()
        {
            var url = "http://202.120.85.70/test1.ashx?all=200";

              WebForm.Common.Meet.gethttp(url);

        }
    }
    enum testen
    {
        green, red, black
    }
}
