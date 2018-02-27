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
    }

}