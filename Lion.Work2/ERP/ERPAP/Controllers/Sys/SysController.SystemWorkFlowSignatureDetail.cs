using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using static ERPAP.Models._BaseAPModel;
using static ERPAP.Models.Sys.SysModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowSignatureDetail(SystemWorkFlowSignatureDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("SystemWorkFlowSignature", "Sys");
                }

                await SetSystemWorkFlowSignatureDetailSysParameter(model);

                if (TryValidatableObject(model))
                {
                    bool result = true;

                    switch (model.ExecAction)
                    {
                        case EnumActionType.Add:
                            if (await model.GetSystemWFSig(CultureID))
                            {
                                SetSystemAlertMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_AddWFSignatureDetailExist);
                                result = false;
                            }
                            else
                            {
                                if (await model.EditSystemWFSig(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                                {
                                    SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.EditWFSignatureDetail_Failure);
                                    result = false;
                                }
                                else if (GetEDIServiceDistributor())
                                {
                                    string eventParaJsonString = model.GetEventParaSysSystemWFSignatureEdit().SerializeToJson();

                                    if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFSignature, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                    {
                                        SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.EditWFSignatureDetail_Failure);
                                        result = false;
                                    }
                                }
                            }
                            break;
                        case EnumActionType.Update:
                            if (await model.EditSystemWFSig(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.EditWFSignatureDetail_Failure);
                                result = false;
                            }
                            else if (GetEDIServiceDistributor())
                            {
                                string eventParaJsonString = model.GetEventParaSysSystemWFSignatureEdit().SerializeToJson();

                                if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFSignature, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                {
                                    SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.EditWFSignatureDetail_Failure);
                                    result = false;
                                }
                            }
                            break;
                        case EnumActionType.Delete:
                            if (await model.DeleteSystemWFSig(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                            {
                                SetSystemAlertMessage(SysSystemWorkFlowSignatureDetail.DeleteWFSignatureDetail_Failure);
                                result = false;
                            }
                            else if (GetEDIServiceDistributor())
                            {
                                string eventParaJsonString = model.GetEventParaSysSystemWFSignatureDelete().SerializeToJson();

                                if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFSignature, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                                {
                                    SetSystemAlertMessage(SysSystemWorkFlowSignatureDetail.DeleteWFSignatureDetail_Failure);
                                    result = false;
                                }
                            }
                            break;
                    }

                    if (result)
                    {
                        if (model.ExecAction == EnumActionType.Add ||
                            model.ExecAction == EnumActionType.Update ||
                            model.ExecAction == EnumActionType.Delete)
                        {
                            if (model.UpdateWorkFlowChart(AuthState.SessionData.UserID, model.SysID, model.WFFlowID, model.WFFlowVer) == false)
                            {
                                SetSystemAlertMessage(SysResource.SystemMsg_RefreshChartFailure);
                            }
                        }
                        return RedirectToAction("SystemWorkFlowSignature", "Sys");
                    }
                }
            }
            else
            {
                model.FormReset();

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetSystemWFSig(CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSystemWorkFlowSignatureDetail);
                    }
                }

                await SetSystemWorkFlowSignatureDetailSysParameter(model);
            }

            return View(model);
        }

        private async Task SetSystemWorkFlowSignatureDetailSysParameter(SystemWorkFlowSignatureDetailModel model)
        {
            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemWFNode);
            }

            if (await model.GetSystemRoleSigList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemRoleIDList);
            }
            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.SignatureUserType), CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetWorkFlowNodeTypeList);
            }
            
            await model.GetSystemWFSigSeqList(CultureID);

            #region - 驗證API -
            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysUserSystemSysIDList);
            }
            else if (model.EntityUserSystemSysIDList != null)
            {
                model.ValidSystemSysIDSelectItems = new List<UserSystemSysID>(model.EntityUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.ValidAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIGroupList);
            }
            else if (model.SystemAPIGroupByIdList != null)
            {
                model.ValidSystemAPIGroupSelectItems = new List<SysModel.SysSystemAPIGroup>(model.SystemAPIGroupByIdList);
            }

            if (await model.GetSystemAPIFuntionList(model.ValidAPISysID, model.ValidAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }
            else if (model.EntitySystemAPIFuntionList != null)
            {
                model.ValidSystemAPIFuntionSelectItems = new List<SysModel.SystemAPIFuntions>(model.EntitySystemAPIFuntionList);
            }
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.APISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIGroupList);
            }

            if (await model.GetSystemAPIFuntionList(model.APISysID, model.APIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }
        }
    }
}