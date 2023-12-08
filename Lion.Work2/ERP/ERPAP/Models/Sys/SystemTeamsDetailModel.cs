using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemTeamsDetailModel : SysModel
    {
        #region - Property -

        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string TeamsChannelID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelNMKOKR { get; set; }

        [Required]
        [StringLength(250)]
        [InputType(EnumInputType.TextBox)]
        public string TeamsChannelUrl { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBox)]
        public string SortOrder { get; set; }

        [StringLength(1)]
        public string IsDisable { get; set; }

        #endregion

        #region - Reset -

        public void FormReset()
        {
            TeamsChannelNMZHTW = string.Empty;
            TeamsChannelNMZHCN = string.Empty;
            TeamsChannelNMENUS = string.Empty;
            TeamsChannelNMTHTH = string.Empty;
            TeamsChannelNMJAJP = string.Empty;
            TeamsChannelNMKOKR = string.Empty;
            TeamsChannelUrl = string.Empty;
            SortOrder = string.Empty;
        }

        #endregion

        public async Task<bool> GetSystemTeamsDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemTeams.QuerySystemTeams(SysID, userID, TeamsChannelID);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysID = (string) null,
                    TeamsChannelID = (string) null,
                    TeamsChannelNMZHTW = (string) null,
                    TeamsChannelNMZHCN = (string) null,
                    TeamsChannelNMENUS = (string) null,
                    TeamsChannelNMTHTH = (string) null,
                    TeamsChannelNMJAJP = (string) null,
                    TeamsChannelNMKOKR = (string) null,
                    TeamsChannelUrl = (string)null,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder = (string) null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysID = responseObj.SysID;
                    TeamsChannelID = responseObj.TeamsChannelID;
                    TeamsChannelNMZHTW = responseObj.TeamsChannelNMZHTW;
                    TeamsChannelNMZHCN = responseObj.TeamsChannelNMZHCN;
                    TeamsChannelNMENUS = responseObj.TeamsChannelNMENUS;
                    TeamsChannelNMTHTH = responseObj.TeamsChannelNMTHTH;
                    TeamsChannelNMJAJP = responseObj.TeamsChannelNMJAJP;
                    TeamsChannelNMKOKR = responseObj.TeamsChannelNMKOKR;
                    TeamsChannelUrl = responseObj.TeamsChannelUrl;
                    IsDisable = responseObj.IsDisable;
                    SortOrder = responseObj.SortOrder;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public async Task<bool> EditSystemTeams(EnumActionType actionType, string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID,
                    TeamsChannelID,
                    TeamsChannelNMZHTW,
                    TeamsChannelNMZHCN,
                    TeamsChannelNMENUS,
                    TeamsChannelNMTHTH,
                    TeamsChannelNMJAJP,
                    TeamsChannelNMKOKR,
                    TeamsChannelUrl,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemTeams.EditSystemTeams(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteSystemTeams(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemTeams.DeleteSystemTeams(SysID, userID, TeamsChannelID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

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