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
        public ActionResult SystemServiceList(SystemServiceListModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.EditSystmeService(AuthState.SessionData.UserID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemServiceList.InsertSystemServiceListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Update && model.EditSystmeService(AuthState.SessionData.UserID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemServiceList.UpdataSystemServiceListFailure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete && model.DeleteSystmeService(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemServiceList.DeleteSystemServiceListFailure);
                    result = false;
                }
            }

            if (result)
            {
                model.FormReset();
            }

            model.GetBaseSystemServiceList(base.CultureID);

            if (model.GetSystmeServiceList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemServiceList.SystemMsg_GetSystemServiceList);
            }

            return View(model);
        }
    }
}