using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using LionTech.APIService.LineBot;
using LionTech.APIService.Teams;
using LionTech.Entity.ERP;

namespace LionTech.Utility.ERP
{
    public static partial class PublicFun
    {
        public static readonly string JsAlertMsgFormatString = "<script type='text/javascript'>alert('{0}');window.close();</script>";

        public static string GetUserMenu(string userMenuXSLString, string userMenuFilePath, string userMenuUserID, EnumUserMenuID userMenuID)
        {
            if (!File.Exists(userMenuFilePath))
            {
                throw new UtilityERPException(EnumUtilityERPMessage.MenuIsNotExist, new string[] { userMenuFilePath });
            }

            XDocument xDocument = XDocument.Load(userMenuFilePath);

            return GetUserMenu(userMenuXSLString, xDocument, userMenuUserID, userMenuID);
        }

        public static string GetUserMenu(string userMenuXSLString, XDocument xDocument, string userMenuUserID, EnumUserMenuID userMenuID)
        {
            var result = (from q
                in xDocument.Elements(EnumUserMenu.UserMenu.ToString()).Elements(EnumUserMenu.MenuDatas.ToString()).Elements(EnumUserMenu.MenuData.ToString())
                          where q.Parent.Parent.Attribute(EnumUserMenu.MenuUserID.ToString()).Value == userMenuUserID
                                && q.Attribute(EnumUserMenu.MenuID.ToString()).Value == ((int)userMenuID).ToString()
                          select q.Element(EnumUserMenu.MenuContents.ToString()));

            if (result.Count() != 1)
            {
                return null;
            }
            else
            {
                return GetXMLToHTMLByXSL(result.FirstOrDefault().ToString(), userMenuXSLString);
            }
        }
        
        private static readonly XslCompiledTransform XslCompiledTransform = new XslCompiledTransform();

        private static string GetXMLToHTMLByXSL(string xmlString, string xslString)
        {
            lock (XslCompiledTransform)
            {
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(xslString)))
                {
                    XslCompiledTransform.Load(xmlReader);
                }

                StringWriter resultStringWriter = new StringWriter();
                using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString)))
                {
                    XslCompiledTransform.Transform(xmlReader, null, resultStringWriter);
                }
                XslCompiledTransform.TemporaryFiles.Delete();
                return resultStringWriter.ToString();
            }
        }

        public static string GetFormattedRequestString(string requestString)
        {
            string returnQuery = string.Empty;

            if (string.IsNullOrWhiteSpace(requestString) == false &&
                requestString.IndexOf('?') > -1)
            {
                List<string> queryParam = new List<string>();
                char andMark = '&';
                char equalMark = '=';

                string[] urlPara = requestString.Substring(requestString.IndexOf('?')).Split(andMark);

                foreach (string para in urlPara)
                {
                    if (para.Split(equalMark)[0] == "EventPara" ||
                        para.Split(equalMark)[0] == "APIPara" ||
                        para.Split(equalMark)[0] == "FlowPara")
                    {
                        queryParam.Add(string.Concat(
                            para.Split(equalMark)[0],
                            equalMark,
                            Common.GetFormingJsonString(para.Split('=')[1]).Replace(Environment.NewLine, "<br/>")
                            ));
                    }
                    else
                    {
                        queryParam.Add(para);
                    }
                }

                returnQuery =
                    string.Concat(requestString.Substring(0, requestString.IndexOf('?')),
                        "<br/>",
                        string.Join("<br/>", string.Join("<br/>" + andMark, queryParam)));
            }

            return returnQuery;
        }

        public static AuthenticationPara GetLionGroupAuthenticationPara(string userID, string userPWD, EnumLocation location, string locationDesc, string userIDNo, string UserBirthday, string ipAddress)
        {
            AuthenticationPara para =
                new AuthenticationPara
                {
                    UserID = userID,
                    UserPWD = (string.IsNullOrWhiteSpace(userPWD) ? null : Security.Encrypt(userPWD)),
                    Location = location.ToString(),
                    LocationDesc = (string.IsNullOrWhiteSpace(locationDesc) ? null : locationDesc),
                    UserIDNo = (string.IsNullOrWhiteSpace(userIDNo) ? null : Security.Encrypt(userIDNo)),
                    UserBirthday = (string.IsNullOrWhiteSpace(UserBirthday) ? null : Security.Encrypt(UserBirthday)),
                    IPAddress = ipAddress
                };

            return para;
        }

        public static void SendErrorTeamsForSERP(string teamsChannelID, string title,string userID, string userName, Exception ex)
        {
            TeamsClient client = TeamsClient.Create();
            client.ClientSysID = EnumSystemID.ERPAP.ToString();

            Exception logException = ex;

            if (logException.InnerException != null)
            {
                logException = logException.InnerException;
            }

            var msg =
                 string.Format("[{0}] 於：{1} 系統發生錯誤。" + "<br>" + " 使用者 ：{2} ，" + "<br>" + " 系統錯誤訊息 : {3} ，" + "<br>" + " 系統錯誤堆疊 : {4}。",
                     title
                        .Replace("http://", string.Empty)
                        .Replace("https://", string.Empty)
                        .Replace(".liontravel.com.tw", string.Empty)
                        .Replace(".liontravel.com", string.Empty)
                        .ToUpper(),
                     Common.GetDateTimeText()
                     , (userID + " " + userName)
                     , logException.Message
                     , logException.StackTrace);

            client.SendMessage(teamsChannelID, title, msg);
        }



        public static void SendErrorMailForSERP(SERPMailMessages msg)
        {
            if (string.IsNullOrWhiteSpace(msg.SmtpClientIPAddress) == false &&
                string.IsNullOrWhiteSpace(msg.MialAddress) == false)
            {
                var client = new SmtpClient { Host = msg.SmtpClientIPAddress };
                var mailMessages = new MailMessage { From = new MailAddress(msg.MialAddress) };
                var subject =
                    string.Format("[{0}] 於：{1} 系統發生錯誤。",
                        msg.AppName
                           .Replace("http://", string.Empty)
                           .Replace("https://", string.Empty)
                           .Replace(".liontravel.com.tw", string.Empty)
                           .Replace(".liontravel.com", string.Empty)
                           .ToUpper(),
                        Common.GetDateTimeText());

                mailMessages.To.Add(new MailAddress(msg.MialAddress));
                mailMessages.Priority = System.Net.Mail.MailPriority.High;
                mailMessages.Subject = subject;
                mailMessages.SubjectEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.BodyEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.IsBodyHtml = true;
                mailMessages.Body = msg.GetHtmlMssageBody();

                client.Send(mailMessages);
            }
        }

        public static void SendErrorLineForSERP(string appName, string lineID, IEnumerable<string> to, Exception ex)
        {
            appName =
                appName.Replace("http://", string.Empty)
                       .Replace("https://", string.Empty)
                       .Replace(".liontravel.com.tw", string.Empty)
                       .Replace(".liontravel.com", string.Empty)
                       .ToUpper();

            ILineClient lineClient = LineClient.Create();
            lineClient.SystemID = EnumSystemID.ERPAP.ToString();
            lineClient.LineID = lineID;

            Exception logException = ex;

            if (logException.InnerException != null)
            {
                logException = logException.InnerException;
            }

            LineMessageText messageText = new LineMessageText
            {
                Text = string.Format("[{0}] 於：{1} 系統發生錯誤，{2}。",
                    appName,
                    Common.GetDateTimeText(),
                    logException.Message)
            };

            lineClient.AddToRange(to);
            lineClient.AddMessage(messageText);
            lineClient.SendMessage();
        }

        public static async Task<string> HttpPutWebRequestGetResponseStringAsync(string requestUriString,int timeOut,byte[] byteArray)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Timeout = timeOut;
            httpWebRequest.Method = "PUT";

            using (Stream stream = await httpWebRequest.GetRequestStreamAsync())
            {
                await stream.WriteAsync(byteArray, 0, byteArray.Length);
            }

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
            {
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
        }
    }

    public class SERPMailMessages
    {
        public string MialAddress { get; set; }
        public string SmtpClientIPAddress { get; set; }
        public string AppName { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public Exception Ex { get; set; }

        public string GetHtmlMssageBody()
        {
            var logException = Ex;

            if (logException.InnerException != null)
            {
                logException = logException.InnerException;
            }

            var stackTraceMsg = string.Empty;
            if (logException.StackTrace != null)
            {
                stackTraceMsg = logException.StackTrace.Replace(Environment.NewLine, "<br>");
            }

            return
                string.Join("<br>",
                    new object[]
                    {
                        (string.IsNullOrWhiteSpace(UserID)
                            ? string.Empty
                            : "User: " + (UserID + " " + UserName)),
                        "Error Message: " + logException.Message,
                        "<br>",
                        stackTraceMsg,
                        "<br>", "<br>", "<br>"
                    });
        }
    }

    public class ExtensionDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : class
    {
        public new void Add(TKey key, TValue value)
        {
            if (value == null)
            {
                return;
            }

            base.Add(key, value);
        }

        public new void Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
                return;
            }

            base.Remove(key);
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                return TryGetValue(key, out value) ? value : null;
            }
            set
            {
                base[key] = value;
            }
        }

        public void Set(Dictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> data in dict)
            {
                Add(data.Key, data.Value);
            }
        }
    }

    /// <summary>
    /// Transfers execution to the supplied url.
    /// </summary>
    public class TransferResult : ActionResult
    {
        public string Url { get; private set; }

        public TransferResult(string url)
        {
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpContext = HttpContext.Current;

            // MVC 3 running on IIS 7+
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(this.Url, true);
            }
            else
            {
                // Pre MVC 3
                httpContext.RewritePath(this.Url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }
        }
    }
}