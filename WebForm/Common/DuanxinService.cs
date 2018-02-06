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
            AddSoapHeader();
        }
        public void AddSoapHeader()
        {

            ecnuweb.MySoapHeaderValue = header;
        }
        //发送短信sms
        public void smsSend(string account, string msg)
        {
            FoWoSoft.Platform.Log.Add(string.Format("发送短信({0})", account + msg), msg, FoWoSoft.Platform.Log.Types.其它分类);

            header.Username = "ECNUSMS";
            header.Password = "ECNU3081";
            //根据UserId获取从远程用户信息
            var telephone = new EduWebService().GetMobile(account);
            if (string.IsNullOrWhiteSpace( telephone)) return;
            var result = ecnuweb.WaitSMSSend(telephone, msg);
        }
    }

}