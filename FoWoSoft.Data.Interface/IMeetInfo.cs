using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Interface
{
  public  interface IMeetInfo
    {
 
        /// <summary>
        /// 根据ApplicatId查询一条记录
        /// </summary>
        FoWoSoft.Data.Model.MeetInfo Get(string applicatId);
        List<Model.MeetInfo> GetAll();
    }
}
