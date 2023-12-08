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
        public async Task<ActionResult> SystemEDIJobLog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIJobLogModel model = new SystemEDIJobLogModel();

            model.FormReset();

            if(base.FunToolData.HasFunToolPara())
            {
                model.QuerySysID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QuerySysID.ToString()];
                model.QueryEDIFlowID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString()];
                model.QueryEDIJobID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QueryEDIJobID.ToString()];
                model.DataDate = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.DataDate.ToString()];
                model.EDIDate = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIDate.ToString()];
                model.EDINO = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDINO.ToString()];
                model.EDIFlowIDSearch = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIFlowIDSearch.ToString()];
                model.EDIJobIDSearch = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIJobIDSearch.ToString()];
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJobLog);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysUserSystemSysIDList);
            }
         
            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                var sysSystemSysId = model.UserSystemByIdList.FirstOrDefault();
                if (sysSystemSysId != null)
                {
                    model.QuerySysID = sysSystemSysId.SysID;
                }
            }

            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                model.QuerySysID = model.UserSystemByIdList[0].SysID;
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID, useNullFlow: true) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (string.IsNullOrWhiteSpace(model.QueryEDIFlowID))
            {
                model.QueryEDIFlowID = model.EntitySysSystemEDIFlowList[0].EDIFlowID;
            }

            if (await model.GetSysSystemEDIJobList(AuthState.SessionData.UserID, model.QuerySysID, model.QueryEDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSystemEDIJobLogList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSystemEDIJobLogList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public async Task<ActionResult> SystemEDIJobLog(SystemEDIJobLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJobLog);

            if (base.IsPostBack)
            {
                _BaseModel.EnumSysFunToolStatus status = model.GetSysFunToolStatus();

                if (status == _BaseModel.EnumSysFunToolStatus.Query)
                {
                    if (base.FunToolData.HasFunToolPara())
                    {
                        model.QuerySysID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QuerySysID.ToString()];
                        model.QueryEDIFlowID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString()];
                        model.QueryEDIJobID = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.QueryEDIJobID.ToString()];
                        model.DataDate = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.DataDate.ToString()];
                        model.EDIDate = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIDate.ToString()];
                        model.EDINO = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDINO.ToString()];
                        model.EDIFlowIDSearch = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIFlowIDSearch.ToString()];
                        model.EDIJobIDSearch = base.FunToolData.ParaDict[SystemEDIJobLogModel.Field.EDIJobIDSearch.ToString()];
                    }
                }
                if (status == _BaseModel.EnumSysFunToolStatus.Create || status == _BaseModel.EnumSysFunToolStatus.Update)
                {
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemEDIJobLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                    paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIJobID.ToString(), model.QueryEDIJobID);
                    paraDict.Add(SystemEDIJobLogModel.Field.DataDate.ToString(), model.DataDate);
                    paraDict.Add(SystemEDIJobLogModel.Field.EDIDate.ToString(), model.EDIDate);
                    paraDict.Add(SystemEDIJobLogModel.Field.EDINO.ToString(), model.EDINO);
                    paraDict.Add(SystemEDIJobLogModel.Field.EDIFlowIDSearch.ToString(), model.EDIFlowIDSearch);
                    paraDict.Add(SystemEDIJobLogModel.Field.EDIFlowIDSearch.ToString(), model.EDIJobIDSearch);

                    if (base.HasFunToolDataList)
                    {
                        model.SetSysFunToolPara(base.UserSystemFunKey, paraDict);
                    }
                    else
                    {
                        model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.FirstNo), model.ToolNM, paraDict);
                    }
                }
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysUserSystemSysIDList);
            }            

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, base.CultureID, useNullFlow: true) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetSysSystemEDIJobList(AuthState.SessionData.UserID, model.QuerySysID, model.QueryEDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIJobList);
            }

            if (await model.GetSystemEDIJobLogList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIJobList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIJobLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIJobID.ToString(), model.QueryEDIJobID);
                paraDict.Add(SystemEDIJobLogModel.Field.DataDate.ToString(), model.DataDate);
                paraDict.Add(SystemEDIJobLogModel.Field.EDIDate.ToString(), model.EDIDate);
                paraDict.Add(SystemEDIJobLogModel.Field.EDINO.ToString(), model.EDINO);
                paraDict.Add(SystemEDIJobLogModel.Field.EDIFlowIDSearch.ToString(), model.EDIFlowIDSearch);
                paraDict.Add(SystemEDIJobLogModel.Field.EDIJobIDSearch.ToString(), model.EDIJobIDSearch);

                model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo), paraDict);
            }

            return View(model);
        }
    }
}