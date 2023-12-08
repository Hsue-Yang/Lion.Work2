using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Web.ERPHelper;
using LionTech.Utility;
using Resources;
using LionTech.Utility.ERP;
using System.Threading.Tasks;
using System.Text;
using System.Net;

namespace ERPAP.Models.Sys
{
    public class LineBotAccountSettingDetailModel : SysModel, IValidatableObject
    {
        #region - Property -
        [Required]
        public string SysID { get; set; }

        public string SysNMID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string LineID { get; set; }

        [Required]
        [StringLength(150)]
        public string LineNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        public string LineNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        public string LineNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        public string LineNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        public string LineNMJAJP { get; set; }
      
        [Required]
        [StringLength(150)]
        public string LineNMKOKR { get; set; }

        public string IsDisable { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [Required]
        [StringLength(200)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ChannelID { get; set; }

        [Required]
        [StringLength(200)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ChannelSecret { get; set; }

        [Required]
        [StringLength(200)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string ChannelAccessToken { get; set; }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            if (ExecAction == EnumActionType.Add)
            {
                #region - LineBot好友代碼重複驗證 - 
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                LineID = string.IsNullOrWhiteSpace(LineID) ? null : LineID;
                
                string apiUrl = API.SystemLineBot.CheckSystemLineBotIdIsExists(SysID, LineID);
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);
                
                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });                             

                if (responseObj.IsExists)
                {
                    yield return new ValidationResult(SysLineBotAccountSettingDetail.SystemMsg_LineBotAccountRepeat);
                }
                #endregion
            }
        }
        #endregion

        #region - 取得LineBot帳號設定明細主檔 -
        /// <summary>
        /// 取得LineBot帳號設定明細主檔
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetLineBotAccountSettingDetail(string userID, EnumCultureID cultureId)
        {
            try
            {
                var sysSystemMain = GetSysSystemMain(SysID, cultureId);

                if (sysSystemMain != null)
                {
                    SysNMID = sysSystemMain.SysNMID;
                }

                if (!string.IsNullOrWhiteSpace(LineID))
                {
                    string apiUrl = API.SystemLineBot.QuerySystemLineBotAccountDetail(userID, SysID, LineID);
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                    var lineBotAccountSettingDetail = Common.GetJsonDeserializeObject<LineBotAccountSettingDetail>(response);

                    if (lineBotAccountSettingDetail != null)
                    {
                        LineNMZHTW = lineBotAccountSettingDetail.LineNMZHTW;
                        LineNMZHCN = lineBotAccountSettingDetail.LineNMZHCN;
                        LineNMENUS = lineBotAccountSettingDetail.LineNMENUS;
                        LineNMTHTH = lineBotAccountSettingDetail.LineNMTHTH;
                        LineNMJAJP = lineBotAccountSettingDetail.LineNMJAJP;
                        LineNMKOKR = lineBotAccountSettingDetail.LineNMKOKR;
                        ChannelID = Security.Decrypt(lineBotAccountSettingDetail.ChannelID);
                        ChannelSecret = Security.Decrypt(lineBotAccountSettingDetail.ChannelSecret);
                        ChannelAccessToken = Security.Decrypt(lineBotAccountSettingDetail.ChannelAccessToken);
                        IsDisable = lineBotAccountSettingDetail.IsDisable;
                        SortOrder = lineBotAccountSettingDetail.SortOrder;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 編輯LineBot好友設定明細 -
        /// <summary>
        /// 編輯LineBot好友設定明細
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> EditLineBotAccountSettingDetail(string updUserID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    LineID = string.IsNullOrWhiteSpace(LineID) ? null : LineID,
                    LineNMZHTW = string.IsNullOrWhiteSpace(LineNMZHTW) ? null : LineNMZHTW,
                    LineNMZHCN = string.IsNullOrWhiteSpace(LineNMZHCN) ? null : LineNMZHCN,
                    LineNMENUS = string.IsNullOrWhiteSpace(LineNMENUS) ? null : LineNMENUS,
                    LineNMTHTH = string.IsNullOrWhiteSpace(LineNMTHTH) ? null : LineNMTHTH,
                    LineNMJAJP = string.IsNullOrWhiteSpace(LineNMJAJP) ? null : LineNMJAJP,
                    LineNMKOKR = string.IsNullOrWhiteSpace(LineNMKOKR) ? null : LineNMKOKR,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = updUserID,
                    ChannelID = string.IsNullOrWhiteSpace(ChannelID) ? null : Security.Encrypt(ChannelID),
                    ChannelSecret = string.IsNullOrWhiteSpace(ChannelSecret) ? null : Security.Encrypt(ChannelSecret),
                    ChannelAccessToken = string.IsNullOrWhiteSpace(ChannelAccessToken) ? null : Security.Encrypt(ChannelAccessToken)
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemLineBot.EditSystemLineBotAccountDetail(updUserID, SysID, LineID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 刪除LineBot好友設定明細 -
        /// <summary>
        /// 刪除LineBot好友設定明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteLineBotAccountSettingDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemLineBot.DeleteSystemLineBotByIds(userID, SysID, LineID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 紀錄LineBot帳號設定明細 -
        /// <summary>
        /// 紀錄LineBot帳號設定明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool RecordLogLineBotAccountSetting(EnumCultureID cultureID, string updUserID, string ipAddress)
        {
            try
            {
                Mongo_BaseAP.RecordLogSystemLinePara para = new Mongo_BaseAP.RecordLogSystemLinePara();
                para.SysID = new DBVarChar(SysID);
                para.SysNM = new DBNVarChar(SysNMID);
                para.LineID = new DBVarChar(LineID);
                para.LineNMZHTW = new DBNVarChar(LineNMZHTW);
                para.LineNMZHCN = new DBNVarChar(LineNMZHCN);
                para.LineNMENUS = new DBNVarChar(LineNMENUS);
                para.LineNMTHTH = new DBNVarChar(LineNMTHTH);
                para.LineNMJAJP = new DBNVarChar(LineNMJAJP);
                para.LineNMKOKR = new DBNVarChar(LineNMKOKR);
                para.ChannelID = new DBVarChar(Security.Encrypt(ChannelID));
                para.ChannelSecret = new DBVarChar(Security.Encrypt(ChannelSecret));
                para.ChannelAccessToken = new DBVarChar(Security.Encrypt(ChannelAccessToken));
                para.IsDisable = new DBVarChar(IsDisable);
                para.SortOrder = new DBVarChar(SortOrder);

                Mongo_BaseAP.EnumModifyType modifyType;

                switch (ExecAction)
                {
                    case EnumActionType.Add:
                        modifyType = Mongo_BaseAP.EnumModifyType.I;
                        break;
                    case EnumActionType.Update:
                        modifyType = Mongo_BaseAP.EnumModifyType.U;
                        break;
                    case EnumActionType.Delete:
                        modifyType = Mongo_BaseAP.EnumModifyType.D;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_LINE, modifyType, updUserID, ipAddress, cultureID, para);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
    }
}