using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserFunction(UserFunctionModel model, List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.CurrentCultureID = base.CultureID;
            model.GetSysUserRoleFunctionTabList(_BaseAPModel.EnumTabAction.SysUserFunction);

            if (model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetUserRawData);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (model.GetEditUserFunctionResult(systemUserFunctionValueList, AuthState.SessionData.UserID, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserFunction.EditUserFunctionResult_Failure);
                        result = false;
                    }
                    else
                    {
                        base.ExecUserFunLogWrite(model.UserID);

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
                            base.SetSystemAlertMessage(SysUserFunction.SystemMsg_GenerateFailure);
                            result = false;
                        }
                    }
                }

                if (model.ExecAction == EnumActionType.Query)
                {
                    return RedirectToAction("UserRoleFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemFunNameList(model.SysID, model.FunControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemFunNameList);
            }

            if (model.GetUserFunctionList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetUserFunctionList);
            }

            return View(model);
        }
    }
}