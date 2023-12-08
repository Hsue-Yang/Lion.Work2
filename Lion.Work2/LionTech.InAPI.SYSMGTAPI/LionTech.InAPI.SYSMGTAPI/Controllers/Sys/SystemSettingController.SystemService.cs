using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemSettingController
	{
        [HttpGet]
        [Route("{sysID}/SystemServices")]
        public async Task<IActionResult> QuerySystemServices([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemServiceList(sysID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("SystemService")]
        public async Task<IActionResult> EditSystemService([FromBody] SystemService systemService)
        {
            await _service.EditSystemServiceDetail(systemService, ClientUserID);
            return Ok();
        }

        [HttpDelete]
        [Route("SystemService/{sysID}/{subSysID}/{serviceID}")]
        public async Task<IActionResult> DeleteSystemService([FromRoute] string sysID, [FromRoute] string subSysID, [FromRoute] string serviceID)
        {
            await _service.DeleteSystemServiceDetail(sysID, subSysID, serviceID);
            return Ok();
        }
    }
}