using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Platform
{
   public class TempTestMeet
    {
         private FoWoSoft.Data.Interface.ITempTestMeet dataTempTestMeet;
         public TempTestMeet()
        {
            this.dataTempTestMeet = Data.Factory.Factory.GetTempTestMeet();
        }

 
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
         public FoWoSoft.Data.Model.TempTestMeet Get(Guid GuidId)
        {
            return dataTempTestMeet.Get(GuidId);
        }
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
         public FoWoSoft.Data.Model.TempTestMeet Get(string useId)
        {
            return dataTempTestMeet.Get(useId);
        }
        public int RoomisModify(FoWoSoft.Data.Model.TempTestMeet tempmeet)
        {
            return dataTempTestMeet.RoomisModify(tempmeet);
        }
        public int RoomisAdd(FoWoSoft.Data.Model.TempTestMeet tempmeet)
        {
            return dataTempTestMeet.RoomisAdd(tempmeet);
        }
       
        }
}
