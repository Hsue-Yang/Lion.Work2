using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {
        [HttpGet]
        [Route("JobLog/{sysID}/{ediFlowID}")]
        public async Task<IActionResult> QuerySystemEDIJobLogs([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromQuery] string ediNO, [FromQuery] string ediJobID, [FromQuery] string ediFlowIDSearch, [FromQuery] string ediJobIDSearch,[FromQuery] string edidate, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (RowCount, SystemEDIJobLogList) = await _service.GetSystemEDIJobLogs(sysID, ediNO, ediFlowID,  ediJobID, ediFlowIDSearch, ediJobIDSearch, edidate, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount ,
                SystemEDIJobLogList
            });
        }
    }
}
