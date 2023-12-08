using System.Web.Mvc;
using ERPAP.Models.Dev;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class DevController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult FunSchedule(FunScheduleModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update &&
                    model.GetEditFunScheduleResult(AuthState.SessionData.UserID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(DevFunSchedule.EditFunScheduleResult_Failure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetDevPhaseList(base.CultureID) == false)
            {
                SetSystemErrorMessage(DevFunSchedule.SystemMsg_GetDevPhaseList);
            }

            if (model.GetSystemFun(base.CultureID) == false)
            {
                SetSystemErrorMessage(DevFunSchedule.SystemMsg_GetSystemFunInfor);
            }
            else
            {
                model.DevOwner = model.EntitySystemFun.DevOwner.GetValue();
                model.PreBeginDate = model.EntitySystemFun.PreBeginDate.GetValue();
                model.PreEndDate = model.EntitySystemFun.PreEndDate.GetValue();
                model.PreWorkHours = model.EntitySystemFun.PreWorkHours.StringValue();
                model.ActBeginDate = model.EntitySystemFun.ActBeginDate.GetValue();
                model.ActEndDate = model.EntitySystemFun.ActEndDate.GetValue();
                model.ActWorkHours = model.EntitySystemFun.ActWorkHours.StringValue();
            }

            if (model.GetFunScheduleList(base.CultureID) == false)
            {
                SetSystemErrorMessage(DevFunSchedule.SystemMsg_GetFunScheduleList);
            }

            return View(model);
        }
    }
}