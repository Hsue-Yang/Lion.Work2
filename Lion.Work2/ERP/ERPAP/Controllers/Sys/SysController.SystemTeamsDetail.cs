using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter("SystemTeams")]
        public async Task<ActionResult> SystemTeamsDetail(SystemTeamsDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemTeamsDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemTeamsDetail.SystemMsg_IsExistSystemTeams);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemTeams(model.ExecAction, AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemTeamsDetail.EditSystemTeamsDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (await model.DeleteSystemTeams(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemTeamsDetail.DeleteSystemTeamsDetailResult_Failure);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemTeams", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemTeamsDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemTeamsDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemTeamsDetail.SystemMsg_UnGetSystemTeamsDetail);
            }

            return View(model);
        }
    }
}