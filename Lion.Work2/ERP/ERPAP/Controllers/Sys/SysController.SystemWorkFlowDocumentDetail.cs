using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowDocumentDetail(SystemWorkFlowDocumentDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add)
                {
                    if (await model.GetSystemWFDoc(CultureID))
                    {
                        SetSystemAlertMessage(SysSystemWorkFlowDocumentDetail.SystemMsg_AddSystemWorkFlowDocumentDetailExist);
                        result = false;
                    }
                    else
                    {
                        if (await model.GetInsertSystemWFDocResult(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.EditSysSystemWorkFlowDocumentDetail_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFDocumentEdit().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFDocument, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.EditSysSystemWorkFlowDocumentDetail_Failure);
                                result = false;
                            }
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetEditSystemWFDocResult(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.EditSysSystemWorkFlowDocumentDetail_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemWFDocumentEdit().SerializeToJson();

                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFDocument, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.EditSysSystemWorkFlowDocumentDetail_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult deleteResult = await model.GetDeleteSystemWFDocResult(AuthState.SessionData.UserID, ClientIPAddress(), CultureID);
                    if (deleteResult == EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemWorkFlowDocumentDetail.DeleteSysSystemWorkFlowDocumentDetail_Failure);
                        result = false;
                    }
                    else if (deleteResult == EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.RuntimeExist)
                    {
                        SetSystemAlertMessage(SysSystemWorkFlowDocumentDetail.DeleteSysSystemWorkFlowDocumentDetail_RuntimeExist);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemWFDocumentDelete().SerializeToJson();

                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFDocument, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemAlertMessage(SysSystemWorkFlowDocumentDetail.DeleteSysSystemWorkFlowDocumentDetail_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    if (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Delete)
                    {
                        if (!model.UpdateWorkFlowChart(AuthState.SessionData.UserID, model.SysID, model.WFFlowID, model.WFFlowVer))
                        {
                            SetSystemAlertMessage(SysResource.SystemMsg_RefreshChartFailure);
                        }
                    }
                    return RedirectToAction("SystemWorkFlowDocument", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.SystemMsg_GetSysSystemWFNode);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetSystemWFDoc(CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowDocumentDetail.SystemMsg_GetSystemWorkFlowDocumentDetail);
                }
            }

            return View(model);
        }
    }
}