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
        public ActionResult UserDomainDetail(UserDomainDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.EditUserDomainDetailList(base.CultureID, AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysUserDomainDetail.AddUserDomainDetailResult_Failure);
                        result = false;
                    }
                }
                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (model.DeleteUserDomainDetailList(base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserDomainDetail.DeleteDomainGroupDetailResult_Failure);
                        result = false;
                    }
                }
                if (result)
                {
                    return RedirectToAction("UserDomain", "Sys");
                }
            }
            model.GetDomainGroupList(base.CultureID);


            return View(model);
        }
    }
}
