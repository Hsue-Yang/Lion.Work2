using ERPAPI.Models.SystemSetting;
using LionTech.APIService.SystemSetting;
using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class SystemSettingController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemRoleFunEditEvent([FromUri]SystemRoleFunModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            
            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;

                List<SystemRoleFun> systemRoleFuns = Common.GetJsonDeserializeObject<List<SystemRoleFun>>(jsonPara);

                if (model.EditSystemRoleFun(systemRoleFuns, updUserID))
                {
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

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemRoleFunQueryEvent([FromUri]SystemRoleFunModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var systemRoleFun = model.GetSystemRoleFun();
                string responseString = Common.GetJsonSerializeObject(systemRoleFun);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }
    }
}