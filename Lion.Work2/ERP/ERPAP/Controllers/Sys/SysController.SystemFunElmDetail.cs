// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限明細
// ---------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunElmDetail(SystemFunElmDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ModifyType, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);

            if (IsPostBack == false)
            {
                if (await model.GetSystemFunElmDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_GetSystemFunElmDetail_Failure);
                }
            }
            else
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Update ||
                    model.ExecAction == EnumActionType.Add ||
                    model.ExecAction == EnumActionType.Delete)
                {
                    if (model.ExecAction == EnumActionType.Update ||
                        model.ExecAction == EnumActionType.Add)
                    {
                        if (TryValidatableObject(model))
                        {
                            if (await model.EditSystemFunElmDetail(AuthState.SessionData.UserID) == false)
                            {
                                SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_EditSystemFunElmDetail_Failure);
                                result = false;
                            }

                            if (result && model.RecordLogSysSystemFunElm(AuthState.SessionData.UserID, CultureID) == false)
                            {
                                SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_RecordLogSysSystemFunElm_Failure);
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    //if (model.ExecAction == EnumActionType.Delete)
                    //{
                    //    if (model.GetSystemFunElmDetail() == false)
                    //    {
                    //        SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_GetSystemFunElmDetail_Failure);
                    //        result = false;
                    //    }

                    //    if (result && model.DeleteSystemFunElmDetail() == false)
                    //    {
                    //        SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_EditSystemFunElmDetail_Failure);
                    //        result = false;
                    //    }

                    //    if (result && model.RecordLogSysSystemFunElm(AuthState.SessionData.UserID, CultureID) == false)
                    //    {
                    //        SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_RecordLogSysSystemFunElm_Failure);
                    //        result = false;
                    //    }
                    //}
                }

                if (result)
                {
                    return RedirectToAction("SystemFunElm");
                }
            }

            if (model.ExecAction == EnumActionType.Add)
            {
                if (await model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunElmDetail.SystemMsg_GetSystemInfoList_Failure);
                }

                model.FormReset();
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunElm")]
        public async Task<ActionResult> SystemFunElmRole(SystemFunElmRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ModifyType, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);
            await model.GetSystemFunElmInfo(AuthState.SessionData.UserID, CultureID);

            if (await model.GetSysSystemRolesList(AuthState.SessionData.UserID, model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunElmRole.SystemMsg_GetSysSystemRoleIDList_Failure);
            }

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (TryValidatableObject(model))
                    {
                        if (await model.EditSystemFunElmRole(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(SysSystemFunElmRole.SystemMsg_EditElmAuthRole_Failure);
                            result = false;
                        }

                        if (result && model.RecordLogSystemRoleFunElm(AuthState.SessionData.UserID, CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemFunElmRole.SystemMsg_RecordLogSystemRoleFunElm_Failure);
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                model.IsSaveSuccess = result;
            }
            else
            {
                if (await model.GetSystemFunElmRoleList(AuthState.SessionData.UserID, CultureID, model.SystemRoleByIdList) == false)
                {
                    SetSystemErrorMessage(SysSystemFunElmRole.SystemMsg_GetElmAuthRoleDetail_Failure);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunElm")]
        public ActionResult SystemFunElmUser(SystemFunElmUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.ModifyType, Entity_BaseAP.EnumCMCodeKind.ElmDisplayType);
            model.GetSystemFunElmInfo(CultureID);

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (TryValidatableObject(model))
                    {
                        if (model.GetElmAuthUserDetail() == false)
                        {
                            SetSystemErrorMessage(SysSystemFunElmUser.SystemMsg_GetElmAuthUserDetail_Failure);
                            result = false;
                        }

                        if (result && model.EditElmAuthUser(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(SysSystemFunElmUser.SystemMsg_EditElmAuthUser_Failure);
                            result = false;
                        }

                        if (result && model.RecordLogSystemUserFunElm(AuthState.SessionData.UserID, CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemFunElmUser.SystemMsg_RecordLogSystemUserFunElm_Failure);
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                model.ElmUserSaveSuccess = result;
            }
            else
            {
                if (model.GetElmAuthUserDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemFunElmUser.SystemMsg_GetElmAuthUserDetail_Failure);
                }

                model.ConvertFunElmUserDictionary();
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunElm")]
        public ActionResult GetElmRawCMUserList(List<string> userIDList)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemFunElmDetailModel model = new SystemFunElmDetailModel();

            var rawCMUserList = model.GetElmRawCMUserList(userIDList);

            if (rawCMUserList != null &&
                rawCMUserList.Any())
            {
                return Json((from s in rawCMUserList
                             select new
                             {
                                 UserID = s.UserID.GetValue(),
                                 UserIDNM = s.UserIDNM.GetValue()
                             }));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunElm")]
        public ActionResult GetFunElmUserInfo(string condition)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemFunElmUserModel model = new SystemFunElmUserModel();

            var autoUserInfoList = model.GetAutoUserInfoList(condition);

            if (autoUserInfoList != null &&
                autoUserInfoList.Any())
            {
                return Json((from s in autoUserInfoList
                             select new
                             {
                                 UserID = s.UserID.GetValue(),
                                 UserNM = s.UserNM.GetValue()
                             }));
            }

            return Json(null);
        }
    }
}