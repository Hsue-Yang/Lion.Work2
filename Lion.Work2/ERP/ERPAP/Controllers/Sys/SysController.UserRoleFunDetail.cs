using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserRoleFunDetail(UserRoleFunDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.GetSysUserRoleFunctionTabList(_BaseAPModel.EnumTabAction.SysUserRoleFunDeatil);

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (TryValidatableObject(model))
                    {
                        if (await model.GetUserSystemRoleList(AuthState.SessionData.UserID, CultureID) == false)
                        {
                            SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetUserSystemRoleList);
                        }

                        if (await model.GetEditUserSystemRoleResult(AuthState.SessionData.UserID, HostIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysUserRoleFunDetail.EditUserSystemRoleResult_Failure);
                            result = false;
                        }
                        else
                        {
                            ExecUserFunLogWrite(model.UserID, null, null);
                            model.RecordLogUserSystemRoleApply(CultureID, AuthState.SessionData.UserID, ClientIPAddress());

                            if (GetEDIServiceDistributor())
                            {
                                if (await model.GetSystemEventTargetList(EnumEDIServiceEventGroupID.SysUserSystemRole.ToString(), EnumEDIServiceEventID.Edit.ToString(), CultureID) == false)
                                {
                                    SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetSysSystemEventTargetList);
                                }
                                else
                                {
                                    if (model.EntitySystemEventTargetList != null && model.EntitySystemEventTargetList.Count > 0)
                                    {
                                        foreach (SysModel.SystemEventTarget target in model.EntitySystemEventTargetList)
                                        {
                                            string eventParaJsonString = model.GetEventParaSysUserSystemRoleEditEntity(target.SysID).SerializeToJson();
                                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysUserSystemRole, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                            {
                                                SetSystemErrorMessage(SysUserRoleFunDetail.EditUserSystemRoleResult_Failure);
                                                result = false;
                                            }
                                        }
                                    }
                                }
                            }

                            if (model.HasRole != null && model.HasRole.Count > 0)
                            {
                                var zh_TW = model.GenerateUserMenuXML(EnumCultureID.zh_TW);
                                var zh_CN = model.GenerateUserMenuXML(EnumCultureID.zh_CN);
                                var en_US = model.GenerateUserMenuXML(EnumCultureID.en_US);
                                var th_TH = model.GenerateUserMenuXML(EnumCultureID.th_TH);
                                var ja_JP = model.GenerateUserMenuXML(EnumCultureID.ja_JP);
                                var ko_KR = model.GenerateUserMenuXML(EnumCultureID.ko_KR);

                                GenerateUserMenu(model.UserID, zh_TW, zh_CN, en_US, th_TH, ja_JP, ko_KR);
                            }
                        }
                    }
                    else
                    {
                        result = false;
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

            if (await model.GetUserMainInfo() == false)
            {
                SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetUserMainInfo);
            }
            else
            {
                model.UserNM = model.UserMainInfo.UserNM;
                model.RoleGroupID = model.UserMainInfo.RoleGroupID;
            }

            if (await model.GetSysSystemRoleGroupList(CultureID) == false)
            {
                SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetSysSystemRoleGroupList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetUserSystemRoleList(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetUserSystemRoleList);
                }
            }

            if (await model.GetSystemSysIDList(true, CultureID) == false)
            {
                SetSystemErrorMessage(SysUserRoleFunDetail.SystemMsg_GetSysSystemSysIDList_Failure);
            }

            return View(model);
        }

        //[HttpPost]
        //[AuthorizationActionFilter("UserRoleFunDetail")]
        //public async Task<ActionResult> GetSystemRoleGroupCollectList(string roleGroupID)
        //{
        //    if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

        //    UserRoleFunDetailModel model = new UserRoleFunDetailModel();

        //    if (await model.GetSysSystemRoleGroupCollectList(roleGroupID))
        //    {
        //        return Content(model.GetJsonFormSelectItem(model.SystemRoleGroupCollectList, false));
        //    }
        //    return Json(null);
        //}

        [HttpPost]
        [AuthorizationActionFilter("UserRoleFunDetail")]
        public ActionResult GenerateUserMenu(string userID, bool isDevEnv)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            byte[] byteArray =
                ExecAPIService(
                    EnumAppSettingAPIKey.APIERPAPAuthorizationERPGenerateUserMenuEventURL,
                    Common.GetJsonSerializeObject(new
                    {
                        userID, IsDevEnv = isDevEnv
                    }), 1024);

            if (isDevEnv)
            {
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet);
            }

            return Content(string.Empty);
        }
    }
}