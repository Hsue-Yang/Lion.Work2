using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemAPIController
    {
        [HttpGet]
        [Route("Logs/{sysID}/{apiGroupID}")]
        public async Task<IActionResult> QuerySystemAPILogs([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromQuery] string apiFunID, [FromQuery] string apiClientSysID, [FromQuery] string apiNo, [FromQuery] string dtBegin, [FromQuery] string dtEnd, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemAPILogList(sysID, apiGroupID, apiFunID, apiClientSysID, apiNo, dtBegin, dtEnd, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemAPILogs = result.SystemAPILogList
            });
        }

        [HttpGet]
        [Route("{sysID}/{apiGroupID}/{apiFunID}/Logs")]
        public async Task<IActionResult> QuerySystemAPIClients([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromRoute] string apiFunID, [FromQuery] string dtBegin, [FromQuery] string dtEnd, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemAPIClientList(sysID, apiGroupID, apiFunID, dtBegin, dtEnd, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemAPIClients = result.SystemAPIClientList
            });
        }

        [HttpGet]
        [Route("{apiNo}/Log")]
        public async Task<IActionResult> QuerySystemAPIClient([FromRoute] string apiNo, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemAPIClientDetail(apiNo, cultureID);
            return Ok(result);
        }
    }
}