﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using FoWoSoft.Data.Model;

namespace WebForm.Common
{
    public enum RoomisOperation
    {
        put_reject,
        put_approve,
        put_step
    }
    public class Meet
    {
        private static string meetUrl = ConfigurationManager.AppSettings["meetUrl"].ToString();
        private DuanxinService duanxinService = new DuanxinService();
        public static List<CMeet> GetMeetList()
        {
            string result = "";
            // context.Response.ContentType = "text/json";
            // context.Response.Write("Hello World");
            //1>创建请求
            var url = "http://console.qa.roomis.com.cn/api/spaces";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Consumer-Custom-ID", "10251025");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream stream = response.GetResponseStream();

                if (!String.IsNullOrEmpty(response.ContentEncoding))
                {
                    if (response.ContentEncoding.Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.Contains("deflate"))
                    {
                        stream = new DeflateStream(stream, CompressionMode.Decompress);
                    }
                }

                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    int count;
                    byte[] buffer = new byte[4096];
                    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, count);
                    }
                    bytes = ms.ToArray();
                }

                #region 检测流编码
                Encoding encoding;

                //检测响应头是否返回了编码类型,若返回了编码类型则使用返回的编码
                //注：有时响应头没有编码类型，CharacterSet经常设置为ISO-8859-1
                if (!string.IsNullOrEmpty(response.CharacterSet) && response.CharacterSet.ToUpper() != "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding(response.CharacterSet == "utf8" ? "utf-8" : response.CharacterSet);
                }
                else
                {
                    //若没有在响应头找到编码，则去html找meta头的charset
                    result = Encoding.Default.GetString(bytes);
                    //在返回的html里使用正则匹配页面编码
                    Match match = Regex.Match(result, @"<meta.*charset=""?([\w-]+)""?.*>", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        encoding = Encoding.GetEncoding(match.Groups[1].Value);
                    }
                    else
                    {
                        //若html里面也找不到编码，默认使用utf-8
                        encoding = Encoding.GetEncoding("utf-8");
                    }
                }
                #endregion

                result = encoding.GetString(bytes);
                var cms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CMeet>>(result);
                return cms;
            }
        }
        public object Roomis(FoWoSoft.Data.Model.WorkFlowExecute.Execute execute)
        {
            var meetInfo = new FoWoSoft.Platform.MeetInfo().GetByTemp3(execute.InstanceID);
            if (meetInfo == null) return null;
            string roomisId = meetInfo.temp1;//会议id;
           // var meetMsg = $" 您申请的会议名称:{meetInfo.temp2}；会议地址:{meetInfo.MeetName},";

            if (WebForm.Common.Tools.CheckBack(execute.ExecuteType, execute.StepID))

            {
                put_reject(roomisId, execute.Sender.Account);

                // 申请失败：您申请的会议名称：***；会议地址：****，审核结果：没有通过：审核人：****；审核意见：****
                var msg = string.Format(DuanxinService.DuanxinSendMsg3, meetInfo.temp2, meetInfo.MeetName, execute.Sender.Name, execute.Comment);
                //$" 申请失败：{meetMsg}审核结果：没有通过：审核人：{execute.Sender.Name},审核意见：{execute.Comment}";
                //20180110短信发送
               // duanxinService.smsSend(execute.Sender.Account, msg);
                //发给审请人
                duanxinService.Sendapplication(execute.InstanceID, msg);
            }
            else if (execute.ExecuteType == FoWoSoft.Data.Model.WorkFlowExecute.EnumType.ExecuteType.Completed)
            {
                var tasks = new FoWoSoft.Platform.WorkFlowTask().GetAll();
                var installTasks = tasks.Where(s => s.InstanceID.ToString().Equals(execute.InstanceID, StringComparison.OrdinalIgnoreCase) && s.Status == 0);


                string remarks = execute.Comment;
                put_approve(roomisId, execute.Sender.Account, remarks);
                //申请成功：您申请的会议名称：***；会议地址：****，已审核完毕，可以使用。
                var msg = string.Format(DuanxinService.DuanxinSendMsg2, meetInfo.temp2, meetInfo.MeetName);
                //var msg = $"申请成功：{meetMsg}已审核完毕，可以使用。" ;
                //20180110短信发送
                //anxinService.smsSend(execute.Sender.Account, msg+ "请查看");
                //发给审请人
                duanxinService.Sendapplication(execute.InstanceID, msg);
            }
            else
            {
                SendStep(roomisId, execute.InstanceID,meetInfo);
            }
            return 1;
        }
        public object Roomis(string installId, RoomisOperation operation)
        {
            var meetInfo = new FoWoSoft.Platform.MeetInfo().GetByTemp3(installId);
            if (meetInfo == null) return null;
            string eventId = meetInfo.temp1;//会议id;
            string approverId = meetInfo.AdminId;// FoWoSoft.Platform.Users.CurrentUser.Account;
            Func<string, string, object> put = null;
            switch (operation)
            {
                case RoomisOperation.put_reject:
                    put = put_reject;
                    break;
                case RoomisOperation.put_approve:
                    //  put = put_approve;
                    break;
                case RoomisOperation.put_step:
                    return SendStep(eventId, installId, meetInfo);


            }

            return put(eventId, approverId);
        }


        /// <summary>
        /// 每一步的会议审核
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="apperot"></param>
        /// <returns></returns>
        public string SendStep(string eventId, string instanceid, FoWoSoft.Data.Model.MeetInfo meetInfo)
        {

            var meetMsg = $" 您申请的会议名称:{meetInfo.temp2}；会议地址:{meetInfo.MeetName},";
         
            var userInfoEdu = new EduWebService().GetUser(meetInfo.ApplicatId);
            if (userInfoEdu == null) return null;
         
            var tasks = new FoWoSoft.Platform.WorkFlowTask().GetAll();
            var installTasks = tasks.Where(s => s.InstanceID.ToString().Equals(instanceid, StringComparison.OrdinalIgnoreCase) && s.Status == 0);
            string[] remarks = new string[] { "请审核", "打开", "中间完成", "退回", "他人已处理", "他人已退回" };
            var n = 0;
            foreach (var item in installTasks)
            { var task= tasks.FirstOrDefault(s => s.ID == item.PrevID);
                if (n == 0)//发给审请人
                {
              
                    if (task!=null) 
                            //申请过程：您申请的会议名称：***；会议地址：****，****（部门）申请通过。
                        duanxinService.Sendapplication(instanceid, string.Format(DuanxinService.DuanxinSendMsg1, meetInfo.temp2,meetInfo.MeetName, task.StepName));
                }
                n++;
          var approver = new FoWoSoft.Platform.Users().Get(item.ReceiveID).Account;
                var remark = (item.Status > -1 && item.Status < 6) ? remarks[item.Status] : "";
                string data = JsonConvert.SerializeObject(new
                {
                    approver = approver,
                    status = "PENDING",
                    remarks = remark
                });
                string address = "api/booking/events/{0}/approval";
                //由*** 部门，***（人名），申请的会议名称为：****会议申请，需要您审核。
                FoWoSoft.Platform.Log.Add1(string.Format("各部门({0})", item.StepName + userInfoEdu.BMMC+ userInfoEdu.XM+ meetInfo.temp2), data, FoWoSoft.Platform.Log.Types.其它分类);


                //20180110短信发送
               
                if (item.StepName == "信息办")
                    //由*** 部门，***（人名），申请的会议名称为：****会议申请已通过，请确认并提供相关支持。
                    duanxinService.Sendapplication(instanceid, string.Format(DuanxinService.DuanxinSendMsg6, userInfoEdu.BMMC, userInfoEdu.XM, meetInfo.MeetName, meetInfo.temp2, meetInfo.MeetTimes));
                else  if (item.StepName.Contains( "各部门")) {
                    var prevName = tasks.FirstOrDefault(s => s.ID == item.PrevID).StepName;

                duanxinService.smsSend(approver, string.Format(DuanxinService.DuanxinSendMsg5, userInfoEdu.BMMC, userInfoEdu.XM, meetInfo.MeetName, meetInfo.temp2, meetInfo.MeetTimes, prevName));
                }
                else
                {
                    duanxinService.smsSend(approver, string.Format(DuanxinService.DuanxinSendMsg4, userInfoEdu.BMMC, userInfoEdu.XM, meetInfo.MeetName, meetInfo.temp2, meetInfo.MeetTimes));

                }

                Put_Roomis(eventId, address, data);
            }

            return "1";

        }
        /// <summary>
        /// 会议审核通过
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="apperot"></param>
        /// <returns></returns>
        public static string put_approve(string eventId, string apperot, string remarks)
        {
            string address = "api/booking/events/{0}/approval";

            string data = JsonConvert.SerializeObject(new
            {
                approver = apperot,
                state= "APPROVED",// status = "APPROVED",
                remarks = remarks,// "备注文字",
            });

            return Put_Roomis(eventId, address, data);

        }
        /// <summary>
        /// 会议审核拒绝
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="apperot"></param>
        /// <returns></returns>
        public static string put_reject(string eventId, string apperot)
        {
            string address = "api/booking/events/{0}/approval";
            string data = JsonConvert.SerializeObject(new
            {
                approver = apperot,
                state= "REJECTED",//status = "REJECTED",2018-2-26
                remarks = "退回"
            });
            return Put_Roomis(eventId, address, data);
        }

        public static string Put_Roomis(string eventId, string address, string data)
        {
            FoWoSoft.Platform.Log.Add(string.Format("发送流程({0})", eventId + data), data, FoWoSoft.Platform.Log.Types.其它分类);

            var method = string.Format(address, eventId);

            string Url = meetUrl;
            string Data = data;
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content = new StringContent(Data, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Add("X-Consumer-Custom-ID", "004");
                try
                {
                    var response = client.PutAsync(method, content).Result;
                    // var ss = client.PutAsJsonAsync(method, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return "Success";
                    }
                    else
                        return "Error";
                }
                catch (Exception err)
                {
                    FoWoSoft.Platform.Log.Add(string.Format("连接有问题({0})", meetUrl), data, FoWoSoft.Platform.Log.Types.系统错误);

                    return meetUrl + "连接有问题" + err.Message;

                }

            }
        }
        public static void gethttp(string Url)
        {


            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Add("X-Consumer-Custom-ID", "004");
                var response = client.GetAsync(Url);//.PutAsync(method, content).Result;
                // var ss = client.PutAsJsonAsync(method, content).Result;
                //if (response.Status=)
                //{
                //    return "Success";
                //}
                //else
                //    return "Error";
            }
        }


        public static string GetMeetOptions(string value)
        {
            StringBuilder options = new StringBuilder();
            var meetList = GetMeetList("meetinfo");
            foreach (var item in meetList)
            {
                options.AppendFormat("<option value=\"{0}\"{1}>{2}{3}</option>", item.id, item.id == value ? " selected=\"selected\"" : "", " ", item.name);
            }
            return options.ToString();


        }
        public static List<CMeet> GetMeetList(string name)
        {
            var cList = new List<CMeet>();
            var list = new FoWoSoft.Platform.MeetInfo().GetAll();
            foreach (var item in list)
            {
                cList.Add(new CMeet
                {
                    id = item.MeetId,
                    name = item.MeetName
                });
            }
            return cList;
        }
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

    }

    public class CMeet
    {
        public string id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string capacity { get; set; }
        public string numDevice { get; set; }
        public string spaceUse { get; set; }
        public object attributes { get; set; }

    }
}