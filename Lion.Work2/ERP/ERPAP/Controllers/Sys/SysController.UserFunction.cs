using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserFunction(UserFunctionModel model, List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.CurrentCultureID = base.CultureID;
            model.GetSysUserRoleFunctionTabList(_BaseAPModel.EnumTabAction.SysUserFunction);

            if (await model.GetUserRawData() == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetUserRawData);
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update &&
                    systemUserFunctionValueList != null &&
                    systemUserFunctionValueList.Any())
                {
                    if (await model.GetUserFunctionList(AuthState.SessionData.UserID, CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserFunction.SystemMsg_GetUserFunctionList);
                    }

                    if (await model.GetEditUserFunctionResult(systemUserFunctionValueList, AuthState.SessionData.UserID, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysUserFunction.EditUserFunctionResult_Failure);
                        result = false;
                    }
                    else
                    {
                        ExecUserFunLogWrite(model.UserID, model.ErpWFNo, model.Memo);
                        await model.RecordLogUserFunApply(CultureID, AuthState.SessionData.UserID, ClientIPAddress(), systemUserFunctionValueList);

                        if (base.GetEDIServiceDistributor())
                        {
                            if (await model.GetSystemEventTargetList(EnumEDIServiceEventGroupID.SysUserFunction.ToString(), EnumEDIServiceEventID.Edit.ToString(), base.CultureID) == false)
                            {
                                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemEventTargetList);
                            }
                            else
                            {
                                if (model.EntitySystemEventTargetList != null &&
                                    model.EntitySystemEventTargetList.Count > 0)
                                {
                                    foreach (SysModel.SystemEventTarget target in model.EntitySystemEventTargetList)
                                    {
                                        string eventParaJsonString = model.GetEventParaSysUserFunctionEditEntity(target.SysID, systemUserFunctionValueList).SerializeToJson();
                                        if (base.ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysUserFunction, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                        {
                                            SetSystemErrorMessage(SysUserFunction.EditUserFunctionResult_Failure);
                                            result = false;
                                        }
                                    }
                                }
                            }
                        }

                        var zh_TW = model.GenerateUserMenuXML(EnumCultureID.zh_TW);
                        var zh_CN = model.GenerateUserMenuXML(EnumCultureID.zh_CN);
                        var en_US = model.GenerateUserMenuXML(EnumCultureID.en_US);
                        var th_TH = model.GenerateUserMenuXML(EnumCultureID.th_TH);
                        var ja_JP = model.GenerateUserMenuXML(EnumCultureID.ja_JP);
                        var ko_KR = model.GenerateUserMenuXML(EnumCultureID.ko_KR);

                        GenerateUserMenu(model.UserID, zh_TW, zh_CN, en_US, th_TH, ja_JP, ko_KR);
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

            if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (await model.GetSystemFunNameList(model.SysID, model.FunControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemFunNameList);
            }

            if (await model.GetUserFunctionList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetUserFunctionList);
            }

            if (await model.GetSystemSysIDList(true, CultureID) == false)
            {
                SetSystemErrorMessage(SysUserFunction.SystemMsg_GetSysSystemSysIDList_Failure);
            }

            return View(model);
        }
    }
}