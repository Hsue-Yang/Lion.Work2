using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserPurviewDetail(UserPurviewDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.LionCountryCode, Entity_BaseAP.EnumCMCodeKind.PurviewType);

            if (model.GetRawCMOrgUnitList() == false)
            {
                SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_GetRawCMOrgUnitList_Failure);
            }
            if (model.GetBaseRawCMOrgComList() == false)
            {
                SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_GetBaseRawCMOrgComList_Failure);
            }

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Update &&
                    model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false &&
                    model.IsITManager == false)
                {
                    SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_NoEditPermissions);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Update)
                {
                    if (TryValidatableObject(model))
                    {
                        if (await model.GetOriginalUserPurviewDetailList(CultureID) == false)
                        {
                            SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_GetUserPurviewDetailList_Failure);
                            result = false;
                        }

                        if (result && await model.EditSysUserPurviewDetail(AuthState.SessionData.UserID, CultureID) == false)
                        {
                            SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_EditSysUserPurview_Failure);
                            result = false;
                        }

                        if (result && await model.RecordLogSysUserPurview(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                        {
                            SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_SysUserPurviewLogData_Failure);
                            result = false;
                        }

                        if (result && model.RecordLogSysUserPurviewApply(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                        {
                            SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_RecordLogSysUserPurviewApply_Failure);
                            result = false;
                        }

                        if (result &&
                            GetEDIServiceDistributor() &&
                            model.GetUserPurviewEventList())
                        {
                            foreach (var purview in model.UserPurviewEventList)
                            {
                                string eventParaJsonString = purview.SerializeToJson();
                                if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysUserPurview, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                {
                                    SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_EditSysUserPurview_Failure);
                                    result = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                model.SaveSuccess = result;
            }
            else
            {
                model.SysNM = HttpUtility.UrlDecode(model.SysNM);
                model.UserNM = HttpUtility.UrlDecode(model.UserNM);
            }

            if (await model.GetOriginalUserPurviewDetailList(CultureID) == false)
            {
                SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_GetUserPurviewDetailList_Failure);
            }

            if (model.GetUserPurviewInfoList() == false)
            {
                SetSystemErrorMessage(SysUserPurviewDetail.SystemMsg_GetUserPurviewInfoList_Failure);
            }

            return View(model);
        }
    }
}