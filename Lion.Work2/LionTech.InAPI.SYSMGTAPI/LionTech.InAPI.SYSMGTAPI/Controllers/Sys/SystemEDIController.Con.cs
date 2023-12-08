using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;


namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {
        [HttpGet]
        [Route("Con/{sysID}")]
        public async Task<IActionResult> QuerySystemEDICons([FromRoute] string sysID, [FromQuery] string ediflowID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDICons(sysID, cultureID, ediflowID);
            return Ok(new
            {
                SystemEDIConList = result
            });
        }

        [HttpPost]
        [Route("Con/Sort")]
        public async Task<IActionResult> EditEDIConSort([FromBody] List<SystemEDICon> systemEDICon)
        {
            await _service.EditEDIConSort(systemEDICon);
            return Ok();
        }

        [HttpGet]
        [Route("Con/{sysID}/{ediflowID}/{ediconID}")]
        public async Task<IActionResult> QuerySystemEDIConDetail([FromRoute] string sysID, [FromRoute] string ediflowID, [FromRoute] string ediconID)
        {
            var result = await _service.GetSystemEDIConDetail(sysID, ediflowID, ediconID);
            return Ok(new
            {
                SystemEDIFlowDetail = result
            });
        }

        [HttpPost]
        [Route("Con")]
        public async Task<IActionResult> EditSystemEDIConDetail([FromBody] SystemEDIConDetail systemEDIConDetail)
        {
            await _service.EditSystemEDIConDetail(systemEDIConDetail);
            return Ok();
        }

        [HttpDelete]
        [Route("Con/{sysID}/{ediflowID}/{ediconID}")]
        public async Task<IActionResult> DeleteSystemEDIConDetail([FromRoute] string sysID, [FromRoute] string ediflowID, [FromRoute] string ediconID)
        {
            var result = await _service.DeleteSystemEDIConDetail(sysID, ediflowID, ediconID);
            if (result == EnumDeleteResult.Success || result == EnumDeleteResult.DataExist)
            {
                return Ok(result.ToString());
            }
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpGet]
        [Route("Con/{sysID}/{ediflowID}/Sort")]
        public async Task<IActionResult> QueryConNewSortOrder([FromRoute] string sysID, [FromRoute] string ediflowID)
        {
            var result = await _service.GetConNewSortOrder(sysID, ediflowID);
            return Ok(result);
        }

    }
}
