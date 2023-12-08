using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using LionTech.Utility;
using Resources;
using System.Threading.Tasks;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserBasicInfo()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserBasicInfoModel model = new UserBasicInfoModel();

            model.FormReset(AuthState.SessionData.UserID);

            if (base.FunToolData.HasFunToolPara())
            {
                model.QueryUserID = base.FunToolData.ParaDict[UserBasicInfoModel.Field.QueryUserID.ToString()];
                model.QueryUserNM = base.FunToolData.ParaDict[UserBasicInfoModel.Field.QueryUserNM.ToString()];
                model.IsLeft = base.FunToolData.ParaDict[UserBasicInfoModel.Field.IsLeft.ToString()];
                model.DateBegin = base.FunToolData.ParaDict[UserBasicInfoModel.Field.DateBegin.ToString()];
                model.DateEnd = base.FunToolData.ParaDict[UserBasicInfoModel.Field.DateEnd.ToString()];
                model.TimeBegin = base.FunToolData.ParaDict[UserBasicInfoModel.Field.TimeBegin.ToString()];
                model.TimeEnd = base.FunToolData.ParaDict[UserBasicInfoModel.Field.TimeEnd.ToString()];
            }

            model.GetSysADSTabList(_BaseAPModel.EnumTabAction.SysUserBasicInfo);

            if (await model.GetUserBasicInfoList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserBasicInfo.SystemMsg_GetUserBasicInfoList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserBasicInfo(UserBasicInfoModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysADSTabList(_BaseAPModel.EnumTabAction.SysUserBasicInfo);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserBasicInfoModel.Field.QueryUserID.ToString(), model.QueryUserID);
                paraDict.Add(UserBasicInfoModel.Field.QueryUserNM.ToString(), model.QueryUserNM);
                paraDict.Add(UserBasicInfoModel.Field.IsLeft.ToString(), model.IsLeft);
                paraDict.Add(UserBasicInfoModel.Field.DateBegin.ToString(), model.DateBegin);
                paraDict.Add(UserBasicInfoModel.Field.DateEnd.ToString(), model.DateEnd);
                paraDict.Add(UserBasicInfoModel.Field.TimeBegin.ToString(), model.TimeBegin);
                paraDict.Add(UserBasicInfoModel.Field.TimeEnd.ToString(), model.TimeEnd);

                model.SetSysFunToolPara(base.UserSystemFunKey, Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo), paraDict);

                if (await model.GetUserBasicInfoList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserBasicInfo.SystemMsg_GetUserBasicInfoList);
                }
            }

            return View(model);
        }
    }
}