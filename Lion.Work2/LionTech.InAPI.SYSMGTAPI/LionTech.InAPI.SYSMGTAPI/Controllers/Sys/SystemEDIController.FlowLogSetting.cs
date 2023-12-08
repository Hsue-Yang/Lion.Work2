using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {
        [HttpPost]
        [Route("FlowLogSetting")]
        public async Task<IActionResult> EditEDIFlowLogSetting([FromBody] SystemEDILogSetting systemEDILogSetting)
        {
            await _service.InsertSystemEDIFlowLog(systemEDILogSetting);
            return Ok();
        }
    }
}
