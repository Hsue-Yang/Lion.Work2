using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class TrustIPDetailModel : SysModel
    {
        public class TrustIPDetail
        {
            public string IPBegin { get; set; }
            public string IPEnd { get; set; }
            public string ComID { get; set; }
            public string ComNM { get; set; }
            public string TrustStatus { get; set; }
            public string TrustType { get; set; }
            public string TrustTypeNM { get; set; }
            public string SourceType { get; set; }
            public string SourceTypeNM { get; set; }
            public string Remark { get; set; }
            public string SortOrder { get; set; }
        }

        public class RecordSysTrustIP
        {
            public string ModifyType { get; set; }
            public string ModifyTypeNM { get; set; }
            public string IPBegin { get; set; }
            public string IPEnd { get; set; }
            public string ComID { get; set; }
            public string ComNM { get; set; }
            public string TrustStatus { get; set; }
            public string TrustType { get; set; }
            public string TrustTypeNM { get; set; }
            public string SourceType { get; set; }
            public string SourceTypeNM { get; set; }
            public string Remark { get; set; }
            public string SortOrder { get; set; }
            public string APINo { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
            public string ExecSysID { get; set; }
            public string ExecSysNM { get; set; }
            public string ExecIPAddress { get; set; }
        }

        public enum EnumModifyResult
        {
            Success,
            Failure,
            SyncASPFailure
        }

        [InputType(EnumInputType.TextBoxHidden)]
        public string IPEndOriginal { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string IPBeginOriginal { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string IPBegin { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string IPEnd { get; set; }

        [Required]
        public string ComID { get; set; }

        [Required]
        public string TrustStatus { get; set; }

        [Required]
        public string TrustType { get; set; }

        [Required]
        public string SourceType { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList;

        public TrustIPDetailModel()
        {
            TabList = new List<TabStripHelper.Tab>
            {
                new TabStripHelper.Tab
                {
                    ControllerName = string.Empty,
                    ActionName = string.Empty,
                    TabText = SysTrustIPDetail.TabText_TrustIPDetail,
                    ImageURL = string.Empty
                }
            };
        }
        
        public void FormReset()
        {
            this.ComID = string.Empty;
            this.TrustStatus = string.Empty;
            this.TrustType = string.Empty;
            this.SourceType = string.Empty;
            this.Remark = string.Empty;
            this.SortOrder = string.Empty;
        }

        public async Task<bool> GetValidTrustIPRepeated(EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var para = new
                {
                    IPBeginOriginal = string.IsNullOrWhiteSpace(IPBeginOriginal) ? null : IPBeginOriginal,
                    IPEndOriginal = string.IsNullOrWhiteSpace(IPEndOriginal) ? null : IPEndOriginal,
                    IPBegin = string.IsNullOrWhiteSpace(IPBegin) ? null : IPBegin,
                    IPEnd = string.IsNullOrWhiteSpace(IPEnd) ? null : IPEnd,
                    ComID = string.IsNullOrWhiteSpace(ComID) ? null : ComID,
                    TrustStatus = string.IsNullOrWhiteSpace(TrustStatus) ? null : TrustStatus,
                    TrustType = string.IsNullOrWhiteSpace(TrustType) ? null : TrustType,
                    SourceType = string.IsNullOrWhiteSpace(SourceType) ? null : SourceType
                };

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.TrustIP.QueryValidTrustIPRepeated();
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                if (Convert.ToBoolean(response))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        TrustIPDetail _entityTrustIPDetail;
        public TrustIPDetail EntityTrustIPDetail { get { return _entityTrustIPDetail; } }

        public async Task<TrustIPDetail> SelectTrustIPDetail(EnumCultureID cultureID)
        {
            try
            {
                string iPBegin = string.IsNullOrWhiteSpace(IPBegin) ? null : IPBegin;
                string iPEnd = string.IsNullOrWhiteSpace(IPEnd) ? null : IPEnd;

                string apiUrl = API.TrustIP.QueryTrustIPDetail(iPBegin, iPEnd, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    TrustIPDetail = (TrustIPDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                return responseObj.TrustIPDetail;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }


        public async Task<bool> GetTrustIPDetail(EnumCultureID cultureID)
        {
            try
            {
                _entityTrustIPDetail = await SelectTrustIPDetail(cultureID);

                if (_entityTrustIPDetail != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumModifyResult> GetEditTrustIPDetailResult(string userNM, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var editPara = new
                {
                    IPBeginOriginal = string.IsNullOrWhiteSpace(IPBeginOriginal) ? null : IPBeginOriginal,
                    IPEndOriginal = string.IsNullOrWhiteSpace(IPEndOriginal) ? null : IPEndOriginal,
                    IPBegin = string.IsNullOrWhiteSpace(IPBegin) ? null : IPBegin,
                    IPEnd = string.IsNullOrWhiteSpace(IPEnd) ? null : IPEnd,
                    ComID = string.IsNullOrWhiteSpace(ComID) ? null : ComID,
                    TrustStatus = string.IsNullOrWhiteSpace(TrustStatus) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    TrustType = string.IsNullOrWhiteSpace(TrustType) ? null : TrustType,
                    SourceType = string.IsNullOrWhiteSpace(SourceType) ? null : SourceType,
                    Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = string.IsNullOrWhiteSpace(updUserID) ? null : updUserID,
                    UpdUserNM = string.IsNullOrWhiteSpace(userNM) ? null : userNM
                };

                var editParaJsonStr = Common.GetJsonSerializeObject(editPara);
                var editBodyBytes = Encoding.UTF8.GetBytes(editParaJsonStr);

                string editApiUrl = API.TrustIP.EditTrustIP();
                var response = await PublicFun.HttpPutWebRequestGetResponseStringAsync(editApiUrl, AppSettings.APITimeOut, editBodyBytes);

                Mongo_BaseAP.EnumModifyType modifyType_ = Mongo_BaseAP.EnumModifyType.U;
                if (ExecAction == EnumActionType.Add)
                {
                    modifyType_ = Mongo_BaseAP.EnumModifyType.I;
                }

                if (response == EnumModifyResult.Success.ToString())
                {
                    var trustIPDetail = await SelectTrustIPDetail(cultureID);

                    this.GetRecordSysTrustIPResult(trustIPDetail, modifyType_, updUserID, ipAddress, cultureID);

                    return EnumModifyResult.Success;
                }
                else if (response == EnumModifyResult.SyncASPFailure.ToString())
                {
                    return EnumModifyResult.SyncASPFailure;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EnumModifyResult.Failure;
        }

        public async Task<EnumModifyResult> GetDeleteTrustIPDetailResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var trustIPDetail = await SelectTrustIPDetail(cultureID);

                string iPBegin = string.IsNullOrWhiteSpace(IPBegin) ? null : IPBegin;
                string iPEnd = string.IsNullOrWhiteSpace(IPEnd) ? null : IPEnd;

                string apiUrl = API.TrustIP.DeleteTrustIP(iPBegin, iPEnd);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                if (response == EnumModifyResult.Success.ToString())
                {
                    this.GetRecordSysTrustIPResult(trustIPDetail, Mongo_BaseAP.EnumModifyType.D, updUserID, ipAddress, cultureID);

                    return EnumModifyResult.Success;
                }
                else if (response == EnumModifyResult.SyncASPFailure.ToString())
                {
                    return EnumModifyResult.SyncASPFailure;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EnumModifyResult.Failure;
        }

        private bool GetRecordSysTrustIPResult(TrustIPDetail entity, Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> modifyTypeList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                RecordSysTrustIP recordPara = new RecordSysTrustIP
                {
                    ModifyType = modifyType.ToString(),
                    ModifyTypeNM = null,
                    IPBegin = entity.IPBegin,
                    IPEnd = entity.IPEnd,
                    ComID = entity.ComID,
                    ComNM = entity.ComNM,
                    TrustStatus = entity.TrustStatus,
                    TrustType = entity.TrustType,
                    TrustTypeNM = entity.TrustTypeNM,
                    SourceType = entity.SourceType,
                    SourceTypeNM = entity.SourceTypeNM,
                    Remark = entity.Remark,
                    SortOrder = entity.SortOrder,
                    APINo = null,
                    UpdUserID = entityBasicInfo.UpdUserID.GetValue().ToString(),
                    UpdUserNM = entityBasicInfo.UpdUserNM.GetValue().ToString(),
                    UpdDT = DateTime.Now,
                    ExecSysID = entityBasicInfo.ExecSysID.GetValue().ToString(),
                    ExecSysNM = entityBasicInfo.ExecSysNM.GetValue().ToString(),
                    ExecIPAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress
                };

                if (modifyTypeList != null && modifyTypeList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(modifyType.ToString()))
                {
                    recordPara.ModifyTypeNM = (modifyTypeList.Find(e => e.CodeID == modifyType.ToString())).CodeNM.GetValue().ToString();
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(recordPara);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.TrustIP.InsertTrustIPMongoDB();
                Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }
    }
}