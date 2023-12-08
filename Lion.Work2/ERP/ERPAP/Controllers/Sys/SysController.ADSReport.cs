using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Utility.ERP;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> ADSReport() 
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            ADSReportModel model = new ADSReportModel();

            model.GetSysADSTabList(_BaseAPModel.EnumTabAction.SysADSReport);

            if (await model.GetSystemSysIDList(true, base.CultureID) == false) 
            {
                SetSystemErrorMessage(SysADSReport.SystemMsg_GetSysSystemSysIDList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("ADSReport")]
        public async Task<ActionResult> ADSReport(string sysID, string reportType, string reportTypeName)
        {
            ADSReportModel model = new ADSReportModel();

            if (await model.GetCsvRawData(sysID, AuthState.SessionData.UserID, reportType))
            {
                TempData["ReportCSV"] = model.CsvReport;
                TempData["ReportTypeName"] = reportTypeName;
                return Json(new { result = true });
            }

            return Json(new { 
                result = false,
                msg = SysADSReport.SystemMsg_GetADSReportList_Failure
            });
        }

        [HttpGet]
        [AuthorizationActionFilter("ADSReport")]
        public ActionResult DownloadCsvFile()
        {
            var memoryStream = TempData["ReportCSV"] as MemoryStream;
            var reportTypeName = TempData["ReportTypeName"] as string;
            return File(memoryStream, "text/csv", string.Format(reportTypeName + $".csv"));
        }
    }
}