using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowChart()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowChartModel model = new SystemWorkFlowChartModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowChartModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowChartModel.EnumCookieKey.WFFlowGroupID.ToString());
                model.WFFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowChartModel.EnumCookieKey.WFFlowID.ToString());
                model.WFFlowVer = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowChartModel.EnumCookieKey.WFFlowVer.ToString());
                model.WFCombineKey = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowChartModel.EnumCookieKey.WFCombineKey.ToString());
            }
            #endregion

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowChart);

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);
            if (model.EntitySystemWorkFlowGroupIDList != null &&
                model.EntitySystemWorkFlowGroupIDList.Count > 0 &&
                string.IsNullOrWhiteSpace(model.WFFlowGroupID))
            {
                model.WFFlowGroupID = model.EntitySystemWorkFlowGroupIDList[0].ItemValue();
            }

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);
            if (model.EntityUserSystemWorkFlowIDList != null &&
                model.EntityUserSystemWorkFlowIDList.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(model.WFFlowID) ||
                    string.IsNullOrWhiteSpace(model.WFFlowVer) ||
                    string.IsNullOrWhiteSpace(model.WFCombineKey))
                {
                    model.WFFlowID = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_ID;
                    model.WFFlowVer = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_VER;
                    model.WFCombineKey = model.EntityUserSystemWorkFlowIDList[0].ItemValue();
                }
                else
                {
                    bool isExist = false;
                    foreach (SysModel.UserSystemWorkFlowIDs entityUserSystemWorkFlowID in model.EntityUserSystemWorkFlowIDList)
                    {
                        if (model.WFCombineKey == entityUserSystemWorkFlowID.ItemValue())
                        {
                            isExist = true;
                            model.WFFlowID = entityUserSystemWorkFlowID.WF_FLOW_ID;
                            model.WFFlowVer = entityUserSystemWorkFlowID.WF_FLOW_VER;
                        }
                    }
                    if (!isExist)
                    {
                        model.WFFlowID = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_ID;
                        model.WFFlowVer = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_VER;
                        model.WFCombineKey = model.EntityUserSystemWorkFlowIDList[0].ItemValue();
                    }
                }
            }
            else
            {
                model.WFFlowID = null;
                model.WFFlowVer = null;
                model.WFCombineKey = null;
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (!string.IsNullOrWhiteSpace(model.SysID) &&
                !string.IsNullOrWhiteSpace(model.WFFlowID) &&
                !string.IsNullOrWhiteSpace(model.WFFlowVer))
            {
                if (!await model.GetSystemWFNodePosition())
                {
                    SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSystemWFNodePosition);
                }

                if (!await model.GetSystemWFArrowPosition())
                {
                    SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSystemWFArrowPosition);
                }
            }
            if (!string.IsNullOrWhiteSpace(model.SysID) &&
                !string.IsNullOrWhiteSpace(model.WFCombineKey) &&
                !model.GetFileDataURI())
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetFileDataURI);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowChart(SystemWorkFlowChartModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowChart);

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);
            if (model.EntitySystemWorkFlowGroupIDList != null &&
                model.EntitySystemWorkFlowGroupIDList.Count > 0 &&
                string.IsNullOrWhiteSpace(model.WFFlowGroupID))
            {
                model.WFFlowGroupID = model.EntitySystemWorkFlowGroupIDList[0].ItemValue();
            }

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);
            if (model.EntityUserSystemWorkFlowIDList != null &&
                model.EntityUserSystemWorkFlowIDList.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(model.WFFlowID) ||
                    string.IsNullOrWhiteSpace(model.WFFlowVer) ||
                    string.IsNullOrWhiteSpace(model.WFCombineKey))
                {
                    model.WFFlowID = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_ID;
                    model.WFFlowVer = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_VER;
                    model.WFCombineKey = model.EntityUserSystemWorkFlowIDList[0].ItemValue();
                }
                else
                {
                    bool isExist = false;
                    foreach (SysModel.UserSystemWorkFlowIDs entityUserSystemWorkFlowID in model.EntityUserSystemWorkFlowIDList)
                    {
                        if (model.WFCombineKey == entityUserSystemWorkFlowID.ItemValue())
                        {
                            isExist = true;
                            model.WFFlowID = entityUserSystemWorkFlowID.WF_FLOW_ID;
                            model.WFFlowVer = entityUserSystemWorkFlowID.WF_FLOW_VER;
                        }
                    }
                    if (!isExist)
                    {
                        model.WFFlowID = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_ID;
                        model.WFFlowVer = model.EntityUserSystemWorkFlowIDList[0].WF_FLOW_VER;
                        model.WFCombineKey = model.EntityUserSystemWorkFlowIDList[0].ItemValue();
                    }
                }
            }
            else
            {
                model.WFFlowID = null;
                model.WFFlowVer = null;
                model.WFCombineKey = null;
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowChartModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (!await model.GetSystemWFNodePosition())
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSystemWFNodePosition);
            }

            if (!await model.GetSystemWFArrowPosition())
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetSystemWFArrowPosition);
            }

            if (!string.IsNullOrWhiteSpace(model.SysID) &&
                !string.IsNullOrWhiteSpace(model.WFCombineKey) &&
                !model.GetFileDataURI())
            {
                SetSystemErrorMessage(SysSystemWorkFlowChart.SystemMsg_GetFileDataURI);
            }

            return View(model);
        }
    }
}