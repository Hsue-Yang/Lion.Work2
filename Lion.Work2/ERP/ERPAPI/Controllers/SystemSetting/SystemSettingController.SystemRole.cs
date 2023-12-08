using ERPAPI.Models.SystemSetting;
using LionTech.Utility;
using System;
using System.Text;
using System.Web;
using System.Web.Http;
using LionTech.APIService.SystemSetting;

namespace ERPAPI.Controllers
{
    public partial class SystemSettingController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemRoleQueryEvent([FromUri]SystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var systemUserList = model.GetSystemRoleList();
                string responseString = Common.GetJsonSerializeObject(systemUserList);
                return Text(responseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemRoleEditEvent([FromUri]SystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            
            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;

                SystemRole systemRole = Common.GetJsonDeserializeObject<SystemRole>(jsonPara);

                if (model.EditSystemRole(systemRole, updUserID))
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

        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemRoleDeleteEvent([FromUri]SystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            
            try
            {
                SystemRole systemRole = Common.GetJsonDeserializeObject<SystemRole>(jsonPara);

                if (model.DeleteSystemRole(systemRole))
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
    }
}