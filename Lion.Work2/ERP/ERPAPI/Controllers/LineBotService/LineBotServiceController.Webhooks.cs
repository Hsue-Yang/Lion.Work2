using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ERPAPI.Models.LineBotService;
using LionTech.APIService.LineBot;
using LionTech.Entity.ERP.LineBotService;
using LionTech.Log;
using LionTech.Utility;

namespace ERPAPI.Controllers.LineBotService
{
    public partial class LineBotServiceController
    {
        [HttpPost]
        public HttpResponseMessage LineWebhooks()
        {
            try
            {
                LineWebhooks lineWebhooks = new LineWebhooks(MSHttpContext.Request);
                WebhooksModel model = new WebhooksModel();
                EntityWebhooks.SystemLine systemLine = null;
                if (model.GetSystemLineList())
                {
                    systemLine =
                        model.SystemLineList.Find(line => lineWebhooks.ValidateSignature(Security.Decrypt(line.ChannelSecret.GetValue())));

                    if (systemLine != null)
                    {
                        var lineWebhookEventList = lineWebhooks.GetLineWebhookEventList();

                        var source = (from s in lineWebhookEventList
                                      select s.Source).FirstOrDefault();
                        
                        if (source != null)
                        {
                            bool isLeave = lineWebhookEventList.Any(s => s.MessageType == "leave");
                            string receiver = null;
                            string sourceType = null;
                            if (source.GetType() == typeof(LineWebhookSourceUserEvent))
                            {
                                sourceType = EnumLineWebhookSourceType.user.ToString().ToUpper();
                                receiver = ((LineWebhookSourceUserEvent)source).UserID;
                            }
                            if (source.GetType() == typeof(LineWebhookSourceGroupEvent))
                            {
                                sourceType = EnumLineWebhookSourceType.group.ToString().ToUpper();
                                receiver = ((LineWebhookSourceGroupEvent)source).GroupID;
                            }
                            if (source.GetType() == typeof(LineWebhookSourceRoomEvent))
                            {
                                sourceType = EnumLineWebhookSourceType.room.ToString().ToUpper();
                                receiver = ((LineWebhookSourceRoomEvent)source).RoomID;
                            }

                            if (receiver != "Udeadbeefdeadbeefdeadbeefdeadbeef") // 排除 line 官方使用者帳號
                            {
                                model.EditSystemLineReceiver(systemLine, receiver, sourceType, isLeave, Common.GetEnumDesc(EnumLogWriter.ERPLineBotService));
                            }
                        }
                    }
                }
                model.GetRecordLineWebhooks(systemLine, lineWebhooks.RequestParametersString, ClientIPAddress());
            }
            catch (Exception ex)
            {
                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex.Message);
            }

            return new HttpResponseMessage();
        }
    }
}
