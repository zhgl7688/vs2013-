using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Interface
{
    public interface IMeetInfo
    {

        /// <summary>
        /// 根据申请人Id applicatId查询一条记录
        /// </summary>
        FoWoSoft.Data.Model.MeetInfo Get(string applicatId);
        List<Model.MeetInfo> GetAll();
        Model.MeetInfo GetByMeetId(string meetId);
        bool MeetInfoRepeat(string temp1);
        int Create(FoWoSoft.Data.Model.MeetInfo meetInfo);
        int ModifyByTemp1(FoWoSoft.Data.Model.MeetInfo meetInfo);

        Model.MeetInfo GetByTemp1(string temp1);
        int DeleteByMeetId(string meetId);
        int RoomisUpdate(FoWoSoft.Data.Model.MeetInfo meetInfo,out string testMeetid);
      
        
        }
        
}
