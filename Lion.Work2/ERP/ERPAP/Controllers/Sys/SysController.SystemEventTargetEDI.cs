using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEventTargetEDI(SystemEventTargetEDIModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    (model.IsITManager ||
                     (string.IsNullOrWhiteSpace(model.TargetSysID) == false && AuthState.SessionData.UserRoleIDs.Contains($"{model.TargetSysID}{EntitySys.EnumSystemRoleID.IT}"))))
                {
                    string eventParaJson = null;
                    string ediEventNo = null;

                    if (string.IsNullOrWhiteSpace(model.TargetSysID) == false)
                    {
                        string filePath = GetFilePathFolderPath(
                            EnumFilePathFolder.EDIServiceEventPara,
                            new[] { model.ExecEDIEventNo.Substring(0, 8), model.ExecEDIEventNo });

                        eventParaJson = model.GetEDIEventPara(filePath);

                        model.ExecEDIEventNo = null;
                    }

                    if (model.ExcuteSubscription(AuthState.SessionData.UserID, ref ediEventNo) == false)
                    {
                        SetSystemErrorMessage(SysSystemEventTargetEDI.InsertSubscriptiont_Failure);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(eventParaJson) == false)
                        {
                            string filePath = GetFilePathFolderPath(
                                EnumFilePathFolder.EDIServiceEventPara,
                                new[] { ediEventNo.Substring(0, 8), ediEventNo });
                            int reTryCount = 0;

                            do
                            {
                                Common.FileWriteStream(filePath, eventParaJson);
                                if (System.IO.File.Exists(filePath))
                                {
                                    break;
                                }
                                reTryCount++;
                            } while (reTryCount < 10);
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.DTBegin) || string.IsNullOrWhiteSpace(model.DTEnd))
                {
                    model.FormReset();
                }
            }

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventFullName = model.GetSysSystemEventFullName(model.SysID, AuthState.SessionData.UserID, model.EventGroupID, model.EventID, CultureID);
            Task<bool> getSystemEventTargetEDIList = model.GetSystemEventTargetEDIList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSysSystemEventFullName, getSystemEventTargetEDIList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTargetEDI.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemEventFullName.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTargetEDI.SystemMsg_UnGetSysSystemEventFullName);
            }

            if (getSystemEventTargetEDIList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventTargetEDI.SystemMsg_UnGetSystemEventTargetEDIList);
            }

            return View(model);
        }
    }
}