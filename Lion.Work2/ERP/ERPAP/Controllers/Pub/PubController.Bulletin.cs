using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult Bulletin(string val1, string val2)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            BulletinModel model = new BulletinModel();

            string cultureID = string.Empty;

            if (!string.IsNullOrWhiteSpace(val1))
            {
                AuthState.SessionData.UserMenuID = (EnumUserMenuID)int.Parse(val1);
                if (CultureID != EnumCultureID.zh_TW)
                {
                    cultureID = Common.GetEnumDesc(CultureID);
                }
                string filePathUserMenu = Path.Combine(
                    new string[]
                    {
                        base.GetFilePathFolderPath(EnumFilePathFolder.UserMenu), AuthState.SessionData.UserID,
                        string.Concat(new object[]
                            {
                                EnumUserMenu.UserMenu.ToString(),
                                ".", AuthState.SessionData.UserID,
                                (string.IsNullOrWhiteSpace(cultureID) ? string.Empty : "."), cultureID
                                ,
                                ".", EnumFileExtension.xml.ToString()
                            }
                        )
                    }
                );

                AuthState.SessionData.FilePathUserMenu = filePathUserMenu;
                base.SetUserMenu((EnumUserMenuID)int.Parse(val1), CultureID);
            }

            if (!string.IsNullOrWhiteSpace(val2))
            {
                EnumCultureID finalCultureID = EnumCultureID.zh_TW;
                if (!string.IsNullOrWhiteSpace(val2) && Enum.IsDefined(typeof(EnumCultureID), val2))
                {
                    finalCultureID = (EnumCultureID)Enum.Parse(typeof(EnumCultureID), val2);
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));

                AuthState.CookieData.SetCultureID(finalCultureID.ToString());

                if (finalCultureID != EnumCultureID.zh_TW)
                {
                    cultureID = Common.GetEnumDesc(finalCultureID);
                }

                string filePathUserMenu = Path.Combine(
                    new string[]
                        {
                            base.GetFilePathFolderPath(EnumFilePathFolder.UserMenu), AuthState.SessionData.UserID,
                            string.Concat(new object[]
                                {
                                    EnumUserMenu.UserMenu.ToString(),
                                    ".", AuthState.SessionData.UserID,
                                    (string.IsNullOrWhiteSpace(cultureID) ? string.Empty : "."), cultureID
                                    ,
                                    ".", EnumFileExtension.xml.ToString()
                                }
                            )
                        }
                );

                AuthState.SessionData.FilePathUserMenu = filePathUserMenu;
                base.SetUserMenu(AuthState.SessionData.UserMenuID, finalCultureID);
            }
            
            //判定是否為當日首次登入
            if (model.ValidDailyFirstLogin(AuthState.SessionData.UserID))
            {
                model.IsFirstLogin = EnumYN.Y.ToString();
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult Bulletin(BulletinModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            return RedirectToAction("SSOLogin", "Home", new { systemID = model.SystemID , targetPath = HttpUtility.UrlDecode(model.TargetPath) });
        }
    }
}