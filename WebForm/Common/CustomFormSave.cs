using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WebForm.Common
{
    public class CustomFormSave
    {
        public static string QianShi(FoWoSoft.Data.Model.WorkFlowCustomEventParams eventParams)
        {
            string title = System.Web.HttpContext.Current.Request.Form["Title"];
            string Contents = System.Web.HttpContext.Current.Request.Form["Contents"];

            if (eventParams.InstanceID.IsInt())
            {
                string sql = "UPDATE TempTest_CustomForm SET Title=@Title,Contents=@Contents WHERE ID=@ID";
                SqlParameter[] parArray = { 
                             new SqlParameter("@Title", title),
                             new SqlParameter("@Contents", Contents),
                             new SqlParameter("@ID", eventParams.InstanceID.ToString())
                             };
                new FoWoSoft.Data.MSSQL.DBHelper().Execute(sql, parArray);
                return eventParams.InstanceID.ToString();
            }
            else
            {
                string sql = "INSERT INTO TempTest_CustomForm(Title,Contents,FlowCompleted) VALUES(@Title,@Contents,@FlowCompleted);SELECT SCOPE_IDENTITY();";
                SqlParameter[] parArray = { 
                             new SqlParameter("@Title", title),
                             new SqlParameter("@Contents", Contents),
                             new SqlParameter("@FlowCompleted", "0")
                             };
                return new FoWoSoft.Data.MSSQL.DBHelper().ExecuteScalar(sql, parArray);
            }
        }

        /// <summary>
        /// 子流程激活前事件（示例）
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static FoWoSoft.Data.Model.WorkFlowExecute.Execute SubFlowActivationBefore(FoWoSoft.Data.Model.WorkFlowCustomEventParams eventParams)
        {
            FoWoSoft.Data.Model.WorkFlowExecute.Execute execute = new FoWoSoft.Data.Model.WorkFlowExecute.Execute();

            //在这里添加插入子流程业务数据代码

            FoWoSoft.Platform.Log.Add("执行了子流程激活前事件", "", FoWoSoft.Platform.Log.Types.其它分类);

            return execute;
        }

        /// <summary>
        /// 子流程结束后事件（示例）
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static void SubFlowCompletedBefore(FoWoSoft.Data.Model.WorkFlowCustomEventParams eventParams)
        {

            //在这里添加子流程结束后代码

            FoWoSoft.Platform.Log.Add("执行了子流程结束后事件", "", FoWoSoft.Platform.Log.Types.其它分类);
        }
    }
}