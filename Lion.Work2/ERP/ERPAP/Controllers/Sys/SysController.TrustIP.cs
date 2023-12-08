using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> TrustIP()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            TrustIPModel model = new TrustIPModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QueryIPBegin = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryIPBegin.ToString());
                model.QueryIPEnd = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryIPEnd.ToString());
                model.QueryComID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryComID.ToString());
                model.QueryTrustStatus = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryTrustStatus.ToString());
                model.QueryTrustType = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryTrustType.ToString());
                model.QuerySourceType = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QuerySourceType.ToString());
                model.QueryRemark = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, TrustIPModel.Field.QueryRemark.ToString());
            }
            #endregion

            if (model.GetBaseRawCMOrgComList() == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseRawCMOrgComList);
            }

            if (model.GetBaseTrustTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseTrustTypeList);
            }

            if (model.GetBaseSourceTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseSourceTypeList);
            }

            if (await model.GetTrustIPList(PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetTrustIPList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> TrustIP(TrustIPModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            
            #region - Set Cookie -
            Dictionary<string, string> cookieDict =
                new Dictionary<string, string>
                {
                    { TrustIPModel.Field.QueryIPBegin.ToString(), model.QueryIPBegin },
                    { TrustIPModel.Field.QueryIPEnd.ToString(), model.QueryIPEnd },
                    { TrustIPModel.Field.QueryComID.ToString(), model.QueryComID },
                    { TrustIPModel.Field.QueryTrustStatus.ToString(), model.QueryTrustStatus },
                    { TrustIPModel.Field.QueryTrustType.ToString(), model.QueryTrustType },
                    { TrustIPModel.Field.QuerySourceType.ToString(), model.QuerySourceType },
                    { TrustIPModel.Field.QueryRemark.ToString(), model.QueryRemark }
                };
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, cookieDict);
            #endregion

            if (model.GetBaseRawCMOrgComList() == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseRawCMOrgComList);
            }

            if (model.GetBaseTrustTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseTrustTypeList);
            }

            if (model.GetBaseSourceTypeList(CultureID) == false)
            {
                SetSystemErrorMessage(SysTrustIP.SystemMsg_GetBaseSourceTypeList);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                if (await model.GetTrustIPList(PageSize, CultureID) == false)
                {
                    SetSystemErrorMessage(SysTrustIP.SystemMsg_GetTrustIPList);
                }
            }

            return View(model);
        }
    }
}