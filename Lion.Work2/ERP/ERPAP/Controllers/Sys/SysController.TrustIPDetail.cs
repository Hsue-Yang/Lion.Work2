using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> TrustIPDetail(TrustIPDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                bool isChangeIP = model.ExecAction == EnumActionType.Update && (model.IPBeginOriginal != model.IPBegin || model.IPEndOriginal != model.IPEnd);

                if ((model.ExecAction == EnumActionType.Add || isChangeIP) && 
                    await model.GetTrustIPDetail(CultureID))
                {
                    SetSystemAlertMessage(SysTrustIPDetail.SystemMsg_AddTrustIPDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || isChangeIP) && 
                    await model.GetValidTrustIPRepeated(CultureID))
                {
                    SetSystemAlertMessage(SysTrustIPDetail.SystemMsg_GetValidTrustIPRepeated);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    TrustIPDetailModel.EnumModifyResult modifyResult = await model.GetEditTrustIPDetailResult(
                        AuthState.SessionData.UserNM, AuthState.SessionData.UserID, ClientIPAddress(), CultureID);

                    switch (modifyResult)
                    {
                        case TrustIPDetailModel.EnumModifyResult.Failure:
                            SetSystemErrorMessage(SysTrustIPDetail.EditTrustIPDetailResult_Failure);
                            result = false;
                            break;
                        case TrustIPDetailModel.EnumModifyResult.SyncASPFailure:
                            SetSystemErrorMessage(SysTrustIPDetail.EditTrustIPDetailResult_SyncASPFailure);
                            result = false;
                            break;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    TrustIPDetailModel.EnumModifyResult modifyResult = await model.GetDeleteTrustIPDetailResult(
                        AuthState.SessionData.UserID, ClientIPAddress(), CultureID);

                    switch (modifyResult)
                    {
                        case TrustIPDetailModel.EnumModifyResult.Failure:
                            SetSystemErrorMessage(SysTrustIPDetail.DeleteTrustIPDetailResult_Failure);
                            result = false;
                            break;
                        case TrustIPDetailModel.EnumModifyResult.SyncASPFailure:
                            SetSystemErrorMessage(SysTrustIPDetail.DeleteTrustIPDetailResult_SyncASPFailure);
                            result = false;
                            break;
                    }
                }

                if (result)
                {
                    return RedirectToAction("TrustIP", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetBaseRawCMOrgComList() == false)
            {
                SetSystemErrorMessage(SysTrustIPDetail.SystemMsg_GetBaseRawCMOrgComList);
            }

            if (model.GetBaseTrustTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIPDetail.SystemMsg_GetBaseTrustTypeList);
            }

            if (model.GetBaseSourceTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIPDetail.SystemMsg_GetBaseSourceTypeList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetTrustIPDetail(CultureID) == false)
                {
                    SetSystemErrorMessage(SysTrustIPDetail.SystemMsg_GetTrustIPDetail);
                }
                else
                {
                    model.IPBeginOriginal = model.IPBegin;
                    model.IPEndOriginal = model.IPEnd;
                    model.ComID = model.EntityTrustIPDetail.ComID;
                    model.TrustStatus = model.EntityTrustIPDetail.TrustStatus;
                    model.TrustType = model.EntityTrustIPDetail.TrustType;
                    model.SourceType = model.EntityTrustIPDetail.SourceType;
                    model.Remark = model.EntityTrustIPDetail.Remark;
                    model.SortOrder = model.EntityTrustIPDetail.SortOrder;
                }
            }

            return View(model);
        }
    }
}