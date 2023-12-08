using System.Text;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/ADSReport")]
    public partial class ADSReportController : ControllerBase
    {
        private readonly IADSReportService _service;

        public ADSReportController(IADSReportService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("csv/{reportType}")]
        public async Task<IActionResult> QueryADSReportsCsv(string reportType, string sysID)
        {
            var reportContent = await _service.GetADSReportsCsv(reportType, sysID);
            
            if (!string.IsNullOrWhiteSpace(reportContent))
            {
                return File(Encoding.GetEncoding("Big5").GetBytes(reportContent),"text/csv");
            }

            return NotFound();
        }
    }
}