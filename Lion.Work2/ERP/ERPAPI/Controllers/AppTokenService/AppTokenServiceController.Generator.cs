using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using ERPAPI.Models;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAPI.Controllers.AppTokenService
{
    public partial class AppTokenServiceController
    {
        [HttpGet]
        public HttpResponseMessage Generator()
        {
            HttpResponseMessage actionResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _BaseAPModel model = new _BaseAPModel();

            try
            {
                model.ExecLogAPIClientBegin(HttpUtility.UrlDecode(HostUrl()), MSHttpContext.Request.Headers[HttpRequestHeader.Authorization.ToString()], ClientIPAddress());

                string responseString;

                int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.APITimeOut.ToString()]);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format(ConfigurationManager.AppSettings[EnumAppSettingKey.APIERPAPAppTokenServiceGeneratorEventURL.ToString()], Common.GetEnumDesc(EnumAPISystemID.TKNAP)));
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Headers.Set(HttpRequestHeader.Authorization, MSHttpContext.Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                httpWebRequest.Timeout = apiTimeOut;
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            responseString = streamReader.ReadToEnd();
                        }
                    }
                }

                model.ExecLogAPIClientEnd(responseString);
                actionResult = new HttpResponseMessage(HttpStatusCode.OK);
                actionResult.Content = new StringContent(responseString, Encoding.UTF8);
                return actionResult;
            }
            catch (WebException webException)
            {
                var response = webException.Response as HttpWebResponse;
                if (response != null)
                {
                    actionResult = new HttpResponseMessage(response.StatusCode);
                }
                model.ExecLogAPIClientEnd(string.Empty);
                return actionResult;
            }
            catch (Exception ex)
            {
                OnException(ex);
                model.ExecLogAPIClientEnd(string.Empty);
                return actionResult;
            }
        }
    }
}
