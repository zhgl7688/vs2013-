using FoWoSoft.Data.Model.WorkFlowExecute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm.Controls.WorkFlow
{
    public class WorkFlowOpation
    {
        FoWoSoft.Data.Model.WorkFlowExecute.Execute execute;
        FoWoSoft.Platform.WorkFlow bworkFlow = new FoWoSoft.Platform.WorkFlow();
        public WorkFlowOpation(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
        {
            this.execute = execute;
        }
        public void submit()
        {

        }
        public void save()
        {

        }
        public void back()
        {

        }
        public void completed()
        {
            //操作roomis审核
            new WebForm.Common.Meet().Roomis(execute.InstanceID, WebForm.Common.RoomisOperation.put_approve);
        }
        public void redirect()
        {

        }
        
    }
}