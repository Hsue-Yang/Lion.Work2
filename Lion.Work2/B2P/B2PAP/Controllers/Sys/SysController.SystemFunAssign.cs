using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemFunAssign(SystemFunAssignModel model, List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.CurrentCultureID = base.CultureID;
            model.GetSysSystemFunTabList(_BaseAPModel.EnumTabAction.SysSystemFunAssign);

            List<EntitySystemFunAssign.SystemFunAssign> systemFunAssignList = new List<EntitySystemFunAssign.SystemFunAssign>();
            if (model.GetSystemFunAssignList(base.CultureID))
            {
                systemFunAssignList = model.EntitySystemFunAssignList;
            }

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (model.GetEditSystemFunAssignResult(AuthState.SessionData.UserID, systemFunAssignValueList, base.CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunAssign.EditSystemFunAssignResult_Failure);
                        result = false;
                    }
                    else
                    {
                        if (systemFunAssignValueList != null)
                        {
                            if (systemFunAssignValueList.Count == 1 && string.IsNullOrWhiteSpace(systemFunAssignValueList[0].UserID))
                            {
                                if (systemFunAssignList != null && systemFunAssignList.Count > 0)
                                {
                                    foreach (EntitySystemFunAssign.SystemFunAssign systemFunAssign in model.EntitySystemFunAssignList)
                                    {
                                        base.ExecUserFunLogWrite(systemFunAssign.UserID.GetValue());
                                    }
                                }
                            }
                            else
                            {
                                foreach (EntitySystemFunAssign.SystemFunAssignValue systemFunAssignValue in systemFunAssignValueList)
                                {
                                    base.ExecUserFunLogWrite(systemFunAssignValue.UserID);
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        SetSystemAlertMessage(SysSystemFunAssign.SystemMsg_SetSystemFunAssignResultWasSuccess);
                    }
                }

                if (result && model.ExecAction == EnumActionType.Query)
                {
                    return RedirectToAction("SystemFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSystemFunInfor(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunAssign.SystemMsg_GetSystemFunInfor);
            }

            if (model.GetSystemFunAssignList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunAssign.SystemMsg_GetSystemFunAssignList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFunAssign")]
        public ActionResult GetRAWUserList(string condition)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunAssignModel model = new SystemFunAssignModel();

            model.GetRAWUserList(condition);

            if (model.EntityRAWUserList != null)
            {
                var userList = model.EntityRAWUserList.Take(20);

                var returnUser = from userData in userList
                                 select new
                                 {
                                     Text = userData.UserNM.GetValue(),
                                     Value = userData.UserID.GetValue(),
                                 };

                return Json(returnUser);
            }
            return null;
        }
    }
}