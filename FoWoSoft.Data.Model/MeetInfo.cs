using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoWoSoft.Data.Model
{
    public class MeetInfo
    {
        public int Id { get; set; }
        public string ApplicatId { get; set; }
        public string MeetTimes { get; set; }
        public string MeetId { get; set; }
        public string MeetName { get; set; }
        public string AdminId { get; set; }
        /// <summary>
        /// 会议ID
        /// </summary>
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
           
        public DateTime  Date1{ get; set; }
        public string  test1{ get; set; } 
        public string  test{ get; set; } 
        /// <summary>
        /// 会议类型
        /// </summary>
        public string  typeid{ get; set; }
        public string  type{ get; set; } 
        public string  Reason{ get; set; } 
        public string  inland{ get; set; }
        public string abroad { get; set; }
    }
}