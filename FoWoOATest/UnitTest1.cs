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
using FoWoSoft.Data.Model;

namespace FoWoOATest
{
    [TestClass]
    public class UnitTest1
    {
        WebForm.EduModels.MeetInfoModel meetInfo;
       public UnitTest1()
        {
              meetInfo = new WebForm.EduModels.MeetInfoModel
            {
                MeetId = "200000022",
                AdminId = "20121102",
                MeetName = "100会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102",
                temp1 = "39",
                temp2 = "1715",
                Date1 = DateTime.Now,
                test1 = "1",
                test = "2",
                typeid = "哲学社会科学类的讲座",
                type = "校内视频会议室",
                Reason = "校内",
                inland = "",
                abroad = "",
                Phone = "777777777",
                Address = "地址7"
            };
        }
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
             new WebForm.Common.UserService().CreateAllUser();
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
            var id = "0301"; 
            // var id = "42017271204";
            new WebForm.EduWebService().ss(id);

            // new WebForm.EduWebService().GetAllUserByDPCODE(id);
            //Assert.IsNotNull(user);
            //Assert.AreEqual("", user);
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

            meetInfo.temp1 = DateTime.Now.ToShortTimeString();
            meetInfo.typeid = Enum.GetName(typeof(meetType), meetType.直播会议);
            meetInfo.type = Enum.GetName(typeof(LivePlatform), LivePlatform.华师大直播平台);
            meetInfo.inland = "入场设备信息";
            meetInfo.abroad = "222.222.33.44|其他需求";
            meetInfo.Reason = "校内|是";
            meetInfo.temp2 = Enum.GetName(typeof(meetType), meetType.直播会议) + DateTime.Now.ToShortTimeString() + "标题";
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            Assert.IsNotNull(adminUser);
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            Assert.IsNotNull(aplicatUser);
          // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().Create(meetInfo);
            System.Threading.Thread.Sleep(1000);
            meetInfo.temp1 = DateTime.Now.ToShortTimeString();
            meetInfo.typeid = Enum.GetName(typeof(meetType), meetType.直播会议);
            meetInfo.type = Enum.GetName(typeof(LivePlatform), LivePlatform.校外第三方直播平台);
            meetInfo.inland = "入场设备信息";
            meetInfo.abroad = "222.222.33.44|其他需求";
            meetInfo.Reason = "校内|是";
            meetInfo.temp2 = Enum.GetName(typeof(meetType), meetType.直播会议) + DateTime.Now.ToShortTimeString() + "标题";
              new WebForm.ashx.ROOMISHandler().Create(meetInfo);
        }


        [TestMethod]
        public void TestRoomisModify()
        {

            var meetInfo = new WebForm.EduModels.MeetInfoModel
            {
                MeetId = "200000022",
                AdminId = "gy",
                MeetName = "100会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102",
                temp1 = "1740",
                temp2 = "会议名称",
                Date1 = DateTime.Now,
                test1 = "11",
                test = "12",
                typeid = "哲学社会科学类的讲座",
                type = "14",
                Reason = "15",
                inland = "16",
                abroad = "17",
                Phone = "9999",
                Address = "地址1111"
            };
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            Assert.IsNotNull(adminUser);
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            Assert.IsNotNull(aplicatUser);
            // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().RoomisModify(meetInfo);
        }
        [TestMethod]
        public void delTest()
        {
            string temp1 = "17:41";
            var result = new FoWoSoft.Platform.MeetInfo().DeleteByMeetId(temp1);
            Assert.AreEqual(0, result);

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
        [TestMethod]
        public void TestLiveAdd()
        {

            var meetInfo = new WebForm.EduModels.MeetInfoModel
            {
                MeetId = "200000022",
                AdminId = "20121102",
                MeetName = "直播会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102",
                temp1 = "37",
                temp2 = "直播会议001",
                Date1 = DateTime.Now,
                test1 = "1",
                test = "2",
                typeid = "直播会议",
                type = "华师大直播平台",
                Reason = "校内|是",
                inland = "入场设备信息",
                abroad = "11.22.33.44|other",
                Phone = "777777777",
                Address = "地址7"
            };
            var adminUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.AdminId);
            Assert.IsNotNull(adminUser);
            var aplicatUser = new WebForm.Common.UserService().CreateNewUser(meetInfo.ApplicatId);
            Assert.IsNotNull(aplicatUser);
            // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().Create(meetInfo);
        }
    }
    enum testen
    {
        green, red, black
    }
    
}
