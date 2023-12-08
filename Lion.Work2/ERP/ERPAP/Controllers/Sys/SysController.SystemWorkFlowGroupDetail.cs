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
        public async Task<ActionResult> SystemWorkFlowGroupDetail(SystemWorkFlowGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroupDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (IsPostBack == false)
            {
                model.FormReset();

                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetSystemWorkFlowGroupDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowGroupDetail.SystemMsg_GetSystemWorkFlowGroupDetail);
                    }
                }
            }
            else
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    return RedirectToAction("SystemWorkFlowGroup", "Sys");
                }

                if (TryValidatableObject(model))
                {
                    bool result = true;

                    if (model.ExecAction == EnumActionType.Add ||
                        model.ExecAction == EnumActionType.Update)
                    {
                        if (await model.GetEditSystemWorkFlowGroupDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemErrorMessage(SysSystemWorkFlowGroupDetail.EditSystemWorkFlowGroupDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFGroupEdit().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFGroup, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowGroupDetail.EditSystemWorkFlowGroupDetailResult_Failure);
                                result = false;
                            }
                        }
                    }
                    else if (model.ExecAction == EnumActionType.Delete)
                    {
                        if (await model.GetDeleteSystemWorkFlowGroupDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                        {
                            SetSystemAlertMessage(SysSystemWorkFlowGroupDetail.DeleteSystemWorkFlowGroupDetailResult_Failure);
                            result = false;
                        }
                        else if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemWFGroupDelete().SerializeToJson();

                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemWFGroup, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemWorkFlowGroupDetail.DeleteSystemWorkFlowGroupDetailResult_Failure);
                                result = false;
                            }
                        }
                    }

                    if (result)
                    {
                        return RedirectToAction("SystemWorkFlowGroup", "Sys");
                    }
                }
            }

            return View(model);
        }
    }
}