using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using B2PAP.Models.Pub;
using LionTech.Entity.B2P;
using LionTech.Utility;
using LionTech.Utility.B2P;

namespace B2PAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult Bulletin(string val1, string val2)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            BulletinModel model = new BulletinModel();

            if (!string.IsNullOrWhiteSpace(val1))
                base.SetUserMenu((EnumUserMenuID)int.Parse(val1));

            if (!string.IsNullOrWhiteSpace(val2))
            {
                EnumCultureID finalCultureID = EnumCultureID.zh_TW;
                if (!string.IsNullOrWhiteSpace(val2) && Enum.IsDefined(typeof(EnumCultureID), val2))
                {
                    finalCultureID = (EnumCultureID)Enum.Parse(typeof(EnumCultureID), val2);
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));

                this.AuthState.CookieData.SetCultureID(finalCultureID.ToString());

                string cultureID = string.Empty;
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
                base.SetUserMenu(EnumUserMenuID.MenuID1);
            }

            return View();
        }
    }
}