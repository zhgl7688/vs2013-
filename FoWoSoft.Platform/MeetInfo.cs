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
        /// 根据applicatId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.MeetInfo Get(string applicatId)
        {
            return dataMeetInfo.Get(applicatId);
        }
        /// <summary>
        /// 根据meetId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.MeetInfo GetByMeetId(string meetId)
        {
            return dataMeetInfo.GetByMeetId(meetId);
        }
        public List<FoWoSoft.Data.Model.MeetInfo> GetAll()
        {
            return dataMeetInfo.GetAll();
        }
        public bool MeetInfoRepeat(string temp1)
        {
            return dataMeetInfo.MeetInfoRepeat(temp1);
        }
        public int Create(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            return dataMeetInfo.Create(meetInfo);
        }
        public int ModifyByTemp1(FoWoSoft.Data.Model.MeetInfo meetInfo)
        {
            return dataMeetInfo.ModifyByTemp1(meetInfo);
        }
        public FoWoSoft.Data.Model.MeetInfo GetByTemp1(string temp1)
        {
            return dataMeetInfo.GetByTemp1(temp1);
        }
        public int DeleteByMeetId(string temp1)
        {
            //判断流程是否开始
            var meetinfo = dataMeetInfo.GetByTemp1(temp1);
            if (meetinfo != null)
            {
                var workflowtask = new FoWoSoft.Data.MSSQL.WorkFlowTask();
                var task = workflowtask.GetListByinstanceid(meetinfo.temp3);
                var tasktwo = task.FirstOrDefault(s => s.StepID == Guid.Parse("88B44E40-E9EB-44F9-9F2B-18B0AAE70A5A"));
                if (tasktwo != null && tasktwo.Status == 0)
                {
                    dataMeetInfo.DeleteByTemp1(temp1);
                    task.ForEach(s => workflowtask.Delete(s.ID));
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
             
        }
        public int RoomisUpdate(FoWoSoft.Data.Model.MeetInfo meetInfo, out string testMeetid)
        {
            return dataMeetInfo.RoomisUpdate(meetInfo, out testMeetid);
        }
        public FoWoSoft.Data.Model.MeetInfo GetByTemp3(string temp3)
        {
            return dataMeetInfo.GetByTemp3(temp3);
        }
    }
}
