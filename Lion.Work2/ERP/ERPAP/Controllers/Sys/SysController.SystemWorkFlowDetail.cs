using System.Threading.Tasks;
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
        public async Task<ActionResult> SystemWorkFlowDetail(SystemWorkFlowDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetSysSystemWorkFlowGroupIDList);
            }

            if (await model.GetSystemUserRoleList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetSysSystemRoleIDList);
            }

            if (await model.GetFlowTypeList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetFlowTypeList);
            }

            if (IsPostBack == false)
            {
                model.FormReset();

                if (model.ExecAction == EnumActionType.Add)
                {
                    model.WFFlowVer = "001";
                }

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetSystemWorkFlowDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetSystemWorkFlowDetail);
                    }
                }
            }
            else
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("SystemWorkFlow", "Sys");
                }

                if (TryValidatableObject(model))
                {
                    bool result = true;

                    if (model.ExecAction == EnumActionType.Add ||
                        model.ExecAction == EnumActionType.Update)
                    {
                        if (await model.GetEditSystemWorkFlowDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowDetail.EditSystemWorkFlowDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFEdit().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWF, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowDetail.EditSystemWorkFlowDetailResult_Failure);
                                result = false;
                            }
                        }
                    }

                    if (result &&
                        model.ExecAction == EnumActionType.Add &&
                        model.UpdateWorkFlowChart(AuthState.SessionData.UserID, model.SysID, model.WFFlowID, model.WFFlowVer) == false)
                    {
                        SetSystemErrorMessage(SysResource.SystemMsg_RefreshChartFailure);
                        result = false;
                    }

                    if (result && model.ExecAction == EnumActionType.Delete)
                    {
                        if (await model.GetDeleteSystemWorkFlowDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemAlertMessage(SysSystemWorkFlowDetail.DeleteSystemWorkFlowDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFDelete().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWF, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                            {
                                SetSystemAlertMessage(SysSystemWorkFlowDetail.DeleteSystemWorkFlowDetailResult_Failure);
                                result = false;
                            }
                        }
                    }

                    if (result)
                    {
                        return RedirectToAction("SystemWorkFlow", "Sys");
                    }
                }
            }

            await SetSystemWorkFlowDetailSysParameter(model);
            return View(model);
        }

        private async Task SetSystemWorkFlowDetailSysParameter(SystemWorkFlowDetailModel model)
        {
            if (await model.GetSystemAPIGroupByIdList(model.MsgSysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetSysSystemAPIGroupList);
            }

            if (await model.GetSystemAPIFuntionList(model.MsgSysID, model.MsgControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }
        }
    }
}