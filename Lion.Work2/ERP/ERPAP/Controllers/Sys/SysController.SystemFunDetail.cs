using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using static ERPAP.Models._BaseAPModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunDetail(SystemFunDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;
            model.GetSysSystemFunTabList(_BaseAPModel.EnumTabAction.SysSystemFunDetail);

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemFunDetail(AuthState.SessionData.UserID, CultureID) == true)
                {
                    SetSystemAlertMessage(SysSystemFunDetail.SystemMsg_IsExistSystemFunDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemFunDetail(model.ExecAction, AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunDetail.EditSystemFunDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunDetail.EditSystemFunDetailResult_Failure);
                            result = false;
                        }

                        eventParaJsonString = model.GetEventParaSysSystemFunMenuEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFunMenu, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunDetail.EditSystemFunDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    SystemFunDetailModel.EnumDeleteSystemFunDetailResult deleteSystemFunDetailResult = await model.DeleteSystemFunDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID);

                    if (deleteSystemFunDetailResult == SystemFunDetailModel.EnumDeleteSystemFunDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunDetail.DeleteSystemFunDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunDetailResult == SystemFunDetailModel.EnumDeleteSystemFunDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemFunDetail.DeleteSystemFunDetailResult_DataExist);
                        result = false;
                    }
                    else if (deleteSystemFunDetailResult == SystemFunDetailModel.EnumDeleteSystemFunDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunDetail.DeleteSystemFunDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemFun", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemSubByIds = model.GetSystemSubByIds(model.SysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemPurviewByIdList = model.GetSystemPurviewByIdList(model.SysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemFunGroupByIdList = model.GetSystemFunGroupByIdList(model.SysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemFunTypeList = model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.SystemFunType), CultureID);
            Task<bool> getSystemFunRoleList = model.GetSystemFunRoleList(model.SysID, AuthState.SessionData.UserID, model.FunControllerID, model.FunActionName, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSystemSubByIds, getSystemPurviewByIdList,
                getSystemFunGroupByIdList, getSystemFunTypeList, getSystemFunRoleList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetAllSystemByIdList);
            }

            if (getSystemSubByIds.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemSubByIdList);
            }

            if (getSystemPurviewByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemPurviewByIdList);
            }

            if (getSystemFunGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemFunGroupByIdList);
            }

            if (getSystemFunTypeList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemFunTypeList);
            }

            if (getSystemFunRoleList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemRoleFunList);
            }

            if ((model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Copy) && 
                await model.GetSystemFunDetail(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunDetail.SystemMsg_UnGetSystemFunDetail);
            }

            return View(model);
        }
    }
}