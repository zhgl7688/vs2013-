using FoWoSoft.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.MSSQL
{
    public class TempTestMeet : ITempTestMeet
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public TempTestMeet()
        {
        }
        public Model.TempTestMeet Get(Guid GuidId)
        {
            string sql = "SELECT * FROM TempTestMeet WHERE ID=@GuidId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@GuidId",SqlDbType.UniqueIdentifier){ Value = GuidId }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.TempTestMeet> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        public Model.TempTestMeet Get(string Title)
        {
            string sql = "SELECT * FROM Guid_id WHERE Title=@Title";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@Title",SqlDbType.Char){ Value = Title }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.TempTestMeet> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<FoWoSoft.Data.Model.TempTestMeet> DataReaderToList(SqlDataReader dataReader)
        {
            List<FoWoSoft.Data.Model.TempTestMeet> List = new List<FoWoSoft.Data.Model.TempTestMeet>();
            FoWoSoft.Data.Model.TempTestMeet model = null;
            while (dataReader.Read())
            {
                model = new FoWoSoft.Data.Model.TempTestMeet();

                model.ID = dataReader.GetGuid(0);
                if (!dataReader.IsDBNull(1)) model.Title = dataReader.GetString(1); else model.Title = "";
                if (!dataReader.IsDBNull(2)) model.UserID = dataReader.GetString(2); else model.UserID = "";
                if (!dataReader.IsDBNull(3)) model.UserID_text = dataReader.GetString(3); else model.UserID_text = "";
                if (!dataReader.IsDBNull(4)) model.DeptID = dataReader.GetString(4); else model.DeptID = "";
                if (!dataReader.IsDBNull(5)) model.DeptName = dataReader.GetString(5); else model.DeptName = "";
                if (!dataReader.IsDBNull(6)) model.Date1 = dataReader.GetDateTime(6);
                if (!dataReader.IsDBNull(7)) model.Date2 = dataReader.GetDateTime(7);
                if (!dataReader.IsDBNull(8)) model.Type = dataReader.GetString(8); else model.Type = "";
                if (!dataReader.IsDBNull(9)) model.Reason = dataReader.GetString(9); else model.Reason = "";
              //  if (!dataReader.IsDBNull(10)) model.WriteTime = dataReader.GetString(10); else model.WriteTime = "";
                if (!dataReader.IsDBNull(11)) model.test = dataReader.GetString(11); else model.test = "";
                if (!dataReader.IsDBNull(12)) model.test1 = dataReader.GetString(12); else model.test1 = "";
                if (!dataReader.IsDBNull(13)) model.abroad = dataReader.GetString(13); else model.abroad = "";
                if (!dataReader.IsDBNull(14)) model.inland = dataReader.GetString(14); else model.inland = "";
                if (!dataReader.IsDBNull(15)) model.college = dataReader.GetString(15); else model.college = "";
                if (!dataReader.IsDBNull(16)) model.test2_text = dataReader.GetString(16); else model.test2_text = "";
                if (!dataReader.IsDBNull(17)) model.flowcompleted = dataReader.GetInt32(17); else model.flowcompleted = 0;
               
                List.Add(model);
            }
            return List;
        }


    }
}
