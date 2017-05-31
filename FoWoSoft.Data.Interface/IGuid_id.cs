using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace  FoWoSoft.Data.Interface
{
   public interface IGuid_id
    {

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">FoWoSoft.Data.Model.Guid_id实体类</param>
        /// <returns>操作所影响的行数</returns>
         int Add(FoWoSoft.Data.Model.Guid_id model);
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
         FoWoSoft.Data.Model.Guid_id Get(int id);
        /// <summary>
        /// 根据GuidId查询一条记录
        /// </summary>
         FoWoSoft.Data.Model.Guid_id Get(Guid GuidId);
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
         FoWoSoft.Data.Model.Guid_id Get(string useId);
      
    }
}
