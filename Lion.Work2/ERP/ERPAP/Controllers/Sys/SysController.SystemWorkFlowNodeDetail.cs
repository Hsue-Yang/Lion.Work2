using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using LionTech.WorkFlow;
using Resources;
using static ERPAP.Models._BaseAPModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowNodeDetail(SystemWorkFlowNodeDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (await model.GetSystemWFFlowName(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSystemWFFlow);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.WorkFlowNodeType), CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetWorkFlowNodeTypeList);
            }

            if (await model.GetBackSystemWFNodeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetBackWorkFlowNodeIDList);
            }

            if (IsPostBack == false)
            {
                if (model.ExecAction == EnumActionType.Add)
                {
                    model.FormReset();
                }
                else if (model.ExecAction == EnumActionType.Update)
                {
                    EnumNodeType nodeType;
                    if (await model.GetSystemWFNodeDetail() == false ||
                        Enum.TryParse(model.NodeType, out nodeType) == false)
                    {
                        model.ExecAction = EnumActionType.Add;
                        SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSystemWorkFlowNodeDetail);
                    }
                    else
                    {
                        model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNodeDetail, nodeType);
                        model.TabList = model.SysWFNodeTabList;
                    }
                }

               await SetSystemWorkFlowNodeDetailSysParameter(model);
            }
            else
            {
               await SetSystemWorkFlowNodeDetailSysParameter(model);

                if (TryValidatableObject(model))
                {
                    bool result = true;

                    if (model.ExecAction == EnumActionType.Add ||
                        model.ExecAction == EnumActionType.Update)
                    {
                        if (await model.GetEditSystemWFNodeDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.EditSystemWorkFlowNodeDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFNodeEdit().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFNode, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.EditSystemWorkFlowNodeDetailResult_Failure);
                                result = false;
                            }
                        }
                    }
                    else if (model.ExecAction == EnumActionType.Delete)
                    {
                        if (await model.GetDeleteSystemWFNodeDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.DeleteSystemWorkFlowNodeDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFNodeDelete().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFNode, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                            {
                                SetSystemAlertMessage(SysSystemWorkFlowDetail.DeleteSystemWorkFlowDetailResult_Failure);
                                result = false;
                            }
                        }
                    }

                    if (result)
                    {
                        if (model.ExecAction == EnumActionType.Add ||
                            model.ExecAction == EnumActionType.Update)
                        {
                            model.ExecAction = EnumActionType.Update;
                            SetSystemAlertMessage(SysSystemWorkFlowNodeDetail.EditSystemWorkFlowNodeDetailResult_Success);
                        }

                        if (model.ExecAction == EnumActionType.Delete)
                        {
                            model.ExecAction = EnumActionType.Add;
                            SetSystemAlertMessage(SysSystemWorkFlowNodeDetail.DeleteSystemWorkFlowNodeDetailResult_Success);
                        }

                        if (model.UpdateWorkFlowChart(AuthState.SessionData.UserID, model.SysID, model.WFFlowID, model.WFFlowVer) == false)
                        {
                            SetSystemAlertMessage(SysResource.SystemMsg_RefreshChartFailure);
                        }
                    }
                }

                EnumNodeType nodeType;
                if (Enum.TryParse(model.NodeType, out nodeType) &&
                    model.ExecAction != EnumActionType.Add)
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNodeDetail, nodeType);
                    model.TabList = model.SysWFNodeTabList;
                }
            }

            if (await model.GetSystemWFNodeRoleList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysSystemRoleIDList);
            }

            return View(model);
        }

        private async Task SetSystemWorkFlowNodeDetailSysParameter(SystemWorkFlowNodeDetailModel model)
        {
            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysUserSystemSysIDList);
            }
            else
            {
                model.EntityAssgSysUserSystemSysIDList = model.EntityUserSystemSysIDList;
            }

            if (await model.GetSysSystemFunControllerIDList(model.FunSysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.AssgAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysSystemAPIGroupList);
            }
            else
            {
                model.EntityAssgSysSystemAPIGroupList = model.SystemAPIGroupByIdList;
            }

            if (await model.GetSystemFunNameList(model.FunSysID, model.FunControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysSystemFunNameList);
            }

            if (await model.GetSystemAPIFuntionList(model.AssgAPISysID, model.AssgAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNodeDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }
            else
            {
                model.EntityAssgSysSystemAPIFuntionList = model.EntitySystemAPIFuntionList;
            }

            if (await model.GetSystemAPIGroupByIdList(model.FunSysID, AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIGroupList);
            }

            if (await model.GetSystemAPIFuntionList(model.FunSysID, model.APIControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }
        }
    }
}