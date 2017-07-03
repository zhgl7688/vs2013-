using FoWoSoft.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm.EduModels
{
    public class MeetInfoModel:MeetInfo
    {
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}