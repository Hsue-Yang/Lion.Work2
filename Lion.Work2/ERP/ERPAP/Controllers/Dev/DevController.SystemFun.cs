using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Dev;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class DevController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public ActionResult SystemFun()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunModel model = new SystemFunModel();

            model.FormReset();

            if (base.FunToolData.HasFunToolPara())
            {
                model.QuerySysID = base.FunToolData.ParaDict[SystemFunModel.Field.QuerySysID.ToString()];
                model.QueryFunControllerID = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunControllerID.ToString()];
                model.QueryFunMenuSysID = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunMenuSysID.ToString()];
                model.QueryFunMenu = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunMenu.ToString()];
                model.OnlyEvent = base.FunToolData.ParaDict[SystemFunModel.Field.OnlyEvent.ToString()];
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QueryFunMenuSysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemFunMenuList);
            }

            if (model.GetSystemFunList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSystemFunList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        [UseFunTool(true)]
        public ActionResult SystemFun(SystemFunModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack)
            {
                _BaseModel.EnumSysFunToolStatus status = model.GetSysFunToolStatus();

                if (status == _BaseModel.EnumSysFunToolStatus.Query)
                {
                    if (base.FunToolData.HasFunToolPara())
                    {
                        model.QuerySysID = base.FunToolData.ParaDict[SystemFunModel.Field.QuerySysID.ToString()];
                        model.QueryFunControllerID = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunControllerID.ToString()];
                        model.QueryFunMenuSysID = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunMenuSysID.ToString()];
                        model.QueryFunMenu = base.FunToolData.ParaDict[SystemFunModel.Field.QueryFunMenu.ToString()];
                        model.OnlyEvent = base.FunToolData.ParaDict[SystemFunModel.Field.OnlyEvent.ToString()];
                    }
                }

                if (status == _BaseModel.EnumSysFunToolStatus.Create || status == _BaseModel.EnumSysFunToolStatus.Update)
                {
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemFunModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemFunModel.Field.QueryFunControllerID.ToString(), model.QueryFunControllerID);
                    paraDict.Add(SystemFunModel.Field.QueryFunMenuSysID.ToString(), model.QueryFunMenuSysID);
                    paraDict.Add(SystemFunModel.Field.QueryFunMenu.ToString(), model.QueryFunMenu);
                    paraDict.Add(SystemFunModel.Field.OnlyEvent.ToString(), model.OnlyEvent);
                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);

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

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QueryFunMenuSysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSysSystemFunMenuList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemFunModel.Field.QueryFunControllerID.ToString(), model.QueryFunControllerID);
                paraDict.Add(SystemFunModel.Field.QueryFunMenuSysID.ToString(), model.QueryFunMenuSysID);
                paraDict.Add(SystemFunModel.Field.QueryFunMenu.ToString(), model.QueryFunMenu);
                paraDict.Add(SystemFunModel.Field.OnlyEvent.ToString(), model.OnlyEvent);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);

                model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo), paraDict);
                
                if (model.GetSystemFunList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(DevSystemFun.SystemMsg_GetSystemFunList);
                }
            }

            return View(model);
        }
    }
}