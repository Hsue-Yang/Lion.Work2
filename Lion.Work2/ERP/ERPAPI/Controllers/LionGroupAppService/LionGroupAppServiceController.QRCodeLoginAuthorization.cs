// 新增日期：2017-03-23
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.LionGroupAppService;
using LionTech.Utility;

namespace ERPAPI.Controllers.LionGroupAppService
{
    public partial class LionGroupAppServiceController
    {
        [HttpPost]
        [Route("LionGroupAppService/v1/QRCodeLoginAuthorization")]
        [TokenAuthorizationActionFilter]
        public HttpResponseMessage QRCodeLoginAuthorizationForApp(QRCodeLoginAuthorizationModel model)
        {
            HttpStatusCode httpStatusCode;

            try
            {
                string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
                model.ExecLogAPIClientBegin(HttpUtility.UrlDecode(HostUrl()), jsonPara, ClientIPAddress());

                httpStatusCode = QRCodeLoginAuthorizationResult();
                
                model.ExecLogAPIClientEnd((httpStatusCode == HttpStatusCode.OK).ToString());
                return Request.CreateResponse(httpStatusCode);
            }
            catch (Exception ex)
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                OnException(ex);
            }

            model.ExecLogAPIClientEnd(bool.FalseString);
            return Request.CreateResponse(httpStatusCode);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public HttpResponseMessage QRCodeLoginAuthorization([FromUri]QRCodeLoginAuthorizationModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            HttpStatusCode httpStatusCode;
            string apiNo = GetApiNo();

            try
            {
                httpStatusCode = QRCodeLoginAuthorizationResult();
                return Request.CreateResponse(httpStatusCode);
            }
            catch (Exception ex)
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                OnException(ex);
            }
            
            return Request.CreateResponse(httpStatusCode);
        }

        internal HttpStatusCode QRCodeLoginAuthorizationResult()
        {
            bool result = true;
            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            var definitionUserInfo = new { UserID = (string)null, Password = (string)null, PingCode = (string)null };
            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            var userInfo = Common.GetJsonDeserializeAnonymousType(jsonPara, definitionUserInfo);

            if (string.IsNullOrWhiteSpace(userInfo.UserID) ||
                string.IsNullOrWhiteSpace(userInfo.Password) ||
                string.IsNullOrWhiteSpace(userInfo.PingCode))
            {
                result = false;
            }

            if (result)
            {
                string pwd = Security.AesDecrypt(userInfo.Password, "1234567812345678", "1234567812345678", Security.EnumEncodeType.Default);
                pwd = Security.Encrypt(pwd);
                userInfo = new
                {
                    userInfo.UserID,
                    Password = pwd,
                    userInfo.PingCode
                };

                jsonPara = Common.GetJsonSerializeObject(userInfo);
                string response = ExecAPIService(EnumAppSettingKey.ERPAPHomeQRCodeAuthorization, jsonPara, WebRequestMethods.Http.Post);

                if (response == bool.TrueString)
                {
                    httpStatusCode = HttpStatusCode.OK;
                }
            }

            return httpStatusCode;
        }
    }
}