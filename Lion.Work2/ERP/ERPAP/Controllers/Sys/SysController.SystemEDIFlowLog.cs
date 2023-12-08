using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public async Task<ActionResult> SystemEDIFlowLog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowLogModel model = new SystemEDIFlowLogModel();

            model.FormReset();

            if (base.FunToolData.HasFunToolPara())
            {
                model.QuerySysID = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.QuerySysID.ToString()];
                model.QueryEDIFlowID = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString()];
                model.DataDate = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.DataDate.ToString()];
                model.EDIDate = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.EDIDate.ToString()];
                model.EDINO = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.EDINO.ToString()];
                model.OnlyQuery = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.OnlyQuery.ToString()];
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLog);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                var sysSystemSysId = model.UserSystemByIdList.FirstOrDefault();
                if (sysSystemSysId != null)
                {
                    model.QuerySysID = sysSystemSysId.SysID;
                }
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID, useNullFlow: true) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSystemEDIFlowLogList(model.QuerySysID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSystemEDIFlowLogList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public async Task<ActionResult> SystemEDIFlowLog(SystemEDIFlowLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLog);

            if (base.IsPostBack)
            {
                _BaseModel.EnumSysFunToolStatus status = model.GetSysFunToolStatus();

                if (status == _BaseModel.EnumSysFunToolStatus.Query)
                {
                    if (base.FunToolData.HasFunToolPara())
                    {
                        model.QuerySysID = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.QuerySysID.ToString()];
                        model.QueryEDIFlowID = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString()];
                        model.DataDate = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.DataDate.ToString()];
                        model.EDIDate = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.EDIDate.ToString()];
                        model.EDINO = base.FunToolData.ParaDict[SystemEDIFlowLogModel.Field.EDINO.ToString()];
                    }
                }

                if (status == _BaseModel.EnumSysFunToolStatus.Create || status == _BaseModel.EnumSysFunToolStatus.Update)
                {
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemEDIFlowLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDIDate.ToString(), model.EDIDate);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDINO.ToString(), model.EDINO);
                    paraDict.Add(SystemEDIFlowLogModel.Field.DataDate.ToString(), model.DataDate);

                    if (base.HasFunToolDataList)
                    {
                        model.SetSysFunToolPara(base.UserSystemFunKey, paraDict);
                    }
                    else
                    {
                        model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.FirstNo), model.ToolNM, paraDict);
                    }
                }

                if (model.ExecAction == EnumActionType.Delete)
                {
                    if (await model.UpdateWaitStatusLog(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_UpdateWaitStatusLog);
                    }
                }
            }
            else
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    model.FormReset();
                }
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID, useNullFlow: true) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSystemEDIFlowLogList(model.QuerySysID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSystemEDIFlowLogList);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemEDIFlowLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDIDate.ToString(), model.EDIDate);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDINO.ToString(), model.EDINO);
                    paraDict.Add(SystemEDIFlowLogModel.Field.DataDate.ToString(), model.DataDate);
                    paraDict.Add(SystemEDIFlowLogModel.Field.OnlyQuery.ToString(), model.OnlyQuery);

                    model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo), paraDict);
                }

                if (model.ExecAction == EnumActionType.Query == true)
                {
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            return View(model);
        }
    }
}