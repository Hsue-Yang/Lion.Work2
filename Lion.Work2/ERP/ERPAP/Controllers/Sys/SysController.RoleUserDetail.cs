// 新增日期：2017-04-26
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Linq;
using System.Text;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using System.Web.WebPages;
using Resources;
using System.Threading.Tasks;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter("RoleUser")]
        public async Task<ActionResult> RoleUserDetail(RoleUserDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Add)
                {
                    if (TryValidatableObject(model))
                    {
                        var apiResult =
                            ExecAPIService(
                                EnumAppSettingAPIKey.APIERPAPAuthorizationERPRoleUserEditEventURL,
                                null,
                                Encoding.UTF8.GetBytes(model.GetAuthRoleUserParaJsonString()));

                        if (string.IsNullOrWhiteSpace(apiResult) ||
                            apiResult.AsBool() == false)
                        {
                            SetSystemErrorMessage(SysRoleUserDetail.SystemMsg_RoleUserEdit_Failure);
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("RoleUser");
                }
            }

            await model.FormReset(AuthState.SessionData.UserID, CultureID);

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("RoleUser")]
        public ActionResult GetRoleUserAutoUserInfo(string condition)
        {
            RoleUserDetailModel model = new RoleUserDetailModel();

            var autoUserInfoList = model.GetAutoUserInfoList(condition);

            if (autoUserInfoList != null &&
                autoUserInfoList.Any())
            {
                return Json((from s in autoUserInfoList
                             select new
                             {
                                 UserID = s.UserID.GetValue(),
                                 UserNM = s.UserNM.GetValue()
                             }));
            }

            return Json(null);
        }
    }
}