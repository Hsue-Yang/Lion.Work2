using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Authorization;
using LionTech.Utility;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPUserRoleQueryEvent([FromUri]ERPUserRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var userRoleList = model.GetUserRoleList();

                string responseString = Common.GetJsonSerializeObject(userRoleList);
                
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPUserRoleEditEvent([FromUri]ERPUserRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserRoleModel.APIParaData>(model.APIPara);

                if (model.EditUserRole(apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress()))
                {
                    bool generateUserMenuXMLResult = GenerateUserMenuXMLResult(model.APIData.UserID);

                    model.GetERPRecordUserFunctionResult(model.APIData.UserID, apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());

                    if (string.IsNullOrWhiteSpace(model.APIData.ErpWFNo) == false)
                    {
                        model.GetERPRecordUserSystemRoleApplyResult(apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());
                    }

                    if (generateUserMenuXMLResult)
                    {
                        return Text(bool.TrueString);
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }

        [HttpPost]
        [AuthorizationActionFilter("ERPUserRoleEditEvent")]
        public IHttpActionResult ERPUserRoleEditEventByPost([FromUri]ERPUserRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            string apiNo = GetApiNo();

            try
            {
                model.APIData = Common.GetJsonDeserializeObject<ERPUserRoleModel.APIParaData>(jsonPara);

                if (model.EditUserRole(apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress()))
                {
                    bool generateUserMenuXMLResult = GenerateUserMenuXMLResult(model.APIData.UserID);

                    model.GetERPRecordUserFunctionResult(model.APIData.UserID, apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());

                    if (string.IsNullOrWhiteSpace(model.APIData.ErpWFNo) == false)
                    {
                        model.GetERPRecordUserSystemRoleApplyResult(apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());
                    }

                    if (generateUserMenuXMLResult)
                    {
                        return Text(bool.TrueString);
                    }
                }
                
                return Text(bool.FalseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }
    }
}