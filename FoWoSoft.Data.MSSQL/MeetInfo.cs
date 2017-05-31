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
        public Model.MeetInfo Get(string applicatId)
        {
            string sql = "SELECT Id, ApplicatId, MeetTimes, MeetId, MeetName, AdminId from  MeetInfo WHERE ApplicatId=@ApplicatId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ApplicatId",SqlDbType.VarChar){ Value = applicatId }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        public List< Model.MeetInfo> GetAll(   )
        {
            string sql = "SELECT Id, ApplicatId, MeetTimes, MeetId, MeetName, AdminId from  MeetInfo ";
            
            SqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<FoWoSoft.Data.Model.MeetInfo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List : null;
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
                List.Add(model);
            }
            return List;
        }
    }
}
