using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemCultureSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemCultureSettingModel model = new SystemCultureSettingModel();

            Task<bool> systemCultureIDs = model.GetSystemCultureIDs(AuthState.SessionData.UserID);
            Task<bool> systemCultures = model.GetSystemCultures(AuthState.SessionData.UserID, PageSize);

            await Task.WhenAll(systemCultureIDs, systemCultures);

            if (systemCultureIDs.Result == false)
            {
                SetSystemErrorMessage(SysSystemCultureSetting.SystemMsg_UnGetSystemCultureIDs);
            }

            if (systemCultures.Result == false)
            {
                SetSystemErrorMessage(SysSystemCultureSetting.SystemMsg_UnGetSystemCultures);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemCultureSetting(SystemCultureSettingModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            Task<bool> systemCultureIDs = model.GetSystemCultureIDs(AuthState.SessionData.UserID);
            Task<bool> systemCultures = model.GetSystemCultures(AuthState.SessionData.UserID, PageSize);

            await Task.WhenAll(systemCultureIDs, systemCultures);

            if (systemCultureIDs.Result == false)
            {
                SetSystemErrorMessage(SysSystemCultureSetting.SystemMsg_UnGetSystemCultureIDs);
            }

            if (systemCultures.Result == false)
            {
                SetSystemErrorMessage(SysSystemCultureSetting.SystemMsg_UnGetSystemCultures);
            }

            if (IsPostBack) 
            {
                if (model.ExecAction == EnumActionType.Update) 
                {
                    if (await model.GenerateCultureJsonFile(AuthState.SessionData.UserID) == false) 
                    {
                        SetSystemErrorMessage(SysSystemCultureSetting.SystemMsg_UnGenerateCultureJsonFile);
                    }
                }
            }

            return View(model);
        }
    }
}