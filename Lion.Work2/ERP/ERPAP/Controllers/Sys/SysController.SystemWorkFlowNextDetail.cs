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
        public async Task<ActionResult> SystemWorkFlowNextDetail(SystemWorkFlowNextDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack == false)
            {
                if (model.ExecAction == EnumActionType.Add)
                {
                    model.FormReset();
                }
                else if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetSystemWFNext(CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowNextDetail.SystemMsg_GetSystemWFNext);
                    }
                }
            }
            else
            {
                if (TryValidatableObject(model))
                {
                    bool result = true;

                    if (model.ExecAction == EnumActionType.Add &&
                       await model.GetSystemWFNext(CultureID))
                    {
                        SetSystemAlertMessage(SysSystemWorkFlowNextDetail.SystemMsg_AddSystemWorkFlowNextDetailExist);
                        result = false;
                    }

                    if (model.ExecAction == EnumActionType.Add ||
                        model.ExecAction == EnumActionType.Update)
                    {
                        if (await model.GetEditSystemWFNext(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowNextDetail.EditSystemWorkFlowNextDetailResult_Failure);
                            result = false;
                        }
                    }

                    if (model.ExecAction == EnumActionType.Delete)
                    {
                        if (await model.GetDeleteSystemWFNext(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemAlertMessage(SysSystemWorkFlowNextDetail.DeleteSystemWorkFlowNextDetailResult_Failure);
                            result = false;
                        }
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
                        return RedirectToAction("SystemWorkFlowNext", "Sys");
                    }
                }
            }

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNextDetail.SystemMsg_GetSysSystemWFNode);
            }

            if (await model.GetSystemWFNodeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNextDetail.SystemMsg_GetSystemWFNodeList);
            }
            
            return View(model);
        }
    }
}