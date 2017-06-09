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
        put_approve
    }
    public class Meet
    {
        private static string meetUrl = ConfigurationManager.AppSettings["meetUrl"].ToString();
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
        public  object Roomis(string installId, RoomisOperation operation)
        {
            var meetInfo = new FoWoSoft.Platform.MeetInfo().GetByTemp3(installId);
            if (meetInfo == null) return null;
            string eventId = meetInfo.temp1;
            string approverId = meetInfo.AdminId;// FoWoSoft.Platform.Users.CurrentUser.Account;
            Func<string, string, object> put = null;
            switch (operation)
            {
                case RoomisOperation.put_reject: put = put_reject;
                    break;
                case RoomisOperation.put_approve: put = put_approve;
                    break;
            }

            return put(eventId, approverId);
        }

        /// <summary>
        /// 会议审核拒绝
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="apperot"></param>
        /// <returns></returns>
        public static string put_reject(string eventId, string apperot)
        {
            var method = string.Format("api/booking/events/{0}/reject", eventId);

            string Url = meetUrl;
            string Data = JsonConvert.SerializeObject(new
            {
                approver = apperot
            });
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string strDecodeBody = HttpUtility.UrlEncode(Data);
                // HttpContent content = new StringContent( strDecodeBody);
                HttpContent content = new StringContent(Data, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Add("X-Consumer-Custom-ID", "004");
                var response = client.PutAsync(method, content).Result;
                // var ss = client.PutAsJsonAsync(method, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                    return "Error";
            }
        }
        /// <summary>
        /// 会议审核通过
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="apperot"></param>
        /// <returns></returns>
        public static string put_approve(string eventId, string apperot)
        {
            var method = string.Format("api/booking/events/{0}/approve", eventId);

            string Url = meetUrl;
            string Data = JsonConvert.SerializeObject(new
            {
                approver = apperot
            });
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string strDecodeBody = HttpUtility.UrlEncode(Data);
                // HttpContent content = new StringContent( strDecodeBody);
                HttpContent content = new StringContent(Data, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(Url);
                client.DefaultRequestHeaders.Add("X-Consumer-Custom-ID", "10251025");
                var response = client.PutAsync(method, content).Result;
                // var ss = client.PutAsJsonAsync(method, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                    return "Error";
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