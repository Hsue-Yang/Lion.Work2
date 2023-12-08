using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Authorization;
using LionTech.Entity;
using LionTech.Utility;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemUserQueryEvent([FromUri] ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var systemUserList = model.GetSystemUserList();
                string responseString = Common.GetJsonSerializeObject(systemUserList);
                
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }

        /// <summary>
        /// 目前尚未使用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPUserCreateEvent([FromUri]ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserModel.APIParaData>(model.APIPara);

                if (model.CreateERPUserAccount())
                {
                    model.GetRecordUserPWDResult(EnumYN.N, apiNo);
                    base.ExecERPUserAccessLogWrite(model.APIData.UserID, apiNo, EnumLogWriter.ERPAuthorization, model.ClientSysID,
                                                   EnumYN.N.ToString(), EnumYN.N.ToString(), EnumYN.N.ToString());
                    return Text(bool.TrueString);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public string ERPUserPWDResetEvent([FromUri]ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false) return string.Empty;
            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserModel.APIParaData>(model.APIPara);

                if (model.ResetERPUserPWD())
                {
                    model.GetRecordUserPWDResult(EnumYN.Y, apiNo);
                    return bool.TrueString;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return bool.FalseString;
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public string ERPUserUnlockEvent([FromUri]ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false) return string.Empty;
            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserModel.APIParaData>(model.APIPara);

                if (model.UnlockERPUserAccount())
                {
                    base.ExecERPUserAccessLogWrite(model.APIData.UserID, apiNo, EnumLogWriter.ERPAuthorization, model.ClientSysID,
                                                   null, EnumYN.N.ToString(), null);
                    return bool.TrueString;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return bool.FalseString;
        }
    }
}