using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForm.Common
{
    public class DuanxinService
    {
        cn.edu.ecnu.duanxin.SMSService ecnuweb = new cn.edu.ecnu.duanxin.SMSService();//申明Webservice
        cn.edu.ecnu.duanxin.MySoapHeader header = new cn.edu.ecnu.duanxin.MySoapHeader();
        public DuanxinService()
        {
            header.Username = "ECNUSMS";
            header.Password = "ECNU3081";
            ecnuweb.MySoapHeaderValue = header;
        }
        //发送短信sms
        public void smsSend(string account, string msg)
        {
            FoWoSoft.Platform.Log.Add1($"发送短信前账号1：{account},信息：{msg}", msg, FoWoSoft.Platform.Log.Types.其它分类);

            //根据UserId获取从远程用户信息
            var telephone = new EduWebService().GetMobile(account);
            if (string.IsNullOrWhiteSpace(telephone)) return;

            var result = ecnuweb.WaitSMSSend(telephone, msg);
            FoWoSoft.Platform.Log.Add1($"{telephone}发送短信:{msg},结果：{result}", msg, FoWoSoft.Platform.Log.Types.其它分类);

        }
        public void Sendapplication(string installceid, string msg)
        {

            //获取申请人手机号
            var tempTestMeet = new FoWoSoft.Platform.TempTestMeet().Get(installceid);
            if ((tempTestMeet != null) && (!string.IsNullOrWhiteSpace(tempTestMeet.DeptID)))
            {
                var telephone = tempTestMeet.DeptID;
                var result = ecnuweb.WaitSMSSend(telephone, msg);
                FoWoSoft.Platform.Log.Add1($"{telephone}发给申请人短信:{msg},结果：{result}", msg, FoWoSoft.Platform.Log.Types.其它分类);


            }
      }
        /*
                申请人：
        申请人：
申请过程：您申请的 【会议地址名称】 【会议名称】 会议，****（部门）已申请通过。
申请成功：您申请的 【会议地址名称】 【会议名称】 会议，已审核完毕，可以使用。
申请失败：您申请的 【会议地址名称】 【会议名称】 会议，没有通过审核。审核人：****；审核意见：****
签核人：
【信息办】 部门的 【张三】，申请【会议地址名称】 【会议名称】 会议，会议时间安排在【2018-3-10】，需要您审核。
转签：
【信息办】 部门的 【张三】，申请 【会议地址名称】 【会议名称】 会议，会议时间安排在【2018-3-10】，由***转签给您，需要您审核。
信息办：
【信息办】 部门的 【张三】，申请 【会议地址名称】 【会议名称】 会议已通过，会议时间安排在【2018-3-10】，请确认并提供相关支持。

一般会议：
申请通过：您申请的 【会议地址名称】 【会议名称】 会议，申请成功，。

        */
        /// <summary>
        ///  申请人：
        /// 申请过程：您申请的 【会议地址名称】 【会议名称】 会议，****（部门）已申请通过。
        /// </summary>
        public static string DuanxinSendMsg1 = "申请过程：您申请的 {1}{0}会议，{2}（部门）已申请通过。";
        /// <summary>
        ///  申请人：
        ///  申请成功：您申请的 【会议地址名称】 【会议名称】 会议，已审核完毕，可以使用。
        /// </summary>
        public static string DuanxinSendMsg2 = "申请成功：您申请的 {1}{0}会议，已审核完毕，可以使用。";
        /// <summary>
        ///  申请人：
        ///  申请失败：您申请的 【会议地址名称】 【会议名称】 会议，没有通过审核。审核人：****；审核意见：****
        /// </summary>
        public static string DuanxinSendMsg3 = "申请失败：您申请的 {1}{0}会议，没有通过审核。审核人：{2}；审核意见：{3}。";
        /// <summary>
        /// 签核人：
        /// 【信息办】 部门的 【张三】，申请【会议地址名称】 【会议名称】 会议，会议时间安排在【2018-3-10】，需要您审核。
        /// </summary>
        public static string DuanxinSendMsg4 = "{0}部门的{1}，申请{2}{3}会议，会议时间安排在{4}，需要您审核。";
        /// <summary>
        /// 转签：
        /// 【信息办】 部门的 【张三】，申请 【会议地址名称】 【会议名称】 会议，会议时间安排在【2018-3-10】，由*** 转签给您，需要您审核。
        /// </summary>
        public static string DuanxinSendMsg5 = "{0} 部门的{1}，申请{2} {3} 会议，会议时间安排在{4}，由{5} 转签给您，需要您审核。";
        /// <summary>
        /// 信息办：
        /// 【信息办】 部门的 【张三】，申请 【会议地址名称】 【会议名称】 会议已通过，会议时间安排在【2018-3-10】，请确认并提供相关支持。
        /// </summary>
        public static string DuanxinSendMsg6 = "{0} 部门的{1}，申请{2} {3} 会议已通过，会议时间安排在{4}，请确认并提供相关支持。";
        public static string DuanxinSendMsg7 = "申请通过：您申请的 {0} {1} 会议，申请成功。";



    }

}