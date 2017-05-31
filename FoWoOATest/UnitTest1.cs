using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using FoWoSoft.Data.MSSQL;
using System.Web;
using System.IO;
using System.Text;

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
                var meetId = 222;
                string sql = @"select count(*) from MeetInfo where MeetId=@MeetId";
              SqlParameter[] parameters = new SqlParameter[]{
				 	new SqlParameter("@MeetId",  222),
				 };
              var result = new DBHelper().ExecuteScalar(sql, parameters);
               var ss=Convert.ToInt16( result)>0 ? true : false;
               Assert.IsTrue(ss);
           // }
        }
        [TestMethod]
        public void TestAdd()
        {
            
            var meetinfo = new FoWoSoft.Data.Model.MeetInfo
            {
                MeetId = "200000021",
                AdminId = "gy",
                MeetName = "100会议",
                MeetTimes = "2017-09-20 11:20",
                ApplicatId = "20121102"
            };
            // new WebForm.ashx.ROOMISHandler().CreateNewTempTestMeet(meetinfo);
            new WebForm.ashx.ROOMISHandler().Create(meetinfo );
        }
    }
}
