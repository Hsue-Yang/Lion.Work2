using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {
        [HttpGet]
        [Route("FlowLog/{sysID}")]
        public async Task<IActionResult> QuerySystemEDILogs([FromRoute] string sysID, [FromQuery] string ediFlowID, [FromQuery] string ediNO, [FromQuery] string ediDate, [FromQuery] string dataDate, [FromQuery] string resultID, [FromQuery] string statusID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (RowCount, systemEDIFlowLogList) = await _service.GetSystemEDIFlowLogs(sysID, ediNO, ediFlowID, ediDate, dataDate, resultID, statusID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount,
                systemEDIFlowLogList
            });
        }

        [HttpPost]
        [Route("FlowLog")]
        public async Task<IActionResult> EditEDIFlowLogs([FromBody] SystemEDIFlowLogUpdateWaitStatus systemEDIFlowLogUpdateWaitStatus)
        {
            await _service.EditEDIFlowLogs(systemEDIFlowLogUpdateWaitStatus);
            return Ok();
        }
    }
}
