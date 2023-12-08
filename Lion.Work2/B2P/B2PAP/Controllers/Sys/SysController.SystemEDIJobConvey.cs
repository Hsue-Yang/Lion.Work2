using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIJobConvey(SystemEDIJobConveyModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemEDIJobConveyModel.Field.QuerySysID.ToString(), model.QuerySysID);
            paraDict.Add(SystemEDIJobConveyModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add == true)
                {

                    switch (model.GetEDIJobSettingResult(AuthState.SessionData.UserID, base.CultureID))
                    {
                        case 0:
                            SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyError);
                            result = false;
                            break;
                        case 1:
                            SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyCountError);
                            result = false;
                            break;
                        case 2:
                            break;
                        default:
                            SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyError);
                            result = false;
                            break;
                    }
                }
                if (result)
                {
                    return RedirectToAction("SystemEDIJob", "Sys");
                }
            }

            return View(model);
        }
    }
}