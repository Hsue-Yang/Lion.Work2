using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIJobConvey(SystemEDIJobConveyModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Add)
                {
                    if (model.UploadEdiJobFile == null)
                    {
                        SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyError);
                        result = false;
                    }
                    else
                    {
                        switch (await model.GetEDIJobSettingResult(AuthState.SessionData.UserID, CultureID))
                        {
                            case SystemEDIJobConveyModel.EnumJobConveyCase.EditJobSuccess:
                                break;
                            case SystemEDIJobConveyModel.EnumJobConveyCase.UnJobData:
                                SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyError);
                                result = false;
                                break;
                            case SystemEDIJobConveyModel.EnumJobConveyCase.EditJobFailure:
                                SetSystemErrorMessage(SysSystemEDIJobConvey.SystemMsg_EDIJobConveyError);
                                result = false;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
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