using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserRoleFunDetail(UserRoleFunDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (model.GetEditUserSystemRoleResult(AuthState.SessionData.UserID, base.HostIPAddress(), base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserRoleFunDetail.EditUserSystemRoleResult_Failure);
                        result = false;
                    }
                    else
                    {
                        base.ExecUserRoleLogWrite(model.UserID);

                        if (model.HasRole != null && model.HasRole.Count > 0)
                        {
                            string filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.zh_TW);
                            bool generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.zh_TW);

                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.zh_CN);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.zh_CN);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.en_US);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.en_US);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.th_TH);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.th_TH);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.ja_JP);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.ja_JP);
                            }

                            if (!generateUserMenuXMLResult)
                            {
                                base.SetSystemAlertMessage(SysUserRoleFunDetail.SystemMsg_GenerateFailure);
                                result = false;
                            }
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("UserRoleFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetUserRawData);
            }
            else
            {
                model.RoleGroupID = model.EntityUserRawData.RoleGroupID.GetValue();
            }

            if (model.GetSysSystemRoleGroupList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetSysSystemRoleGroupList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetUserSystemRoleList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetUserSystemRoleList);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("UserRoleFunDetail")]
        public ActionResult GetSystemRoleGroupCollectList(string roleGroupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserRoleFunDetailModel model = new UserRoleFunDetailModel();

            if (model.GetSysSystemRoleGroupCollectList(roleGroupID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemRoleGroupCollectList, false));
            }

            return Json(null);
        }
    }
}