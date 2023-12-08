using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using static ERPAP.Models.Sys.SystemFunAssignModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunAssign(SystemFunAssignModel model, List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.CurrentCultureID = base.CultureID;
            model.GetSysSystemFunTabList(_BaseAPModel.EnumTabAction.SysSystemFunAssign);

            List<SystemFunAssign> systemFunAssignList = new List<SystemFunAssign>();

            if (await model.GetSystemFunInfo(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunAssign.SystemMsg_GetSystemFunInfor);
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetSystemFunAssignList(CultureID))
                    {
                        systemFunAssignList = model.EntitySystemFunAssignList;
                    }

                    if (await model.GetEditSystemFunAssignResult(AuthState.SessionData.UserID, systemFunAssignValueList, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunAssign.EditSystemFunAssignResult_Failure);
                        result = false;
                    }
                    else
                    {
                        if (systemFunAssignValueList != null)
                        {
                            if (systemFunAssignValueList.Count == 1 && string.IsNullOrWhiteSpace(systemFunAssignValueList[0].UserID))
                            {
                                if (systemFunAssignList != null && systemFunAssignList.Count > 0)
                                {
                                    foreach (SystemFunAssign systemFunAssign in model.EntitySystemFunAssignList)
                                    {
                                        ExecUserFunLogWrite(systemFunAssign.UserID, model.ErpWFNo, model.Memo);
                                    }
                                }
                            }
                            else
                            {
                                foreach (EntitySystemFunAssign.SystemFunAssignValue systemFunAssignValue in systemFunAssignValueList)
                                {
                                    ExecUserFunLogWrite(systemFunAssignValue.UserID, model.ErpWFNo, model.Memo);
                                }
                            }
                        }

                        await model.RecordLogUserFunApply(CultureID, AuthState.SessionData.UserID, ClientIPAddress(), systemFunAssignValueList);

                        if (base.GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemFunAssignEditEntity(systemFunAssignValueList).SerializeToJson();
                            if (base.ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFunAssign, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemFunAssign.EditSystemFunAssignResult_Failure);
                                result = false;
                            }
                        }
                    }

                    if (result)
                    {
                        SetSystemAlertMessage(SysSystemFunAssign.SystemMsg_SetSystemFunAssignResultWasSuccess);
                    }
                }

                if (result && model.ExecAction == EnumActionType.Query)
                {
                    return RedirectToAction("SystemFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemFunAssignList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunAssign.SystemMsg_GetSystemFunAssignList);
            }

            return View(model);
        }
    }
}