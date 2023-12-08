using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity;
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
        public async Task<ActionResult> SystemRecord()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemRecordModel model = new SystemRecordModel();

            model.GetSysADSTabList(_BaseAPModel.EnumTabAction.SysSystemRecord);

            model.FormReset();

            if (IsPostBack)
            {
                model.RecordType = FunToolData.ParaDict[SystemRecordModel.Field.RecordType.ToString()];
                model.IsOnlyDiffData = FunToolData.ParaDict[SystemRecordModel.Field.IsOnlyDiffData.ToString()];

                model.DateBegin = FunToolData.ParaDict[SystemRecordModel.Field.DateBegin.ToString()];
                model.DateEnd = FunToolData.ParaDict[SystemRecordModel.Field.DateEnd.ToString()];
                model.TimeBegin = FunToolData.ParaDict[SystemRecordModel.Field.TimeBegin.ToString()];
                model.TimeEnd = FunToolData.ParaDict[SystemRecordModel.Field.TimeEnd.ToString()];

                model.UserID = FunToolData.ParaDict[SystemRecordModel.Field.UserID.ToString()];
                model.SysID = FunToolData.ParaDict[SystemRecordModel.Field.SysID.ToString()];
                model.FunControllerID = FunToolData.ParaDict[SystemRecordModel.Field.FunControllerID.ToString()];
                model.FunActionName = FunToolData.ParaDict[SystemRecordModel.Field.FunActionName.ToString()];

            }

            if (model.GetBaseRecordTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetBaseRecordTypeList);
            }

            if (await model.GetSystemSysIDList(true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemSysIDList);
            }

            if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (await model.GetSystemFunNameList(model.SysID, model.FunControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemFunNameList);
            }

            if (await model.GetSysSystemRoleGroupList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemRoleGroupList);
            }

            if (model.GetLineBotIDList(AuthState.SessionData.UserID, model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetLineBotIDList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public async Task<ActionResult> SystemRecord(SystemRecordModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysADSTabList(_BaseAPModel.EnumTabAction.SysSystemRecord);

            if (IsPostBack)
            {
                _BaseModel.EnumSysFunToolStatus status = model.GetSysFunToolStatus();

                if (status == _BaseModel.EnumSysFunToolStatus.Query)
                {
                    if (FunToolData.HasFunToolPara())
                    {
                        model.RecordType = FunToolData.ParaDict[SystemRecordModel.Field.RecordType.ToString()];
                        model.IsOnlyDiffData = FunToolData.ParaDict[SystemRecordModel.Field.IsOnlyDiffData.ToString()];

                        model.DateBegin = FunToolData.ParaDict[SystemRecordModel.Field.DateBegin.ToString()];
                        model.DateEnd = FunToolData.ParaDict[SystemRecordModel.Field.DateEnd.ToString()];
                        model.TimeBegin = FunToolData.ParaDict[SystemRecordModel.Field.TimeBegin.ToString()];
                        model.TimeEnd = FunToolData.ParaDict[SystemRecordModel.Field.TimeEnd.ToString()];

                        model.UserID = FunToolData.ParaDict[SystemRecordModel.Field.UserID.ToString()];
                        model.SysID = FunToolData.ParaDict[SystemRecordModel.Field.SysID.ToString()];
                        model.FunControllerID = FunToolData.ParaDict[SystemRecordModel.Field.FunControllerID.ToString()];
                        model.FunActionName = FunToolData.ParaDict[SystemRecordModel.Field.FunActionName.ToString()];
                    }
                }

                if (status == _BaseModel.EnumSysFunToolStatus.Create || status == _BaseModel.EnumSysFunToolStatus.Update)
                {
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemRecordModel.Field.RecordType.ToString(), model.RecordType);
                    paraDict.Add(SystemRecordModel.Field.IsOnlyDiffData.ToString(), (model.IsOnlyDiffData == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : EnumYN.N.ToString());

                    paraDict.Add(SystemRecordModel.Field.DateBegin.ToString(), model.DateBegin);
                    paraDict.Add(SystemRecordModel.Field.DateEnd.ToString(), model.DateEnd);
                    paraDict.Add(SystemRecordModel.Field.TimeBegin.ToString(), model.TimeBegin);
                    paraDict.Add(SystemRecordModel.Field.TimeEnd.ToString(), model.TimeEnd);

                    paraDict.Add(SystemRecordModel.Field.UserID.ToString(), model.UserID);
                    paraDict.Add(SystemRecordModel.Field.SysID.ToString(), model.SysID);
                    paraDict.Add(SystemRecordModel.Field.FunControllerID.ToString(), model.FunControllerID);
                    paraDict.Add(SystemRecordModel.Field.FunActionName.ToString(), model.FunActionName);

                    if (HasFunToolDataList)
                    {
                        model.SetSysFunToolPara(UserSystemFunKey, paraDict);
                    }
                    else
                    {
                        model.SetSysFunToolPara(UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.FirstNo), model.ToolNM, paraDict);
                    }
                }
            }

            if (model.GetBaseRecordTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetBaseRecordTypeList);
            }

            if (await model.GetSystemSysIDList(true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemSysIDList);
            }

            if (await model.GetSysSystemConditionIDList(model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemConditionIDList);
            }

            if (model.GetLineBotIDList(AuthState.SessionData.UserID, model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetLineBotIDList_Failure);
            }

            if (await model.GetSysSystemFunControllerIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (await model.GetSystemFunNameList(model.SysID, model.FunControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemFunNameList);
            }

            if (await model.GetSysSystemRoleGroupList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSysSystemRoleGroupList);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemRecordModel.Field.RecordType.ToString(), model.RecordType);
                paraDict.Add(SystemRecordModel.Field.IsOnlyDiffData.ToString(), (model.IsOnlyDiffData == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : EnumYN.N.ToString());

                paraDict.Add(SystemRecordModel.Field.DateBegin.ToString(), model.DateBegin);
                paraDict.Add(SystemRecordModel.Field.DateEnd.ToString(), model.DateEnd);
                paraDict.Add(SystemRecordModel.Field.TimeBegin.ToString(), model.TimeBegin);
                paraDict.Add(SystemRecordModel.Field.TimeEnd.ToString(), model.TimeEnd);

                paraDict.Add(SystemRecordModel.Field.UserID.ToString(), model.UserID);
                paraDict.Add(SystemRecordModel.Field.SysID.ToString(), model.SysID);
                paraDict.Add(SystemRecordModel.Field.FunControllerID.ToString(), model.FunControllerID);
                paraDict.Add(SystemRecordModel.Field.FunActionName.ToString(), model.FunActionName);

                model.SetSysFunToolPara(UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo), paraDict);

                if (await model.GetSystemRecordList(PageSize, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetSystemRecordList);
                }
            }
            
            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRecord")]
        public ActionResult SystemRecordSysRoleConditionDetail(SystemRecordModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.SetSysSystemRoleConditionDetailData(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetRoleConditionJsonString_Failure);
            }
            
            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRecord")]
        public async Task<ActionResult> GetFunctionGroupList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemFunControllerIDList(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunControllerIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRecord")]
        public async Task<ActionResult> GetFunctionNameList(string sysID, string funControllerID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemFunNameList(sysID, funControllerID, CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunNameList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRecord")]
        public ActionResult SystemRecordUserPurviewDetail(SystemRecordModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.SetSysUserPurviewDetailData(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetRoleConditionJsonString_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRecord")]
        public async Task<ActionResult> SystemRecordUserApplyList(SystemRecordModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.GetBaseRecordTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetBaseRecordTypeList);
            }

            if (await model.GetLogUserApplyList() == false)
            {
                SetSystemErrorMessage(SysSystemRecord.SystemMsg_GetLogUserApplyList_Failure);
            }

            return View(model);
        }
    }
}