using ERPAPI.Models.Authorization;
using LionTech.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPGenerateUserMenuEvent([FromUri] ERPGenerateUserMenuModel model)
        {
            if (AuthState.IsAuthorized == false) return null;
            
            try
            {
                model.APIData = Common.GetJsonDeserializeObject<ERPGenerateUserMenuModel.APIParaData>(model.APIPara);

                if (model.EditUserFunInfo(model.APIData.UserID))
                {
                    if (model.APIData.IsDevEnv)
                    {
                        byte[] fileByteArray = GenerateUserMenuDevEnvXMLResult(model.APIData.UserID);
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(fileByteArray)
                        };
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        return ResponseMessage(result);
                    }

                    bool generateUserMenuXMLResult = GenerateUserMenuXMLResult(model.APIData.UserID);
                    if (generateUserMenuXMLResult)
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return BadRequest();
        }
    }
}