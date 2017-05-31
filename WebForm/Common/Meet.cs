using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebForm.Common
{
    public class Meet
    {
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