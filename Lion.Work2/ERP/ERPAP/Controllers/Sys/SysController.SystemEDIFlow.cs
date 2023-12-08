using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.Sockets;
using LionTech.Web.ERPHelper;
using Resources;


namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIFlow()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowModel model = new SystemEDIFlowModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowModel.Field.QuerySysID.ToString());
                model.QuerySCHFrequency = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowModel.Field.QuerySCHFrequency.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlow);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSystemEDIFlowList);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0005", base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSCHFrequencyList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIFlow(SystemEDIFlowModel model, List<SystemEDIFlowModel.EDIFlowValue> EDIFlowValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetEDIFlowSettingResult(AuthState.SessionData.UserID, EDIFlowValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_SaveEDIFlowSortOrderError);
                }
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlow);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSystemEDIFlowList);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0005", base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_GetSCHFrequencyList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIFlowModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIFlowModel.Field.QuerySCHFrequency.ToString(), model.QuerySCHFrequency);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Copy)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIFlowModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIFlowModel.Field.QuerySCHFrequency.ToString(), model.QuerySCHFrequency);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                //輸出xml進入點
                if (await model.SaveEDIXML(model.QuerySysID, base.CultureID, AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemEDIFlow.SysMsg_OutputXMLOK);
                }
                else
                {
                    SetSystemErrorMessage(SysSystemEDIFlow.SystemMsg_SaveEDIXML);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIFlow")]
        public async Task<ActionResult> SystemEDIServiceStatus(SystemEDIFlowModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain) &&
                Enum.IsDefined(typeof(EnumEDISystemID), model.QuerySysID) &&
                model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) &&
                model.IsITManager)
            {
                string ipAddress = await model.GetSystemEDIIPAddress(AuthState.SessionData.UserID, model.QuerySysID);
                if (string.IsNullOrWhiteSpace(ipAddress) == false)
                {
                    int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);
                    string serviceIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.ServiceControllerIPAddress.ToString()];
                    int servicePort = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.ServiceControllerPort.ToString()]);
                    ServiceControllerTcpClient service = new ServiceControllerTcpClient(serviceIPAddress, servicePort);
                    service.ServiceControllerAction = new ServiceControllerAction();
                    service.ServiceControllerAction.MachineName = ipAddress;
                    service.ServiceControllerAction.ServiceName = Common.GetEnumDesc(Utility.GetEnumEDISystemID(model.QuerySysID));
                    service.ServiceControllerAction.ExcuteAction = ServiceControllerAction.EnumAction.Status.ToString();

                    string serviceStatus = service.Connect(apiTimeOut);

                    if (string.IsNullOrWhiteSpace(serviceStatus) == false)
                    {
                        return Json(new
                        {
                            isSuccess = true,
                            serviceStatus,
                            statusName = GetServiceControllerStatusName(serviceStatus)
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIFlow")]
        public async Task<ActionResult> SystemEDIServiceExcute(SystemEDIFlowModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain) &&
                Enum.IsDefined(typeof(EnumEDISystemID), model.QuerySysID) &&
                model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) &&
                model.IsITManager)
            {
                string ipAddress = await model.GetSystemEDIIPAddress(AuthState.SessionData.UserID, model.QuerySysID);
                if (string.IsNullOrWhiteSpace(ipAddress) == false)
                {
                    int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);
                    string serviceIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.ServiceControllerIPAddress.ToString()];
                    int servicePort = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.ServiceControllerPort.ToString()]);
                    ServiceControllerTcpClient service = new ServiceControllerTcpClient(serviceIPAddress, servicePort);
                    service.ServiceControllerAction = new ServiceControllerAction();
                    service.ServiceControllerAction.MachineName = ipAddress;
                    service.ServiceControllerAction.ServiceName = Common.GetEnumDesc(Utility.GetEnumEDISystemID(model.QuerySysID));

                    if (model.ServiceStatus == ServiceControllerStatus.Running.ToString())
                    {
                        service.ServiceControllerAction.ExcuteAction = ServiceControllerAction.EnumAction.Stop.ToString();
                    }
                    else if (model.ServiceStatus == ServiceControllerStatus.Stopped.ToString())
                    {
                        service.ServiceControllerAction.ExcuteAction = ServiceControllerAction.EnumAction.Start.ToString();
                    }

                    string serviceStatus = service.Connect(apiTimeOut);

                    if (string.IsNullOrWhiteSpace(serviceStatus) == false)
                    {
                        return Json(new
                        {
                            isSuccess = true,
                            serviceStatus,
                            statusName = GetServiceControllerStatusName(serviceStatus)
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIFlow")]
        public async Task<ActionResult> SystemEDIFlowDir(SystemEDIFlowModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (Enum.IsDefined(typeof(EnumEDISystemID), model.QuerySysID))
            {
                await model.GetEDIServiceFilePath(AuthState.SessionData.UserID);

                if (IsPostBack)
                {
                    if (model.ExecAction == EnumActionType.Update)
                    {
                        return Content(model.UpdateDirFileData());
                    }
                }
                else
                {
                    if (model.GetDirFileTreeJsonString() == false)
                    {
                        SetSystemErrorMessage(SysSystemEDIFlowDir.SystemMsg_GetDirFileTreeJsonString_Failure);
                    }
                }
            }
            else
            {
                model.HasNoEDIService = true;
            }

            return View(model);
        }

        private string GetServiceControllerStatusName(string serviceStatus)
        {
            string statusName;
            switch ((ServiceControllerStatus)Enum.Parse(typeof(ServiceControllerStatus), serviceStatus))
            {
                case ServiceControllerStatus.ContinuePending:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_ContinuePending;
                    break;
                case ServiceControllerStatus.Paused:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_Paused;
                    break;
                case ServiceControllerStatus.PausePending:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_PausePending;
                    break;
                case ServiceControllerStatus.Running:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_Running;
                    break;
                case ServiceControllerStatus.StartPending:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_StartPending;
                    break;
                case ServiceControllerStatus.Stopped:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_Stopped;
                    break;
                case ServiceControllerStatus.StopPending:
                    statusName = SysSystemEDIFlow.SysMsg_ServiceControllerStatus_StopPending;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return statusName;
        }
    }
}
