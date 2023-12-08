using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LionTech.Utility.B2P
{
    public static partial class PublicFun
    {
        public static string GetUserMenu(string userMenuXSLString, string userMenuFilePath, string userMenuUserID, EnumUserMenuID userMenuID)
        {
            if (!File.Exists(userMenuFilePath))
            {
                throw new UtilityB2PException(EnumUtilityB2PMessage.MenuIsNotExist, new string[] { userMenuFilePath });
            }

            XDocument xDocument = XDocument.Load(userMenuFilePath);

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
                return Common.GetXMLToHTMLByXSL(result.FirstOrDefault().ToString(), userMenuXSLString);
            }
        }

        public static List<string> GetJsonToStringList(string jsonString)
        {
            List<string> returnList = new List<string>();

            string content = jsonString.Replace("{", string.Empty).Replace("}", string.Empty);
            string[] contentArray = content.Split(':');
            string result = null;

            returnList.Add("{");

            foreach (string value in contentArray)
            {
                string[] valueArray = new string[] { string.Empty, string.Empty };

                if (value.StartsWith("["))
                {
                    valueArray[0] = value.Substring(0, value.IndexOf(']') + 1);
                    valueArray[1] = value.Substring(value.IndexOf(']') + 1).Replace(",", string.Empty);
                }
                else
                {
                    valueArray = value.Split(',');
                }

                if (result == null)
                {
                    result = value;
                }
                else
                {
                    if (valueArray.Length == 1 || valueArray[1] == string.Empty)
                    {
                        returnList.Add(result + ":" + valueArray[0]);
                    }
                    else
                    {
                        returnList.Add(result + ":" + valueArray[0] + ",");
                        result = valueArray[1];
                    }
                }
            }

            returnList.Add("}");

            return returnList;
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