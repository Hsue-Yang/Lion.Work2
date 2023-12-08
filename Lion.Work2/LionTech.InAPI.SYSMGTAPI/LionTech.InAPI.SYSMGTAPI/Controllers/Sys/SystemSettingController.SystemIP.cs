using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemSettingController
    {
        [HttpGet]
        [Route("{sysID}/SystemIPs")]
        public async Task<IActionResult> QuerySystemIPs([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemIPList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemIPs = result.systemIPList
            });
        }

        [HttpPost]
        [Route("SystemIP")]
        public async Task<IActionResult> EditSystemIP([FromBody] SystemIP systemIP)
        {
            await _service.EditSystemIPDetail(systemIP, ClientUserID);
            return Ok();
        }

        [HttpDelete]
        [Route("SystemIP/{sysID}/{subSysID}/{ipAddress}")]
        public async Task<IActionResult> DeleteSystemIP([FromRoute] string sysID, [FromRoute] string subSysID, [FromRoute] string ipAddress)
        {
            await _service.DeleteSystemIPDetail(sysID, subSysID, ipAddress);
            return Ok();
        }
    }
}