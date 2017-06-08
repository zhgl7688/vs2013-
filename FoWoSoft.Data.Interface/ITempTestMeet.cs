using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Interface
{
   public interface ITempTestMeet
    {

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">FoWoSoft.Data.Model.Guid_id实体类</param>
        /// <returns>操作所影响的行数</returns>
       
        /// <summary>
       /// 根据主键查询一条记录
        /// </summary>
       FoWoSoft.Data.Model.TempTestMeet Get(Guid GuidId);
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
       FoWoSoft.Data.Model.TempTestMeet Get(string useId);
        int RoomisModify(FoWoSoft.Data.Model.MeetInfo meetInfo);
        
        }
}
