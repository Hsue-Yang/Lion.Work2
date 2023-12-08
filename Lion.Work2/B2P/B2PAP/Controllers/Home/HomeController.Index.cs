using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using B2PAP.Models.Home;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Home;
using LionTech.Utility;
using LionTech.Utility.B2P;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult Index()
        {
            //SessionIDManager sessionIDManager = new SessionIDManager();
            //string newSessionID = sessionIDManager.CreateSessionID(HttpContext.ApplicationInstance.Context);
            //bool redirected = false;
            //bool cookieAdded = false;
            //sessionIDManager.SaveSessionID(HttpContext.ApplicationInstance.Context, newSessionID, out redirected, out cookieAdded);

            EnumCultureID finalCultureID = EnumCultureID.zh_TW;
            if (this.AuthState.CookieData.CultureID != null)
            {
                finalCultureID = Utility.GetCultureID(this.AuthState.CookieData.CultureID);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));

            this.AuthState.CookieData.SetCultureID(finalCultureID.ToString());

            IndexModel model = new IndexModel();

            if (TempData["B2PLoginModelUserID"] != null)
            {
                model.UserID = TempData["B2PLoginModelUserID"].ToString();
            }
            if (TempData["B2PLoginModelTargetUrl"] != null)
            {
                model.TargetUrl = TempData["B2PLoginModelTargetUrl"].ToString();
            }

            if (Request.Url.Host == "localhost" || !Request.IsSecureConnection)
                model.EnvironmentRemind = HomeIndex.Text_EnvironmentRemind;

            model.CultureID = finalCultureID.ToString();
            if (model.GetBaseCultureIDList(finalCultureID) == false)
            {
                SetSystemErrorMessage(HomeIndex.SystemMsg_GetBaseCultureIDList);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            EnumCultureID finalCultureID = EnumCultureID.zh_TW;
            if (!string.IsNullOrWhiteSpace(model.CultureID) && Enum.IsDefined(typeof(EnumCultureID), model.CultureID))
            {
                finalCultureID = (EnumCultureID)Enum.Parse(typeof(EnumCultureID), model.CultureID);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));

            this.AuthState.CookieData.SetCultureID(finalCultureID.ToString());

            if (Request.Url.Host == "localhost" || !Request.IsSecureConnection)
                model.EnvironmentRemind = HomeIndex.Text_EnvironmentRemind;

            if (model.GetBaseCultureIDList(finalCultureID) == false)
            {
                SetSystemErrorMessage(HomeIndex.SystemMsg_GetBaseCultureIDList);
            }

            bool isValid = false;

            #region - Valid User Account -
            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.AuthenticatingB2P())
                {
                    isValid = true;
                }
                else
                {
                    SetSystemErrorMessage(HomeIndex.SystemMsg_B2PAuthenticatingError);
                }
            }
            #endregion

            if (isValid && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetValidateUserAuthorization(AuthState.SessionData.SessionID, base.ClientIPAddress()) == true &&
                    model.GetUserMain() == true && model.GetUserSystemList() == true && model.GetUserSystemRoleList() == true)
                {
                    StringBuilder userSystemIDStringBuilder = new StringBuilder();
                    foreach (EntityIndex.UserSystem userSystem in model.EntityUserSystemList)
                    {
                        userSystemIDStringBuilder.Append(string.Concat(new object[] { userSystem.SystemID.GetValue(), "," }));
                    }
                    string userSystemID = userSystemIDStringBuilder.ToString();
                    userSystemID = userSystemID.Substring(0, userSystemID.Length - 1);

                    StringBuilder userRoleIDStringBuilder = new StringBuilder();
                    foreach (EntityIndex.UserSystemRole userSystemRole in model.EntityUserSystemRoleList)
                    {
                        userRoleIDStringBuilder.Append(string.Concat(new object[] { userSystemRole.SystemID.GetValue(), userSystemRole.RoleID.GetValue(), "," }));
                    }
                    string userRoleID = userRoleIDStringBuilder.ToString();
                    userRoleID = userRoleID.Substring(0, userRoleID.Length - 1);

                    AuthState.SessionData.SetUser(
                        model.UserID, model.EntityUserMain.UserNM.GetValue(),
                        model.EntityUserMain.UserComID.GetValue(),
                        model.EntityUserMain.UserUnitID.GetValue(), model.EntityUserMain.UserUnitNM.GetValue(),
                        userSystemID, userRoleID);

                    AuthState.SessionData.UserMenuXSLString = Common.FileReadStream(base.GetAppDataFilePath(EnumAppDataFile.UserMenuContentXSL));

                    string cultureID = string.Empty;
                    if (base.CultureID != LionTech.Entity.B2P.EnumCultureID.zh_TW)
                    {
                        cultureID = Common.GetEnumDesc(base.CultureID);
                    }

                    string filePathUserMenu = Path.Combine(
                        new string[]
                        {
                            base.GetFilePathFolderPath(EnumFilePathFolder.UserMenu), model.UserID,
                            string.Concat(new object[]
                                                {
                                                    EnumUserMenu.UserMenu.ToString(),
                                                    ".", model.UserID,
                                                    (string.IsNullOrWhiteSpace(cultureID) ? string.Empty : "."), cultureID
                                                    ,
                                                    ".", EnumFileExtension.xml.ToString()
                                                })
                        });

                    AuthState.SessionData.FilePathUserMenu = filePathUserMenu;

                    base.SetUserMenu(EnumUserMenuID.MenuID1);

                    try
                    {
                        string apiReturnString = bool.FalseString;
                        if (base.ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain))
                        {
                            string apiParaJsonString = model.GetAPIParaSCMAPB2PSettingSupB2PUserBulletinEntity().SerializeToJson();
                            apiReturnString = base.ExecAPIService(EnumAppSettingAPIKey.APISCMAPB2PSettingSupB2PUserBulletinCheckEventURL, apiParaJsonString);
                        }

                        if (apiReturnString == bool.TrueString)
                        {
                            return Redirect(string.Concat(new string[] { Common.GetEnumDesc(EnumSystemID.SCMB2P), "/B2PBBS01/ListView" }));
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(model.EntityUserMain.DefaultPath.GetValue()))
                            {
                                string headURL = Common.GetEnumDesc((EnumSystemID)Enum.Parse(typeof(EnumSystemID), model.EntityUserMain.DefaultSysID.GetValue()));

                                return Redirect(string.Concat(new string[] { headURL, model.EntityUserMain.DefaultPath.GetValue() }));
                            }
                            else
                            {
                                if (model.TargetUrl != null)
                                {
                                    return Redirect(model.TargetUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Bulletin", "Pub");
                                }
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Bulletin", "Pub");
                    }
                }
                else
                {
                    SetSystemErrorMessage(HomeIndex.SystemMsg_UserInformationError);
                }
            }

            if (!isValid)
            {
                model.FormReset();
            }

            return View(model);
        }
    }
}