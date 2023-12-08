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
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPSystemFunQueryEvent([FromUri]SystemFunModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            try
            {
                var systemFunList = model.GetSystemFunList();
                string responseString = Common.GetJsonSerializeObject(systemFunList);
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