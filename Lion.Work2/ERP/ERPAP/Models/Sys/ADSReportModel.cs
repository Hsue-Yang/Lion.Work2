using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LionTech.Utility.ERP;
using Resources;

namespace ERPAP.Models.Sys
{
    public class ADSReportModel : SysModel
    {
        public enum EnumReportType
        {
            SysRoleToFunction,
            SysUserToFunction,
            SysSingleFunctionAwarded,
            SysUserLoginLastTime,
            SysReportToPermissions,
        }

        [Required]
        public string ReportType { get; set; }

        public string SysID { get; set; }

        public ADSReportModel() { }

        public Dictionary<string, string> ComboBoxADSReportList { get { return _ComboBoxADSReportList; } }

        private readonly Dictionary<string, string> _ComboBoxADSReportList = new Dictionary<string, string>()
        {
            {EnumReportType.SysRoleToFunction.ToString(), SysADSReport.ComboBox_SyRoleToFunction},
            {EnumReportType.SysUserToFunction.ToString(), SysADSReport.ComboBox_SyUserToFunction},
            {EnumReportType.SysSingleFunctionAwarded.ToString(), SysADSReport.ComboBox_SySingleFunctionAwarded},
            {EnumReportType.SysUserLoginLastTime.ToString(), SysADSReport.ComboBox_SysLastTime},
            {EnumReportType.SysReportToPermissions.ToString(), SysADSReport.ComboBox_SysReportToPermissions},
        };

        public Stream CsvReport { get; private set; }

        public async Task<bool> GetCsvRawData(string sysID,string userID, string reportType)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = API.ADSReport.QueryADSReportsCsv(userID ?? string.Empty, reportType, sysID);
                    HttpResponseMessage response = await client.GetAsync(string.Format(apiUrl));

                    if (response != null)
                    {
                        CsvReport = await response.Content.ReadAsStreamAsync();
                    }
                }

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