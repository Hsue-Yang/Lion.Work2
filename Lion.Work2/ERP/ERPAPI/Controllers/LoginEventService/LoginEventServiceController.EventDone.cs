// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Web;
using System.Web.Http;
using ERPAPI.Models.LoginEventService;
using LionTech.Utility;

namespace ERPAPI.Controllers.LoginEventService
{
    public partial class LoginEventServiceController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult EventDone(EventDoneModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            try
            {
                string updUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (model.EditEventDone(updUser))
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