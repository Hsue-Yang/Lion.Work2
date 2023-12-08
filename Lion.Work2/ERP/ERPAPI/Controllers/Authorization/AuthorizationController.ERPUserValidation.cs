using System;
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
        public IHttpActionResult ERPUserValidateEvent([FromUri]ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserModel.APIParaData>(model.APIPara);

                AuthenticationResults auth = model.ValidateERPUserInfor();
                model.GetRecordUserLoginResult(auth.VerificationResult, apiNo);

                return Text(jsonConvert.Serialize(auth));
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(string.Empty);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPUserValidation([FromUri]ERPUserValidationModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(bool.FalseString);

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserValidationModel.APIParaData>(model.APIPara);

                model.Format();

                if (model.GetValidUserAccountResult())
                {
                    return Text(bool.TrueString);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Text(bool.FalseString);
        }
    }
}