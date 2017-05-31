using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.MSSQL
{
    public class Guid_id : FoWoSoft.Data.Interface.IGuid_id
    {
        private DBHelper dbHelper = new DBHelper();
        /// <summary>
        /// 构造函数
        /// </summary>
        public Guid_id()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">FoWoSoft.Data.Model.Guid_id实体类</param>
        /// <returns>操作所影响的行数</returns>
        public int Add(FoWoSoft.Data.Model.Guid_id model)
        {
            string sql = @" INSERT INTO Guid_id (GuidId, useId) VALUES( @GuidId, @useId)";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@GuidId", SqlDbType.UniqueIdentifier, -1){ Value = model.GuidId },
				new SqlParameter("@useId", SqlDbType.VarChar, 500){ Value = model.useId },
				 	};
            return dbHelper.Execute(sql, parameters);
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(int id)
        {
            string sql = "SELECT id, GuidId, useId FROM Guid_id WHERE id=@ID";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID",SqlDbType.Int){ Value = id }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.Guid_id> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        /// <summary>
        /// 根据GuidId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(Guid GuidId)
        {
            string sql = "SELECT id, GuidId, useId FROM Guid_id WHERE GuidId=@GuidId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@GuidId",SqlDbType.UniqueIdentifier){ Value = GuidId }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.Guid_id> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(string useId)
        {
            string sql = "SELECT id, GuidId, useId FROM Guid_id WHERE useId=@useId";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@useId",SqlDbType.Char){ Value = useId }
			};
            SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<FoWoSoft.Data.Model.Guid_id> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<FoWoSoft.Data.Model.Guid_id> DataReaderToList(SqlDataReader dataReader)
        {
            List<FoWoSoft.Data.Model.Guid_id> List = new List<FoWoSoft.Data.Model.Guid_id>();
            FoWoSoft.Data.Model.Guid_id model = null;
            while (dataReader.Read())
            {
                model = new FoWoSoft.Data.Model.Guid_id();
                model.id = dataReader.GetInt32(0);
                model.GuidId = dataReader.GetGuid(1);
                model.useId = dataReader.GetString(2);
                List.Add(model);
            }
            return List;
        }
    }
}
