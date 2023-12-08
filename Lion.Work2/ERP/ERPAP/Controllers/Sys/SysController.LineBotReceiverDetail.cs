using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.APIService.LineBot;
using LionTech.Entity.ERP;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter("LineBotAccountSetting")]
        public async Task<ActionResult> LineBotReceiverDetail(LineBotReceiverDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false || model.IsITManager == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
                return RedirectToAction("LineBotAccountSetting");
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.LineReceiverSourceType);

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Update ||
                    model.ExecAction == EnumActionType.Delete)
                {
                    if (model.ExecAction == EnumActionType.Delete)
                    {
                        try
                        {
                            ILineClient lineClient = LineClient.Create();
                            lineClient.ClientSysID = EnumSystemID.ERPAP.ToString();
                            lineClient.ClientUserID = AuthState.SessionData.UserID;
                            lineClient.SystemID = model.SysID;
                            lineClient.LineID = model.LineID;
                            lineClient.Leave(model.ReceiverID);

                            if (await model.GetLineBotReceiverDetail(AuthState.SessionData.UserID, CultureID) == false)
                            {
                                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_GetLineBotReceiverDetail_Failure);
                                result = false;
                            }

                            if (result && model.RecordLogLineBotReceiverDetail(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                            {
                                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_RecordLogLineBotReceiverDetail_Failure);
                                result = false;
                            }
                        }
                        catch (LineException ex)
                        {
                            SetSystemErrorMessage(string.Format(SysLineBotReceiverDetail.SystemMsg_LineBotServiceLeave_Failure, ex.Message));
                            result = false;
                        }
                    }
                    else if (model.ExecAction == EnumActionType.Update)
                    {
                        if (TryValidatableObject(model))
                        {
                            if (await model.EditLineBotReceiverDetail(AuthState.SessionData.UserID) == false)
                            {
                                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_EditLineBotReceiverDetail_Failure);
                                result = false;
                            }

                            if (result && await model.GetLineBotReceiverDetail(AuthState.SessionData.UserID, CultureID) == false)
                            {
                                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_GetLineBotReceiverDetail_Failure);
                            }

                            if (result && model.RecordLogLineBotReceiverDetail(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                            {
                                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_RecordLogLineBotReceiverDetail_Failure);
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return new TransferResult("/Sys/LineBotReceiver");
                }
            }

            if (await model.GetLineBotReceiverDetail(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotReceiverDetail.SystemMsg_GetLineBotReceiverDetail_Failure);
            }

            if (model.SourceType == Entity_BaseAP.EnumLineSourceType.USER.ToString())
            {
                try
                {
                    ILineClient lineClient = LineClient.Create();
                    lineClient.ClientSysID = EnumSystemID.ERPAP.ToString();
                    lineClient.ClientUserID = AuthState.SessionData.UserID;
                    lineClient.SystemID = model.SysID;
                    lineClient.LineID = model.LineID;
                    var profileResult = lineClient.GetProfile(model.ReceiverID);

                    if (profileResult != null)
                    {
                        model.DisplayNM = profileResult.DisplayName;
                        model.ImageUrl = profileResult.PictureUrl;
                    }
                }
                catch (LineException ex)
                {
                    SetSystemErrorMessage(string.Format(SysLineBotReceiverDetail.SystemMsg_LineBotServiceGetProfile_Failure, ex.Message));
                }
            }

            return View(model);
        }
    }
}