using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class LineBotReceiverDetailModel : SysModel, IValidatableObject
    {
        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        public string SysNM { get; set; }

        public string SysNMID { get; set; }

        public string DisplayNM { get; set; }

        public string SourceTypeNM { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string LineID { get; set; }

        public string LineNMID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string ReceiverID { get; set; }

        [Required]
        [StringLength(20)]
        public string LineReceiverID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string LineReceiverNMKOKR { get; set; }

        [StringLength(1)]
        public string IsDisable { get; set; }

        [StringLength(20)]
        public string SourceType { get; set; }

        private Dictionary<string, string> _sourceTypeDic;

        public Dictionary<string, string> SourceTypeDic
        {
            get
            {
                if (_sourceTypeDic == null &&
                    CMCodeDictionary != null)
                {
                    _sourceTypeDic = (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.LineReceiverSourceType].Cast<Entity_BaseAP.CMCode>()
                                      select new
                                      {
                                          SourceType = s.CodeID.GetValue(),
                                          SourceTypeNM = s.CodeNM.GetValue()
                                      }).ToDictionary(k => k.SourceType, v => v.SourceTypeNM);
                }

                return _sourceTypeDic;
            }
        }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            #region - LineBot好友代碼重複驗證 -
            SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
            LineID = string.IsNullOrWhiteSpace(LineID) ? null : LineID;
            LineReceiverID = string.IsNullOrWhiteSpace(LineReceiverID) ? null : LineReceiverID;
            ReceiverID = string.IsNullOrWhiteSpace(ReceiverID) ? null : ReceiverID;

            string apiUrl = API.SystemLineBotReceiver.CheckSystemLineBotReceiverIdIsExists(SysID, LineID, LineReceiverID, ReceiverID);
            string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);
            
            var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });

            if (responseObj.IsExists)
            {
                yield return new ValidationResult(SysLineBotReceiverDetail.SystemMsg_LineBotLineRecieverIDRepeat);
            }
            #endregion
        }
        #endregion

        #region - 取得LineBot好友設定明細主檔 -
        /// <summary>
        /// 取得LineBot好友設定明細主檔
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetLineBotReceiverDetail(string userID, EnumCultureID cultureID)
        {
            try
            {
                var sysSystemMain = GetSysSystemMain(SysID, cultureID);
                if (sysSystemMain != null)
                {
                    SysNM = sysSystemMain.SysNM;
                    SysNMID = sysSystemMain.SysNMID;

                    string apiUrl = API.SystemLineBotReceiver.QuerySystemLineBotReceiverDetail(userID, SysID, ReceiverID, LineID, cultureID.ToString().ToUpper());
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                    var lineBotReceiverDetail = Common.GetJsonDeserializeObject<SystemLineBotReceiver>(response);

                    if (lineBotReceiverDetail != null)
                    {
                        LineNMID = lineBotReceiverDetail.LineNMID;
                        LineReceiverID = lineBotReceiverDetail.LineReceiverID;
                        LineReceiverNMZHTW = lineBotReceiverDetail.LineReceiverNMZHTW;
                        LineReceiverNMZHCN = lineBotReceiverDetail.LineReceiverNMZHCN;
                        LineReceiverNMENUS = lineBotReceiverDetail.LineReceiverNMENUS;
                        LineReceiverNMTHTH = lineBotReceiverDetail.LineReceiverNMTHTH;
                        LineReceiverNMJAJP = lineBotReceiverDetail.LineReceiverNMJAJP;
                        LineReceiverNMKOKR = lineBotReceiverDetail.LineReceiverNMKOKR;
                        IsDisable = lineBotReceiverDetail.IsDisable;
                        SourceType = lineBotReceiverDetail.SourceType;
                    }
                    return true;
                }
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
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> EditLineBotReceiverDetail(string updUserID)
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
                    ReceiverID = string.IsNullOrWhiteSpace(ReceiverID) ? null : ReceiverID,
                    LineReceiverID = string.IsNullOrWhiteSpace(LineReceiverID) ? null : LineReceiverID,
                    LineReceiverNMZHTW = string.IsNullOrWhiteSpace(LineReceiverNMZHTW) ? null : LineReceiverNMZHTW,
                    LineReceiverNMZHCN = string.IsNullOrWhiteSpace(LineReceiverNMZHCN) ? null : LineReceiverNMZHCN,
                    LineReceiverNMENUS = string.IsNullOrWhiteSpace(LineReceiverNMENUS) ? null : LineReceiverNMENUS,
                    LineReceiverNMTHTH = string.IsNullOrWhiteSpace(LineReceiverNMTHTH) ? null : LineReceiverNMTHTH,
                    LineReceiverNMJAJP = string.IsNullOrWhiteSpace(LineReceiverNMJAJP) ? null : LineReceiverNMJAJP,
                    LineReceiverNMKOKR = string.IsNullOrWhiteSpace(LineReceiverNMKOKR) ? null : LineReceiverNMKOKR,
                    UpdUserID = updUserID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemLineBotReceiver.EditSystemineBotReceiverDetail(updUserID, SysID, LineID, ReceiverID);
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

        #region - 紀錄LineBot好友設定明細 -
        /// <summary>
        /// 紀錄LineBot好友設定明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool RecordLogLineBotReceiverDetail(EnumCultureID cultureID, string updUserID, string ipAddress)
        {
            try
            {
                Mongo_BaseAP.RecordLogSystemLineReceiverPara para = new Mongo_BaseAP.RecordLogSystemLineReceiverPara();
                para.SysID = new DBVarChar(SysID);
                para.SysNM = new DBNVarChar(SysNM);
                para.LineID = new DBVarChar(LineID);
                para.LineNM = new DBNVarChar(LineNMID);
                para.LineReceiverID = new DBVarChar(LineReceiverID);
                para.LineReceiverNMZHTW = new DBNVarChar(LineReceiverNMZHTW);
                para.LineReceiverNMZHCN = new DBNVarChar(LineReceiverNMZHCN);
                para.LineReceiverNMENUS = new DBNVarChar(LineReceiverNMENUS);
                para.LineReceiverNMTHTH = new DBNVarChar(LineReceiverNMTHTH);
                para.LineReceiverNMJAJP = new DBNVarChar(LineReceiverNMJAJP);
                para.LineReceiverNMKOKR = new DBNVarChar(LineReceiverNMKOKR);
                para.IsDisable = new DBChar(IsDisable);
                para.ReceiverID = new DBVarChar(ReceiverID);
                para.SourceType = new DBVarChar(SourceType);

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_LINE_RECEIVER, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, para);
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