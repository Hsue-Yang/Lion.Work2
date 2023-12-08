using ERPAPI.Models.SystemSetting;
using LionTech.APIService.SystemSetting;
using LionTech.Utility;
using System;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class SystemSettingController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPRoleFunElmEditEvent([FromUri]SystemRoleFunElmModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            
            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;

                SystemRoleFunElm systemRoleFunElm = Common.GetJsonDeserializeObject<SystemRoleFunElm>(jsonPara);

                if (model.EditSystemRoleFunElm(systemRoleFunElm, updUserID))
                {
                    return Text(bool.TrueString);
                }
                
                return Text(bool.FalseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }
    }
}