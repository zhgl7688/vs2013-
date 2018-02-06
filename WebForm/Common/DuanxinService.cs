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
   
        //发送短信sms
        public void smsSend(string account, string msg)
        {
            FoWoSoft.Platform.Log.Add($"发送短信前账号1：{account},信息：{msg}", msg, FoWoSoft.Platform.Log.Types.其它分类);

            header.Username = "ECNUSMS";
            header.Password = "ECNU3081";
            ecnuweb.MySoapHeaderValue = header;  
            //根据UserId获取从远程用户信息
            var telephone = new EduWebService().GetMobile(account);
            if (string.IsNullOrWhiteSpace( telephone)) return;

            var result = ecnuweb.WaitSMSSend(telephone, msg);
            FoWoSoft.Platform.Log.Add($"{telephone}发送短信:{msg},结果：{result}", msg, FoWoSoft.Platform.Log.Types.其它分类);

        }
    }

}