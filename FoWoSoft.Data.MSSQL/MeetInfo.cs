using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.MSSQL
{
    public class MeetInfo : FoWoSoft.Data.Interface.IMeetInfo
    {
        private DBHelper dbHelper = new DBHelper();
        const string selectfileds = "SELECT Id, ApplicatId, MeetTimes, MeetId, MeetName, AdminId,temp1,temp2,temp3 from  MeetInfo ";
        public bool MeetInfoRepeat(string temp1)
        {
            string sql = @"select count(*)from MeetInfo where temp1=@temp1";
            SqlParameter[] parameters = new SqlParameter[]{
                     new SqlParameter("@temp1",  temp1),
                 };
            var result = new DBHelper().ExecuteScalar(sql, parameters);
            return Convert.ToInt16(result) > 0 ? true : false;
        }
        public Model.MeetInfo Get(string applicatId)
        {
            string sql = selectfileds+"  WHERE ApplicatId=@ApplicatId";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@ApplicatId",SqlDbType.VarChar){ Value = applicatId }
            };
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        public Model.MeetInfo GetByMeetId(string meetId)
        {
            string sql =selectfileds+ "  WHERE MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@MeetId",SqlDbType.VarChar){ Value = meetId }
            };
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        public Model.MeetInfo GetByTemp1(string temp1)
        {
            string sql = selectfileds+"   WHERE temp1=@temp1";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@temp1",SqlDbType.VarChar){ Value = temp1 }
            };
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        public List<Model.MeetInfo> GetAll()
        {
            string sql = selectfileds;

            SqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List : null;
        }
        public int Create(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            string sql = @"INSERT INTO MeetInfo( ApplicatId ,  MeetTimes ,  MeetId ,   MeetName ,  AdminId,temp1 ,temp2   )
                      VALUES  ( @ApplicatId,@MeetTimes,@MeetId ,@MeetName,@AdminId ,@temp1 ,@temp2 )";
            SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@ApplicatId",meetInfo.ApplicatId ),
                new SqlParameter("@MeetTimes",meetInfo.MeetTimes),
                new SqlParameter("@MeetName",meetInfo.MeetName),
                new SqlParameter("@AdminId",meetInfo.AdminId),
                new SqlParameter("@MeetId",  meetInfo.MeetId),
                    new SqlParameter("@temp1",  meetInfo.temp1),
                    new SqlParameter("@temp2",  meetInfo.temp2),
                 };
            return new DBHelper().Execute(sql, parameters);
        }
        public int ModifyByTemp1(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            string sql = @"UPDATE MeetInfo SET ApplicatId=@ApplicatId,  MeetTimes=@MeetTimes,
                          MeetName=@MeetName, AdminId=@AdminId, MeetId=@MeetId,temp2=@temp2 WHERE temp1=@temp1";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@ApplicatId",meetInfo.ApplicatId ),
                new SqlParameter("@MeetTimes",meetInfo.MeetTimes),
                new SqlParameter("@MeetName",meetInfo.MeetName),
                new SqlParameter("@AdminId",meetInfo.AdminId),
                new SqlParameter("@MeetId",  meetInfo.MeetId),
                new SqlParameter("@temp1",  meetInfo.temp1),
                 new SqlParameter("@temp2",  meetInfo.temp2),
                 };
            return new DBHelper().Execute(sql, parameters);

        }
        public int DeleteByMeetId(string meetId)
        {
            string sql = @"DELETE MeetInfo   WHERE MeetId=@MeetId";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@MeetId",   meetId),
                 };
            return new DBHelper().Execute(sql, parameters);

        }
        public int RoomisUpdate(FoWoSoft.Data.Model.MeetInfo meetInfo ,out string testMeetid)
        {
             testMeetid = Guid.NewGuid().ToString();
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
            return new DBHelper().Execute(sql, parameters);

        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<FoWoSoft.Data.Model.MeetInfo> DataReaderToList(SqlDataReader dataReader)
        {
            List<FoWoSoft.Data.Model.MeetInfo> List = new List<FoWoSoft.Data.Model.MeetInfo>();
            FoWoSoft.Data.Model.MeetInfo model = null;
            while (dataReader.Read())
            {
                model = new FoWoSoft.Data.Model.MeetInfo();
                model.Id = dataReader.GetInt32(0);
                model.ApplicatId = dataReader.GetString(1);
                model.MeetTimes = dataReader.GetDateTime(2).ToString();
                model.MeetId = dataReader.GetString(3);
                model.MeetName = dataReader.GetString(4);
                model.AdminId = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    model.temp1 = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    model.temp2 = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    model.temp3 = dataReader.GetString(8);
                List.Add(model);
            }
            return List;
        }
    }
}
