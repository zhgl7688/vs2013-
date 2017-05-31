using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Platform
{
  public class MeetInfo
    {
        private FoWoSoft.Data.Interface.IMeetInfo dataMeetInfo;
        public MeetInfo()
        {
            this.dataMeetInfo = Data.Factory.Factory.GetMeetInfo();
        }
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.MeetInfo Get(string useId)
        {
            return dataMeetInfo.Get(useId);
        }
        public List<FoWoSoft.Data.Model.MeetInfo> GetAll()
        {
            return dataMeetInfo.GetAll();
        }
    }
}
