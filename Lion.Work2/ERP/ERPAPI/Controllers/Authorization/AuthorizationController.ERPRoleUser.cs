using ERPAPI.Models.Authorization;
using LionTech.Utility;
using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPRoleUserQueryEvent([FromUri]ERPRoleUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var roleUserList = model.GetRoleUserList();
                string responseString = Common.GetJsonSerializeObject(roleUserList);
                
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPRoleUserEditEvent([FromUri]ERPRoleUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string apiNo = GetApiNo();

            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPRoleUserModel.APIParaData>(model.APIPara);

                if (model.EditRoleUser(apiNo, updUserID, model.ClientSysID, ClientIPAddress()))
                {
                    if (string.IsNullOrWhiteSpace(model.APIData.ErpWFNo) == false)
                    {
                        model.GetERPRecordUserSystemRoleApplyResult(apiNo, updUserID, model.ClientSysID, ClientIPAddress());
                    }
                    
                    return Text(bool.TrueString);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Text(bool.FalseString);
        }

        [HttpPost]
        [ActionName("ERPRoleUserEditEvent")]
        [AuthorizationActionFilter("ERPRoleUserEditEvent")]
        public IHttpActionResult ERPRoleUserEditEventByPost([FromUri]ERPRoleUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }
            
            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            string apiNo = GetApiNo();

            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPRoleUserModel.APIParaData>(jsonPara);

                if (model.EditRoleUser(apiNo, updUserID, model.ClientSysID, ClientIPAddress()))
                {
                    if (string.IsNullOrWhiteSpace(model.APIData.ErpWFNo) == false)
                    {
                        model.GetERPRecordUserSystemRoleApplyResult(apiNo, updUserID, model.ClientSysID, ClientIPAddress());
                    }

                    return Text(bool.TrueString);
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