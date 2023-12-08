using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserSettingDetail(UserSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetUserSettingDetail() == true)
                {
                    SetSystemAlertMessage(SysUserSettingDetail.SystemMsg_AddUserSettingDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    string userPWD = string.Empty;
                    if (model.ExecAction == EnumActionType.Update && string.IsNullOrWhiteSpace(model.UserPWD))
                    {
                        if (model.GetUserSettingDetail() == false)
                        {
                            SetSystemErrorMessage(SysUserSettingDetail.SystemMsg_GetUserSettingDetail);
                            result = false;
                        }
                        else
                        {
                            userPWD = model.EntityUserSettingDetail.UserPWD.GetValue();
                        }
                    }
                    else
                    {
                        userPWD = LionTech.Utility.Validator.GetEncodeString(model.UserPWD);
                    }

                    if (!model.MailFormat)
                    {
                        SetSystemErrorMessage(SysUserSettingDetail.ValidationMailFormatResult_Failure);
                        result = false;
                    }

                    if (result)
                    {
                        if (model.GetEditUserSettingDetailResult(AuthState.SessionData.UserID, AuthState.SessionData.UserComID, AuthState.SessionData.UserUnitID, AuthState.SessionData.UserUnitNM, base.HostIPAddress()))
                        {
                            base.ExecUserRoleLogWrite(model.UserID);
                           
                            string filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.zh_TW);
                            bool generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.zh_TW);

                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.zh_CN);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.zh_CN);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.en_US);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.en_US);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.th_TH);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.th_TH);
                            }
                            if (generateUserMenuXMLResult)
                            {
                                filePath = base.GetUserMenuFilePath(model.UserID, EnumCultureID.ja_JP);
                                generateUserMenuXMLResult = model.GenerateUserMenuXML(filePath, EnumCultureID.ja_JP);
                            }

                            if (!generateUserMenuXMLResult)
                            {
                                base.SetSystemAlertMessage(SysUserSettingDetail.SystemMsg_GenerateFailure);
                                result = false;
                            }

                            string apiParaJsonString = model.GetAPIParaSCMAPB2PSettingSupB2PUserEntity(AuthState.SessionData.UserID, userPWD).SerializeToJson();
                            string apiReturnString = base.ExecAPIService(EnumAppSettingAPIKey.APISCMAPB2PSettingSupB2PUserEditEventURL, apiParaJsonString);
                            if (apiReturnString == null || apiReturnString != bool.TrueString)
                            {
                                SetSystemErrorMessage(SysUserSettingDetail.EditUserSettingDetailResult_Failure);
                                result = false;
                            }
                        }
                        else
                        {
                            SetSystemErrorMessage(SysUserSettingDetail.EditUserSettingDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("UserSetting", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSystemUserDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysUserSettingDetail.SystemMsg_GetSystemUserDetail);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetUserSettingDetail() == false)
                {
                    SetSystemErrorMessage(SysUserSettingDetail.SystemMsg_GetUserSettingDetail);
                }
                else
                {
                    model.UserNM = model.EntityUserSettingDetail.UserNM.GetValue();
                    model.UserPWD = model.EntityUserSettingDetail.UserPWD.GetValue();
                    model.UserGender = model.EntityUserSettingDetail.UserGender.GetValue();
                    model.UserTitle = model.EntityUserSettingDetail.UserTitle.GetValue();
                    model.UserTel1 = model.EntityUserSettingDetail.UserTel1.GetValue();
                    model.UserTel2 = model.EntityUserSettingDetail.UserTel2.GetValue();
                    model.UserEmail = model.EntityUserSettingDetail.UserEmail.GetValue();
                    model.Remark = model.EntityUserSettingDetail.Remark.GetValue();
                    model.IsDisable = model.EntityUserSettingDetail.IsDisable.GetValue();
                    model.IsGrantor = model.EntityUserSettingDetail.IsGrantor.GetValue();
                    model.UpdInfor = string.Format("{0} ({1})",
                        new object[] { model.EntityUserSettingDetail.UpdUserNM.GetValue(), Common.GetDateTimeText(model.EntityUserSettingDetail.UpdDT.GetValue()) }
                    );
                }
            }

            return View(model);
        }
    }
}