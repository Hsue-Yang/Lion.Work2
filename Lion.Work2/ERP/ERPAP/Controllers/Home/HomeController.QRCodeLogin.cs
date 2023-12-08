using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Home;
using LionTech.Entity;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.QRCoder;
using LionTech.Web.ERPHelper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Resources;

namespace ERPAP.Controllers
{
    public interface IQRCodeClient
    {
        void Login(string pincode);
        void Generator(string pincode);
    }

    internal class QRCodeInfo
    {
        public string UserID { get; set; }
        public string ConnectionID { get; set; }
        public string UserPassword { get; set; }
        public string PingCode { get; set; }
        public bool IsAuthorization { get; set; }
        public DateTime ConnectDT { get; set; }
    }

    [HubName("qrCode")]
    public class QRCodeClientHub : Hub<IQRCodeClient>
    {
        private const string ASP_NET_SESSIONID = "ASP.NET_SessionId";
        private static readonly Dictionary<string,List<string>>  ConnectionPingCode = new Dictionary<string, List<string>>();
        internal static Dictionary<string, QRCodeInfo> ConnetionPingCodePool = new Dictionary<string, QRCodeInfo>();

        public override Task OnConnected()
        {
            if (Context.Request.Cookies.Count > 0 &&
                Context.Request.Cookies.ContainsKey(ASP_NET_SESSIONID))
            {
                string sessionId = Context.Request.Cookies[ASP_NET_SESSIONID].Value;
                Groups.Add(Context.ConnectionId, sessionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _RemovePingCode(Context.ConnectionId);
            Debug.WriteLine("OnDisconnected");
            return base.OnDisconnected(stopCalled);
        }

        private void _RemovePingCode(string connectionID)
        {
            lock (ConnetionPingCodePool)
            {
                if (ConnectionPingCode.ContainsKey(connectionID))
                {
                    foreach (string pingCode in ConnectionPingCode[connectionID])
                    {
                        ConnetionPingCodePool.Remove(pingCode);
                    }

                    ConnectionPingCode.Remove(connectionID);
                }
            }
            Debug.WriteLine($"_RemovePingCode connectionID {connectionID}");
        }

        private void _AddPingCode(string connectionID, string pingCode)
        {
            if (ConnectionPingCode.ContainsKey(connectionID))
            {
                ConnectionPingCode[connectionID].Add(pingCode);
            }
            else
            {
                ConnectionPingCode.Add(connectionID, new List<string> { pingCode });
            }

            Debug.WriteLine($"_AddPingCode connectionID {connectionID}, pingCode {pingCode}");
        }

        [HubMethodName("authorizeVaildator")]
        public void AuthorizeVaildator()
        {
            Clients.Client(Context.ConnectionId);
        }

        [HubMethodName("generator")]
        public void Generator()
        {
            string pingCode = Guid.NewGuid().ToString();

            lock (ConnetionPingCodePool)
            {
                ConnetionPingCodePool.Add(pingCode, new QRCodeInfo
                {
                    ConnectionID = Context.ConnectionId,
                    ConnectDT = DateTime.Now.AddMinutes(1)
                });

                _AddPingCode(Context.ConnectionId, pingCode);
            }

            Debug.WriteLine($"Generator pingCode {pingCode}");
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(pingCode, QRCodeGenerator.ECCLevel.H))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap image = qrCode.GetGraphic(20, Color.Black, Color.White, null);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Jpeg);
                            byte[] imageBytes = ms.ToArray();
                            
                            string base64String = Convert.ToBase64String(imageBytes);
                            Clients.Client(Context.ConnectionId).Generator(base64String);
                        }
                    }
                }
            }
        }
    }

    public partial class HomeController
    {
        [HttpPost]
        public ActionResult QRCodeAuthorization(QRCodeLoginModel model)
        {
            if (model.ValidateIP(ClientIPAddress()))
            {
                QRCodeInfo qrCodeInfo = null;
                var definitionUserInfo = new { UserID = (string)null, Password = (string)null, PingCode = (string)null };
                string jsonPara = Common.GetStreamToString(Request.InputStream, Encoding.UTF8);
                var userInfo = Common.GetJsonDeserializeAnonymousType(jsonPara, definitionUserInfo);

                lock (QRCodeClientHub.ConnetionPingCodePool)
                {
                    if (QRCodeClientHub.ConnetionPingCodePool.ContainsKey(userInfo.PingCode))
                    {
                        qrCodeInfo = QRCodeClientHub.ConnetionPingCodePool[userInfo.PingCode];
                        qrCodeInfo.UserID = userInfo.UserID;
                        qrCodeInfo.UserPassword = userInfo.Password;
                        qrCodeInfo.IsAuthorization = true;
                    }
                }

                if (qrCodeInfo != null &&
                    qrCodeInfo.IsAuthorization)
                {
                    IHubContext<IQRCodeClient> context = GlobalHost.ConnectionManager.GetHubContext<IQRCodeClient>("qrCode");
                    context.Clients.Client(qrCodeInfo.ConnectionID).Login(userInfo.PingCode);
                    return Content(bool.TrueString);
                }
            }
            return Content(bool.FalseString);
        }

        [HttpPost]
        public ActionResult QRCodeLogin(QRCodeLoginModel model)
        {
            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(IndexModel.EnumCookieKey.LoginType.ToString(), model.LoginType);
            AuthState.CookieData.SetCookieKeys(LOGIN_SYSTEMFUN_KEY, paraDict);
            #endregion

            model.UserID = null;
            model.UserPassword = null;

            bool isValid = TryValidatableObject(model);

            if (isValid)
            {
                isValid = false;

                try
                {
                    lock (QRCodeClientHub.ConnetionPingCodePool)
                    {
                        QRCodeInfo qrCodeInfo = QRCodeClientHub.ConnetionPingCodePool[model.PingCode];

                        if (qrCodeInfo.IsAuthorization &&
                            qrCodeInfo.ConnectDT >= DateTime.Now)
                        {
                            model.UserID = qrCodeInfo.UserID;
                            model.UserPassword = Security.Decrypt(qrCodeInfo.UserPassword);
                        }
                    }

                    isValid = _GetVerificationResult(model);
                    FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"_GetVerificationResult isValid {isValid}");
                }
                catch (Exception ex)
                {
                    FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
                }

                if (isValid && model.ExecAction == EnumActionType.Update)
                {
                    FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"_GetLoginResult");
                    if (_GetLoginResult(model))
                    {
                        FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"_GetLoginResult true");
                        _GetTokenInfo(model);

                        _GetUserMenuContentXSL(model);

                        model.ValidFirstLogin(model.UserID, AuthState.SessionData.UserID);

                        if (model.GetUserLoginEvent(model.UserID) == false)
                        {
                            SetSystemErrorMessage(HomeIndex.SystemMsg_GetUserLoginEvent_Failure);
                        }
                        else if (model.UserLoginEvent != null)
                        {
                            if (model.UserLoginEvent.IsOutSourcing.GetValue() == EnumYN.Y.ToString())
                            {
                                return RedirectToAction("SSOLogin", "Home", new { systemID = model.UserLoginEvent.SysID.GetValue(), targetPath = model.UserLoginEvent.TargetPath.GetValue() });
                            }
                            return Redirect(model.UserLoginEvent.TargetPath.GetValue());
                        }

                        return RedirectToAction("Bulletin", "Pub");
                    }
                    FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), $"_GetLoginResult false");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}